using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteringRam : SpecialModifier
{
    private Vector3 safePosition;
    private bool crossing = false;

    public override void Activate(GameplayController controller) {
        base.Activate(controller);
        Debug.Log($"{SnakeSegment.Snake.name} got a battering ram!");
    }

    public override void Deactivate() {
        Debug.Log($"{SnakeSegment.Snake.name} battering ram used.");
        SnakeSegment = null;
    }

    public override void MovementModifier(ref float _){
        if(!crossing) return;

        SnakeSegment segment = SnakeSegment.Snake.Head;
        while(segment != null) {
            // while there is a segment at the safe position, keeps crossing
            if(segment.transform.position == safePosition) return;
            segment = segment.NextSegment;
        }
        crossing = false;
        Deactivate();
    }

    public override bool CollisionModifier(SnakeSegment segmentCollided, Collider other) {
        SnakeSegment otherSegment = other.gameObject.GetComponent<SnakeSegment>();
        if(otherSegment == null) return false;

        // if head collides activates crossing
        if(!crossing && segmentCollided.IsHead) {
            Debug.Log($"{SnakeSegment.Snake.name} is crossing!");
            safePosition = otherSegment.transform.position;
            crossing = true;
        }

        // if object collided is not at safe position, must collide
        if(crossing && otherSegment.transform.position != safePosition) return false;

        return crossing;
    }
}
