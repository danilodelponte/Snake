using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private Portal portalPrefab;
    [SerializeField] private int width = 40;
    [SerializeField] private int height = 20;

    public int Width { get { return width; } }
    public int Height { get { return height; } }

    private void Awake() {
        InitWalls();
    }

    private void InitWalls() {
        Portal leftWall = Instantiate(portalPrefab, transform);
        leftWall.transform.localScale = new Vector3(1, height, 1);
        leftWall.transform.position = new Vector3(-width/2, 0, 0);
        leftWall.TransportFilter = new Vector3(1, 0, 0);
        leftWall.OffSet = Vector3.left;

        Portal rightWall = Instantiate(portalPrefab, transform);
        rightWall.transform.localScale = new Vector3(1, height, 1);
        rightWall.transform.position = new Vector3(width/2, 0, 0);
        rightWall.TransportFilter = new Vector3(1, 0, 0);
        rightWall.OffSet = Vector3.right;

        leftWall.OtherEnd = rightWall;
        rightWall.OtherEnd = leftWall;

        Portal upperWall = Instantiate(portalPrefab, transform);
        upperWall.transform.localScale = new Vector3(width, 1, 1);
        upperWall.transform.position = new Vector3(0, height/2, 0);
        upperWall.TransportFilter = new Vector3(0, 1, 0);
        upperWall.OffSet = Vector3.up;

        Portal bottomWall = Instantiate(portalPrefab, transform);
        bottomWall.transform.localScale = new Vector3(width, 1, 1);
        bottomWall.transform.position = new Vector3(0, -height/2, 0);
        bottomWall.TransportFilter = new Vector3(0, 1, 0);
        bottomWall.OffSet = Vector3.down;

        upperWall.OtherEnd = bottomWall;
        bottomWall.OtherEnd = upperWall;
    }

    public Vector3 RandomPosition(){
        Vector3Int min = new Vector3Int(-width/2 +1, -height/2 +1, 0);
        Vector3Int max = new Vector3Int(width/2 -1, height/2 -1, 0);
        return new Vector3Int(Random.Range(min.x, max.x), Random.Range(min.y, max.y), 0);
    }
}
