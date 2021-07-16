using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    public static Portal Prefab { get => PrefabCache.Load<Portal>("WallPortal"); }
}
