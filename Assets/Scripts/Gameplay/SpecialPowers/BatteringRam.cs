using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteringRam : SpecialPower
{
    private Vector3 safePosition;
    private bool crossingActive = false;

    public override void Activate() {
        Debug.Log($"{SnakeSegment.ParentSnake} got a battering ram!");
    }

    public override void Deactivate()
    {
        base.Deactivate();
        Debug.Log($"{SnakeSegment.ParentSnake} battering ram used.");
    }

    public override bool SpecialCollision(SnakeSegment segmentCollided, Collider other) {
        SnakeSegment otherSegment = other.gameObject.GetComponent<SnakeSegment>();
        if(otherSegment == null) return false;

        if(!crossingActive && segmentCollided.IsHead) {
            safePosition = otherSegment.transform.position;
            crossingActive = true;
        }
        Debug.Log($"{SnakeSegment.ParentSnake} is crossing!");
        if(segmentCollided.IsTail || otherSegment.IsTail || otherSegment.IsHead) Deactivate();
        if(otherSegment.transform.position == safePosition) return true;
        
        return false;
    }
}
