using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SnakeSegmentRepository {
    public static SnakeSegment Prefab { get => PrefabCache.Load<SnakeSegment>("SnakeSegment"); }

    public static SnakeSegment Build(Vector3 position, Quaternion rotation, Snake parentSnake) {
	    SnakeSegment segment = GameObject.Instantiate(Prefab, position, rotation, parentSnake.transform);
        return segment;
    }
}
