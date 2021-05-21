using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnginePower : SpecialPower
{
    private float movementDeltaDecrease = .03f;

    public override void Activate()
    {
        Debug.Log($"{SnakeSegment.Snake} got engine power!");
        base.Activate();
    }

    public override float SpecialMovement(float maxDeltaTime){
        return maxDeltaTime - movementDeltaDecrease;
    }
}
