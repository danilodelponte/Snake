using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleScore : SpecialModifier {

    public float Timer;
    public float MaxTime = 15f;

    public override void FixedUpdate() {
        Timer += Time.fixedDeltaTime;
        if(Timer > MaxTime) Deactivate();
    }

    // multiplies score by 2
    public override void ScoreGainModifier(ref int gain) {
        gain *= 2;
    }
}
