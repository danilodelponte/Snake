using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPower
{
    public SnakeSegment SnakeSegment { get; set; }

    public virtual void Activate() {}
    public virtual void Deactivate() {
        SnakeSegment.SpecialPower = null;
    }
    public virtual float SpecialMovement(float maxDeltaTime){ return maxDeltaTime; }
    public virtual bool SpecialCollision(SnakeSegment segmentCollided, Collider other) { return false; }
}
