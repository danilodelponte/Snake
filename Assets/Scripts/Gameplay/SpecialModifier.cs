using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialModifier
{
    public SnakeSegment SnakeSegment { get; set; }

    public virtual void Activate() {}
    public virtual void Deactivate() {
        SnakeSegment.Modifier = null;
    }

    public virtual bool DeathModifier(){ return false; }
    public virtual Vector3 DirectionModifier(Vector3 direction){ return direction; }
    public virtual float MovementModifier(float maxDeltaTime){ return maxDeltaTime; }
    public virtual bool CollisionModifier(SnakeSegment segmentCollided, Collider other) { return false; }
    public virtual void FixedUpdate() {}
}
