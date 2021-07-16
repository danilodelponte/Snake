using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteringRam : SpecialModifier
{
    private Vector3 safePosition;
    private bool crossing = false;
    // MOVER PARA SCRIPTABLE

    public override void MovementModifier(ref float _){
        if(!crossing) return;

        Snake snake = Segment.Snake;
        SnakeSegment segment = snake.Head;
        while(segment != null) {
            // while there is a segment at the safe position, keeps crossing
            if(segment.transform.position == safePosition) return;
            segment = segment.NextSegment;
        }
        crossing = false;
        Deactivate();
    }

    public override bool CollisionModifier(SnakeSegment segmentCollided, GameObject other) {
        SnakeSegment otherSegment = other.GetComponent<SnakeSegment>();
        if(otherSegment == null) return false;

        // if head collides activates crossing
        Snake snake = segmentCollided.Snake;
        if(!crossing && snake.Head == segmentCollided) {
            Debug.Log("crossing!");
            safePosition = otherSegment.transform.position;
            crossing = true;
        }

        // if object collided is not at safe position, must collide
        if(crossing && otherSegment.transform.position != safePosition) {
            Deactivate();
            return false;
        }

        return crossing;
    }
}
