using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteringRam : SpecialModifier
{
    private Vector3 safePosition;
    private bool crossingActive = false;

    public override void Activate() {
        Debug.Log($"{SnakeSegment.Snake} got a battering ram!");
    }

    public override void Deactivate() {
        base.Deactivate();
    }

    public override bool CollisionModifier(SnakeSegment segmentCollided, Collider other) {
        SnakeSegment otherSegment = other.gameObject.GetComponent<SnakeSegment>();
        if(otherSegment == null || segmentCollided == null) return false;
        if(segmentCollided.Snake == otherSegment.Snake) return false;

        if(!crossingActive && segmentCollided.IsHead) {
            safePosition = otherSegment.transform.position;
            crossingActive = true;
        }
        // If current segment is a tail, the crossing is finished
        if(segmentCollided.IsTail || otherSegment.IsTail || otherSegment.IsHead) Deactivate();
        if(otherSegment.transform.position == safePosition) return true;
        
        return false;
    }
}
