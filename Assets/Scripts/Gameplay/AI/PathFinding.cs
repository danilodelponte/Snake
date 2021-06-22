using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding {

    private Arena arena;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    private List<PathNodeType> obstacleTypeList;

    public PathFinding(Arena arena, PathNodeType[] obstacleTypes = null) {
        if(obstacleTypes == null) obstacleTypes = new PathNodeType[0];
        obstacleTypeList = new List<PathNodeType>(obstacleTypes);
        this.arena = arena;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {
        PathNode startNode = arena.GetNode(startX, startY);
        PathNode endNode = arena.GetNode(endX, endY);
        return FindPath(startNode, endNode);
    }

    public List<PathNode> FindPath(PathNode startNode, PathNode endNode) {
        openList = new List<PathNode>() { startNode };
        closedList = new List<PathNode>();

        for(int x = 0; x < arena.Width; x++) {
            for(int y = 0; y < arena.Height; y++) {
                PathNode node = arena.GetNode(x, y);
                node.gCost = int.MaxValue;
                node.previousNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = DistanceCost(startNode, endNode);

        while(openList.Count > 0) {
            PathNode current = LowestFCostNode(openList);
            if(current == endNode) return PathFound(endNode);
            
            openList.Remove(current);
            closedList.Add(current);

            foreach(var node in current.Neighbours()) {
                if(node != endNode && obstacleTypeList.Contains(node.type)) {
                    closedList.Add(node);
                    continue;
                }
                if(closedList.Contains(node)) continue;

                int gCost = current.gCost + DistanceCost(current, node);
                if(gCost < node.gCost) {
                    node.previousNode = current;
                    node.gCost = gCost;
                    node.hCost = DistanceCost(node, endNode);
                }
                if(!openList.Contains(node)) {
                    openList.Add(node);
                }
            }
        }

        return null;
    }

    private List<PathNode> PathFound(PathNode endNode) {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode current = endNode;
        while(current.previousNode != null) {
            path.Add(current.previousNode);
            current = current.previousNode;
        }
        path.Reverse();
        return path;
    }

    private int DistanceCost(PathNode a, PathNode b) {
        return a.DistanceTo(b);
    }

    private PathNode LowestFCostNode(List<PathNode> pathNodes) {
        PathNode lowestFCost = pathNodes[0];
        foreach (var node in pathNodes) {
            if(node.fCost < lowestFCost.fCost) lowestFCost = node;
        }
        return lowestFCost;
    }
}
