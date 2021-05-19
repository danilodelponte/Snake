using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathNodeType
{
    SNAKE,
    COLLECTABLE,
    FREE,
}

public class PathNode {

    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public int fCost { get => gCost + hCost; }
    public PathNodeType type = PathNodeType.FREE;

    private Arena arena;

    public PathNode previousNode;
    private List<PathNode> neighbours;

    public PathNode(Arena arena, Vector3 position, PathNodeType type = PathNodeType.FREE) {
        this.arena = arena;
        this.x = (int) position.x;
        this.y = (int) position.y;
    }

    public PathNode(Arena arena, int x, int y, PathNodeType type = PathNodeType.FREE) {
        this.arena = arena;
        this.x = x;
        this.y = y;
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

    public override string ToString() {
        return type.ToString().ToCharArray()[0] + "";
    }
}
