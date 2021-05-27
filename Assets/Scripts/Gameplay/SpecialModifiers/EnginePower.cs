using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnginePower : SpecialModifier
{
    private float movementDeltaDecrease = .03f;

    public override void Activate(GameplayController controller) {
        base.Activate(controller);
        Debug.Log($"{SnakeSegment.Snake} got engine power!");
    }

    // decreases time to move
    public override void MovementModifier(ref float maxDeltaTime){
        maxDeltaTime -= movementDeltaDecrease;
    }
}
