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
        UpdateGrid();
        SpawnWalls();
    }

    public void UpdateGrid() {
        gridArray = new PathNode[width, height];
        UpdateSnakeNodes();
        UpdateCollectableNodes();
    }

    private void UpdateSnakeNodes() {
        GameObject[] snakeSegments = GameObject.FindGameObjectsWithTag("SnakeSegment");
        SnakeHeadNodes = new List<PathNode>();
        for (int i = 0; i < snakeSegments.Length; i++) {
            SnakeSegment segment = snakeSegments[i].GetComponent<SnakeSegment>();
            Transform segmentTransform = segment.transform;
            KeepWithinBounds(ref segmentTransform);
            Vector3 position = segmentTransform.position;
            PathNode node = new PathNode(this, position, PathNodeType.SNAKE);
            gridArray[(int) position.x, (int) position.y] = node;
            if(segment.IsHead) SnakeHeadNodes.Add(node);
        }
    }

    private void UpdateCollectableNodes() {
        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");
        CollectableNodes = new List<PathNode>();
        for (int i = 0; i < collectables.Length; i++) {
            Transform collectableTransform = collectables[i].transform;
            KeepWithinBounds(ref collectableTransform);
            Vector3 position = collectableTransform.position;
            PathNode node = new PathNode(this, position, PathNodeType.COLLECTABLE);
            gridArray[(int) position.x, (int) position.y] = node;
            CollectableNodes.Add(node);
        }
    }

    public void KeepWithinBounds(ref Transform transform){
        Vector3 position = transform.position;
        position.x = position.x % width;
        if(position.x < 0) position.x = position.x + width;
        position.y = position.y % height;
        if(position.y < 0) position.y = position.y + height;
        transform.position = position;
    }

    public PathNode GetNode(Vector3 position) {
        return GetNode((int)position.x, (int)position.y);
    }

    public PathNode GetNode(int x, int y) {
        if(gridArray[x, y] == null) gridArray[x, y] = new PathNode(this, x, y);

        return gridArray[x, y];
    }

    private void SpawnWalls() {
        Portal leftWall = Instantiate(Portal.Prefab, transform);
        leftWall.transform.localScale = new Vector3(1, height, 1);
        leftWall.transform.position = new Vector3(-1, height/2 - .5f, 0);
        leftWall.TeleportFilter = new Vector3(1, 0, 0);
        leftWall.TeleportOffset = Vector3.left;

        Portal rightWall = Instantiate(Portal.Prefab, transform);
        rightWall.transform.localScale = new Vector3(1, height, 1);
        rightWall.transform.position = new Vector3(width, height/2 - .5f, 0);
        rightWall.TeleportFilter = new Vector3(1, 0, 0);
        rightWall.TeleportOffset = Vector3.right;

        leftWall.OtherEnd = rightWall;
        rightWall.OtherEnd = leftWall;

        Portal upperWall = Instantiate(Portal.Prefab, transform);
        upperWall.transform.localScale = new Vector3(width, 1, 1);
        upperWall.transform.position = new Vector3(width/2 - .5f, height, 0);
        upperWall.TeleportFilter = new Vector3(0, 1, 0);
        upperWall.TeleportOffset = Vector3.up;

        Portal bottomWall = Instantiate(Portal.Prefab, transform);
        bottomWall.transform.localScale = new Vector3(width, 1, 1);
        bottomWall.transform.position = new Vector3(width/2 - .5f, -1, 0);
        bottomWall.TeleportFilter = new Vector3(0, 1, 0);
        bottomWall.TeleportOffset = Vector3.down;

        upperWall.OtherEnd = bottomWall;
        bottomWall.OtherEnd = upperWall;
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
                if(currentNode.type != PathNodeType.FREE) continue;

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
