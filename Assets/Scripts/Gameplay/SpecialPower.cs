using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPower
{
    public virtual float EvaluateMovementDelta(float maxDeltaTime){ return maxDeltaTime; }
    public virtual void Activate(){ }
    public virtual bool HandleCollision(SnakeSegment segment, Collider other) { return false; }
}
