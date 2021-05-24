using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    private static Portal prefab;
    public static Portal Prefab { get => LoadPrefab(); }

    public Portal OtherEnd { get; set; }
    public Vector3 TeleportFilter { get; set; }
    public Vector3 TeleportOffset { get; set; }

    private static Portal LoadPrefab() {
        if(prefab == null) prefab = Resources.Load<Portal>("Prefabs/WallPortal");
        return prefab;
    }
    
    public void Teleport(SnakeSegment segment) {
        Vector3 positionDiff = OtherEnd.transform.position - segment.transform.position;
        positionDiff = Vector3.Scale(positionDiff, TeleportFilter);
        segment.transform.position += positionDiff + TeleportOffset;
    }
}
