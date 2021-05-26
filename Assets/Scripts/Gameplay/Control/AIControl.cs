using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIControl : SnakeControl
{
    private Arena Arena { get => GetArena(); }
    private Arena _arena;

    private Arena GetArena() {
        if(_arena != null) return _arena;

        return _arena = GameObject.Find("Arena").GetComponent<Arena>();
    }

    // Update is called once per frame
    public override Vector3 GetDirection()
    {
        PathNode startNode = Arena.GetNode(Snake.Head.transform.position);
        List<PathNode> collectableNodes = Arena.CollectableNodes;
        collectableNodes.Sort(Comparer<PathNode>.Create(
            (pn1, pn2) => startNode.DistanceTo(pn1) - startNode.DistanceTo(pn2)
        ));

        if(collectableNodes.Count == 0) return Snake.Head.CurrentDirection;

        PathNode endNode = collectableNodes[0];
        var pathFinding = new PathFinding(Arena);

        List<PathNode> path = pathFinding.FindPath(startNode, endNode);
        if(path == null || path.Count < 2) return Snake.Head.CurrentDirection;

        PathNode nextMove = path[1];
        int xDistance = nextMove.x - startNode.x;
        int xLoopDistance = Arena.Width - Mathf.Abs(xDistance);
        if(xLoopDistance < Mathf.Abs(xDistance)) xDistance *= -1;

        int yDistance = nextMove.y - startNode.y;
        int yLoopDistance = Arena.Height - Mathf.Abs(yDistance);
        if(yLoopDistance < Mathf.Abs(yDistance)) yDistance *= -1;
        return new Vector3(xDistance, yDistance).normalized;
    }
}
