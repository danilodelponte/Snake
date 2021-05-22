using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleScore : SpecialModifier {

    private float timer;
    private float maxTime = 15f;

    public override void Activate()
    {
        Debug.Log("Doubles the score!");
        base.Activate();
    }

    public override void FixedUpdate() {
        // explode after some time
        timer += Time.deltaTime;
        if(timer > maxTime) Deactivate();
    }

    public override int ScoreGainModifier(int gain) {
        return gain * 2;
    }
}
