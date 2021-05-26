using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public static Portal Prefab { get => PrefabCache.Load<Portal>("WallPortal"); }

    public Portal OtherEnd { get; set; }
    public Vector3 TeleportFilter { get; set; }
    public Vector3 TeleportOffset { get; set; }
    
    public void Teleport(SnakeSegment segment) {
        Vector3 positionDiff = OtherEnd.transform.position - segment.transform.position;
        positionDiff = Vector3.Scale(positionDiff, TeleportFilter);
        segment.transform.position += positionDiff + TeleportOffset;
    }
}
