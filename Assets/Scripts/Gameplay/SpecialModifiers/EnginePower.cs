using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnginePower : SpecialModifier
{
    private float movementDeltaDecrease = .03f;

    public override void Activate()
    {
        Debug.Log($"{SnakeSegment.Snake} got engine power!");
        base.Activate();
    }

    public override float MovementModifier(float maxDeltaTime){
        return maxDeltaTime - movementDeltaDecrease;
    }
}
