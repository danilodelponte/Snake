using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : SpecialModifier
{
    public float Timer;
    public float MaxTime = 5f;
    
    public override void FixedUpdate() {
        // explode after some time
        Timer += Time.deltaTime;
        if(Timer > MaxTime) Deactivate();
    }

    public override void DirectionModifier(ref Vector3 direction){
        direction = Vector3.zero;
    }
}
