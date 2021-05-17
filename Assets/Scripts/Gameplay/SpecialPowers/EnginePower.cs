﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnginePower : SpecialPower
{
    private float movementDeltaDecrease = .03f;

    public override float EvaluateMovementDelta(float maxDeltaTime){
        return maxDeltaTime - movementDeltaDecrease;
    }
}