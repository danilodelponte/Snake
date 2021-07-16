using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confused : SpecialModifier
{
    public float Timer;
    public float MaxTime = 5f;

    public override void FixedUpdate() {
        Timer += Time.fixedDeltaTime;
        if(Timer > MaxTime) Deactivate();
    }

    // Invert keys direction
    public override void DirectionModifier(ref Vector3 direction) {
        direction *= -1;
    }
}
