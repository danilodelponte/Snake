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

    public override Vector3 GetDirection() {
        PathNode startNode = Arena.GetNode(Snake.Head.transform.position);
        PathNode endNode = ClosestCollectable(startNode);
        if(endNode == null) return Snake.Head.CurrentDirection;

        var pathFinding = new PathFinding(Arena, new PathNodeType[] { PathNodeType.SNAKE });
        List<PathNode> path = pathFinding.FindPath(startNode, endNode);
        if(path == null || path.Count < 2) return Snake.Head.CurrentDirection;

        PathNode nextMove = path[1];
        return startNode.DirectionTo(nextMove);
    }

    private PathNode ClosestCollectable(PathNode startNode) { 
        List<PathNode> collectableNodes = Arena.CollectableNodes;
        collectableNodes.Sort(Comparer<PathNode>.Create(
            (pn1, pn2) => startNode.DistanceTo(pn1) - startNode.DistanceTo(pn2)
        ));

        if(collectableNodes.Count == 0) return null;
        else return collectableNodes[0];
    }
}
