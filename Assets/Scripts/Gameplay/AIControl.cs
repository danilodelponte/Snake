using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIControl : MonoBehaviour
{
    private Snake snake;
    private Arena arena;

    private Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

    private void Awake() {
        snake = gameObject.GetComponent<Snake>();
    }

    private void Start() {
        arena = GameObject.Find("Arena").GetComponent<Arena>();
    }

    // Update is called once per frame
    public Vector3 GetDirection()
    {
        PathNode startNode = arena.GetNode(snake.Head.transform.position);
        List<PathNode> collectableNodes = arena.GetNodes(PathNodeType.COLLECTABLE);
        collectableNodes.Sort(Comparer<PathNode>.Create((pn1, pn2) => pn1.DistanceTo(pn2)));

        if(collectableNodes.Count == 0) return snake.Direction;

        PathNode endNode = collectableNodes[0];
        var pathFinding = new PathFinding(arena);

        List<PathNode> path = pathFinding.FindPath(startNode, endNode);
        if(path == null) return snake.Direction;

        PathNode nextMove = path[1];
        int xDistance = nextMove.x - startNode.x;
        int xLoopDistance = arena.Width - Mathf.Abs(xDistance);
        if(xLoopDistance < Mathf.Abs(xDistance)) xDistance *= -1;

        int yDistance = nextMove.y - startNode.y;
        int yLoopDistance = arena.Height - Mathf.Abs(yDistance);
        if(yLoopDistance < Mathf.Abs(yDistance)) yDistance *= -1;
        return new Vector3(xDistance, yDistance).normalized;
    }
}
