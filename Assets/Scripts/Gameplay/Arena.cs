using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private int width = 40;
    [SerializeField] private int height = 20;

    public int Width { get => width; set => width = value;}
    public int Height { get => height; set => height = value;}
    private PathNode[,] gridArray;
    private TextMesh[,] debugTextArray;
    public List<PathNode> SnakeHeadNodes { get; private set; }
    public List<PathNode> CollectableNodes { get; private set; }

    public void Generate(int width, int height){
        if(gridArray != null) return;
        this.width = width;
        this.height = height;
        SpawnWalls();
    }

    public void UpdateGrid() {
        gridArray = new PathNode[width, height];
        UpdateSnakeNodes(SnakeRepository.All());
        UpdateCollectableNodes(CollectableRepository.All());
    }

    private void UpdateSnakeNodes(IEnumerable<Snake> snakes) {
        SnakeHeadNodes = new List<PathNode>();

        foreach (var snake in snakes) {
            foreach (SnakeSegment segment in snake.Segments()) {
                Transform segmentTransform = segment.transform;
                Vector3 position = segmentTransform.position;
                var node = new PathNode(this, position, segment.gameObject);
                gridArray[(int) position.x, (int) position.y] = node;
                if(segment == snake.Head) SnakeHeadNodes.Add(node);
            }    
        }
    }

    private void UpdateCollectableNodes(IEnumerable<Collectable> collectables) {
        CollectableNodes = new List<PathNode>();
        foreach (var collectable in collectables) {
            Transform collectableTransform = collectable.transform;
            Vector3 position = collectableTransform.position;
            PathNode node = new PathNode(this, position, collectable.gameObject);
            gridArray[(int) position.x, (int) position.y] = node;
            CollectableNodes.Add(node);
        }
    }

    public void KeepWithinBounds(ref Vector3 position){
        position.x = position.x % width;
        if(position.x < 0) position.x = position.x + width;
        position.y = position.y % height;
        if(position.y < 0) position.y = position.y + height;
    }

    public PathNode GetNode(Vector3 position) {
        return GetNode((int)position.x, (int)position.y);
    }

    public PathNode GetNode(int x, int y) {
        if(gridArray[x, y] == null) gridArray[x, y] = new PathNode(this, x, y);

        return gridArray[x, y];
    }

    public void SetNode(Vector3 position, Object nodeObject) {
        SetNode((int)position.x, (int)position.y, nodeObject);
    }

    public void SetNode(int x, int y, Object nodeObject) {
        gridArray[x, y] = new PathNode(this, x, y, nodeObject);
    }

    private void SpawnWalls() {
        Portal leftWall = Instantiate(Portal.Prefab, transform);
        leftWall.transform.localScale = new Vector3(1, height, 1);
        leftWall.transform.position = new Vector3(-1, height/2 - .5f, 0);

        Portal rightWall = Instantiate(Portal.Prefab, transform);
        rightWall.transform.localScale = new Vector3(1, height, 1);
        rightWall.transform.position = new Vector3(width, height/2 - .5f, 0);

        Portal upperWall = Instantiate(Portal.Prefab, transform);
        upperWall.transform.localScale = new Vector3(width, 1, 1);
        upperWall.transform.position = new Vector3(width/2 - .5f, height, 0);

        Portal bottomWall = Instantiate(Portal.Prefab, transform);
        bottomWall.transform.localScale = new Vector3(width, 1, 1);
        bottomWall.transform.position = new Vector3(width/2 - .5f, -1, 0);
    }

    public Vector3Int RandomPosition() {
        return new Vector3Int(Random.Range(0, width-1), Random.Range(0, height-1), 0);
    }

    // finds a position with an even distance between all snake heads and collectables
    public Vector3 EquallyDistributedPosition() {
        PathNode[] targetNodes = GetTargetNodes().ToArray();
        PathNode maxMinDistanceNode = null;
        PathNode currentNode;
        int minDistance = int.MaxValue;
        int maxMinDistance = 0;
        Vector3Int start = RandomPosition();
        int xx, yy;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {

                xx = start.x + x;
                if(xx > width -1) xx -= width;
                yy = start.y + y;
                if(yy > height -1) yy -= height;

                currentNode = GetNode(xx, yy);
                if(currentNode.NodeObject != null) continue;

                minDistance = int.MaxValue;
                foreach (var node in targetNodes) {
                    int distance = currentNode.DistanceTo(node);
                    if(distance < minDistance) minDistance = distance;
                }

                if(minDistance > maxMinDistance) {
                    maxMinDistance = minDistance;
                    maxMinDistanceNode = currentNode;
                }
            }
        }

        if(maxMinDistanceNode == null) return start;
        return new Vector3(maxMinDistanceNode.x, maxMinDistanceNode.y);
    }

    public List<PathNode> GetTargetNodes() {
        UpdateGrid();
        List<PathNode> targetList = new List<PathNode>();
        targetList.AddRange(SnakeHeadNodes);
        targetList.AddRange(CollectableNodes);
        return targetList;
    }

    public void GridDebug() {
        for(int x = 0; x < gridArray.GetLength(0); x++) {
            for(int y = 0; y < gridArray.GetLength(1); y++) {
                Vector3 position = new Vector3(x, y);
                position -= new Vector3(.5f, .5f);
                Debug.DrawLine(position, position + new Vector3(1, 0), Color.white, 100f);
                Debug.DrawLine(position, position + new Vector3(0, 1), Color.white, 100f);
            }
        }
        Debug.DrawLine(new Vector3(-.5f, height-.5f), new Vector3(width-.5f, height-.5f), Color.white, 100f);
        Debug.DrawLine(new Vector3(width-.5f, -.5f), new Vector3(width-.5f, height-.5f), Color.white, 100f);
    }
}
