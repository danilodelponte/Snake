using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private int width = 40;
    [SerializeField] private int height = 20;

    public int Width { get { return width; } }
    public int Height { get { return height; } }
    private PathNode[,] gridArray;
    private TextMesh[,] debugTextArray;

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

    public List<PathNode> GetNodes(PathNodeType type){
        List<PathNode> nodes = new List<PathNode>();
        PathNode node;
        for(int x = 0; x < gridArray.GetLength(0); x++) {
            for(int y = 0; y < gridArray.GetLength(1); y++) {
                node = gridArray[x, y];
                if(node.type == type) nodes.Add(node);
            }
        }
        return nodes;
    } 

    private Quaternion DefaultRotation(){
        return Quaternion.Euler(Vector3.zero);
    }

    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y);
    }

    public void SetNode(Vector3 position, PathNodeType type) {
        gridArray[(int)position.x, (int)position.y].type = type;
        // debugTextArray[(int)position.x ,(int)position.y].text = type.ToString().ToCharArray()[0] + "";
        Color color = Color.white;
        if(type == PathNodeType.SNAKE) color = Color.green;
        if(type == PathNodeType.COLLECTABLE) color = Color.red;
        // debugTextArray[(int)position.x ,(int)position.y].color = color;
    }

    public PathNode GetNode(Vector3 position) {
        return GetNode((int)position.x, (int)position.y);
    }

    public PathNode GetNode(int x, int y) {
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

    public Vector3 RandomPosition(){
        return new Vector3Int(Random.Range(0, width-1), Random.Range(0, height-1), 0);
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
