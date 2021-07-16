using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIControl : SnakeControl
{
    private Arena Arena { get => GetArena(); }
    private Arena arena;

    private Arena GetArena() {
        if(arena != null) return arena;

        return arena = GameObject.Find("Arena").GetComponent<Arena>();
    }

    public override Vector3 GetDirection() {
        PathNode startNode = Arena.GetNode(Snake.Head.transform.position);
        PathNode endNode = ClosestCollectable(startNode);
        Vector3 direction = Snake.GetComponent<SnakeMovement>().CurrentDirection;
        if(endNode == null) return direction;

        var obstaclesTypes = new Type[]{ typeof(SnakeSegment) };
        var pathFinding = new PathFinding(Arena, obstaclesTypes);
        List<PathNode> path = pathFinding.FindPath(startNode, endNode);
        if(path == null || path.Count < 2) return direction;

        PathNode nextMove = path[1];
        direction = startNode.DirectionTo(nextMove);
        return direction;
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
