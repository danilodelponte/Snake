using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegmentSnapshot {

    private static GameObject segmentPrefab = Resources.Load("Prefabs/SnakeSegment") as GameObject;

    public SnakeSegmentSnapshot NextSegment { get; }
    public Vector3 Position { get; }
    public Quaternion Rotation { get; } 
    public Vector3 CurrentDirection { get; }
    public SpecialPower SpecialPower { get; }

    public SnakeSegmentSnapshot(SnakeSegment segment) {
        if(segment.NextSegment != null) NextSegment = new SnakeSegmentSnapshot(segment.NextSegment);
        CurrentDirection = segment.CurrentDirection;
        Position = segment.transform.position;
        Rotation = segment.transform.rotation;
        SpecialPower = segment.SpecialPower;
    }

    public SnakeSegment LoadHead(Snake parent) {
        SnakeSegment segment = Load(parent);
        parent.Head = segment;
        LoadPowers(segment);
        return segment;
    }

    private SnakeSegment Load(Snake parent) { 
        GameObject go = (GameObject) GameObject.Instantiate(segmentPrefab, Position, Rotation, parent.transform);
        SnakeSegment segment = (SnakeSegment) go.GetComponent<SnakeSegment>();
        if(NextSegment != null) segment.NextSegment = NextSegment.Load(parent);
        segment.CurrentDirection = CurrentDirection;
        return segment;
    }

    private void LoadPowers(SnakeSegment segment){
        segment.SpecialPower = SpecialPower;
        if(!segment.IsTail) segment.NextSegment.SpecialPower = NextSegment.SpecialPower;
    }

}
