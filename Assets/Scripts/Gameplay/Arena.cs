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
    public PathNode[] SnakeSegmentNodes { get; private set; }
    public PathNode[] CollectableNodes { get; private set; }

    public void Generate(int width, int height){
        if(gridArray != null) return;

        this.width = width;
        this.height = height;

        gridArray = new PathNode[width, height];
        for(int x = 0; x < gridArray.GetLength(0); x++) {
            for(int y = 0; y < gridArray.GetLength(1); y++) {
                gridArray[x, y] = new PathNode(this, x, y);
            }
        }

        SpawnWalls();
    }

    public void UpdateGrid() {
        gridArray = new PathNode[width, height];
        UpdateSnakeNodes();
        UpdateCollectableNodes();
    }

    private void UpdateSnakeNodes() {
        GameObject[] snakeSegments = GameObject.FindGameObjectsWithTag("SnakeSegment");
        SnakeSegmentNodes = new PathNode[snakeSegments.Length];
        for (int i = 0; i < snakeSegments.Length; i++) {
            Transform segmentTransform = snakeSegments[i].transform;
            KeepWithinBounds(ref segmentTransform);
            Vector3 position = segmentTransform.position;
            PathNode node = new PathNode(this, position, PathNodeType.SNAKE);
            gridArray[(int) position.x, (int) position.y] = node;
            SnakeSegmentNodes[i] = node;
        }
    }

    private void UpdateCollectableNodes() {
        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");
        CollectableNodes = new PathNode[collectables.Length];
        for (int i = 0; i < collectables.Length; i++) {
            Transform collectableTransform = collectables[i].transform;
            KeepWithinBounds(ref collectableTransform);
            Vector3 position = collectableTransform.position;
            PathNode node = new PathNode(this, position, PathNodeType.COLLECTABLE);
            gridArray[(int) position.x, (int) position.y] = node;
            CollectableNodes[i] = node;
        }
    }

    public void KeepWithinBounds(ref Transform transform){
        Vector3 position = transform.position;
        position.x = position.x%width;
        if(position.x < 0) position.x = position.x + width;
        position.y = position.y%height;
        if(position.y < 0) position.y = position.y + height;
        transform.position = position;
    }

    private Quaternion DefaultRotation(){
        return Quaternion.Euler(Vector3.zero);
    }

    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y);
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
        List<PathNode> targetNodes = new List<PathNode>();
        Snake[] snakes = GameObject.FindObjectsOfType<Snake>();

        foreach(var snake in snakes) {
            PathNode headNode = GetNode(snake.Head.transform.position);
            targetNodes.Add(headNode);
        }

        Collectable[] collectables = GameObject.FindObjectsOfType<Collectable>();
        foreach (var collectable in collectables) {
            PathNode node = GetNode(collectable.transform.position);
            targetNodes.Add(node);
        }
        return targetNodes;
    }
    
    public const int sortingOrderDefault = 5000;
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 20, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault) {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public void GridDebug() {
        // if(debugTextArray != null) return;

        // debugTextArray = new TextMesh[width, height];
        for(int x = 0; x < gridArray.GetLength(0); x++) {
            for(int y = 0; y < gridArray.GetLength(1); y++) {
                Vector3 position = GetWorldPosition(x, y);
                // debugTextArray[x,y] = CreateWorldText(gridArray[x,y].ToString(), null, position, 5, Color.white, TextAnchor.MiddleCenter);
                position -= new Vector3(.5f, .5f);
                Debug.DrawLine(position, position + new Vector3(1, 0), Color.white, 100f);
                Debug.DrawLine(position, position + new Vector3(0, 1), Color.white, 100f);
            }
        }
        Debug.DrawLine(new Vector3(-.5f, height-.5f), new Vector3(width-.5f, height-.5f), Color.white, 100f);
        Debug.DrawLine(new Vector3(width-.5f, -.5f), new Vector3(width-.5f, height-.5f), Color.white, 100f);
    }
}
