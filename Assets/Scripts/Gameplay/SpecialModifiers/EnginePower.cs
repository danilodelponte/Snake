using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnginePower : SpecialModifier
{
    private float movementDeltaDecrease = .03f;

    // decreases time to move
    public override void MovementModifier(ref float maxDeltaTime){
        maxDeltaTime -= movementDeltaDecrease;
    }
}
