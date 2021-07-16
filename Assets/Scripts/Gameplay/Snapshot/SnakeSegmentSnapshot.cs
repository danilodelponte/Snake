using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegmentSnapshot {

    private static GameObject segmentPrefab = (GameObject) Resources.Load("Prefabs/SnakeSegment");

    public SnakeSegmentSnapshot NextSegment { get; }
    public Vector3 Position { get; }
    public Quaternion Rotation { get; } 
    public Vector3 CurrentDirection { get; }
    public SpecialModifierSnapshot Modifier { get; }

    public SnakeSegmentSnapshot(SnakeSegment segment) {
        if(segment.NextSegment != null) NextSegment = new SnakeSegmentSnapshot(segment.NextSegment);
        Position = segment.transform.position;
        Rotation = segment.transform.rotation;
        Modifier = new SpecialModifierSnapshot(segment.Modifier);
    }

    public SnakeSegment Load(Snake parentSnake) { 
        SnakeSegment segment = SnakeSegmentRepository.Build(Position, Rotation, parentSnake);
        if(NextSegment != null) segment.NextSegment = NextSegment.Load(parentSnake);
        // Modifier.Load(segment);

        return segment;
    }
}
