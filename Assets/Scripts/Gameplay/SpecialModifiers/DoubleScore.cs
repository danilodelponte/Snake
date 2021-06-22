using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleScore : SpecialModifier {

    private float timer;
    private float maxTime = 15f;

    public override void Activate(GameplayController controller) {
        base.Activate(controller);
        Debug.Log("Doubles the score!");
    }

    public override void Deactivate() {
        SnakeSegment = null;
    }

    public override void FixedUpdate() {
        timer += Time.deltaTime;
        if(timer > maxTime) Deactivate();
    }

    // multiplies score by 2
    public override void ScoreGainModifier(ref int gain) {
        gain *= 2;
    }
}
