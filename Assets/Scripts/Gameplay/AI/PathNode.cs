using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {
    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public int fCost { get => gCost + hCost; }

    private Arena arena;

    public PathNode previousNode;
    private List<PathNode> neighbours;
    private Object nodeObject;
    public Object NodeObject { get => nodeObject; }

    public PathNode(Arena arena, Vector3 position, Object nodeObject = null) {
        this.arena = arena;
        this.x = (int) position.x;
        this.y = (int) position.y;
        this.nodeObject = nodeObject;
    }

    public PathNode(Arena arena, int x, int y, Object nodeObject = null) {
        this.arena = arena;
        this.x = x;
        this.y = y;
        this.nodeObject = nodeObject;
    }

    public List<PathNode> Neighbours() {
        if(neighbours != null) return neighbours;

        neighbours = new List<PathNode>();
        if(x-1 >= 0) neighbours.Add(arena.GetNode(x-1, y));
        else neighbours.Add(arena.GetNode(arena.Width-1, y));

        if(x+1 < arena.Width) neighbours.Add(arena.GetNode(x+1, y));
        else neighbours.Add(arena.GetNode(0, y));

        if(y-1 >= 0) neighbours.Add(arena.GetNode(x, y-1));
        else neighbours.Add(arena.GetNode(x, arena.Height-1));

        if(y+1 < arena.Height) neighbours.Add(arena.GetNode(x, y+1));
        else neighbours.Add(arena.GetNode(x, 0));
        
        return neighbours;
    }

    public int DistanceTo(PathNode other) {
        int xDistance = Mathf.Abs(x - other.x);
        int yDistance = Mathf.Abs(y - other.y);
        xDistance = Mathf.Min(xDistance, arena.Width - xDistance);
        yDistance = Mathf.Min(yDistance, arena.Height - yDistance);
        return xDistance + yDistance;
    }

    public Vector3 DirectionTo(PathNode other) {
        int xDirection = other.x - this.x;
        int xReverseDirection = arena.Width - Mathf.Abs(xDirection);
        if(xReverseDirection < Mathf.Abs(xDirection)) xDirection *= -1;

        int yDirection = other.y - this.y;
        int yReverseDirection = arena.Height - Mathf.Abs(yDirection);
        if(yReverseDirection < Mathf.Abs(yDirection)) yDirection *= -1;
        return new Vector3(xDirection, yDirection).normalized;
    }
}