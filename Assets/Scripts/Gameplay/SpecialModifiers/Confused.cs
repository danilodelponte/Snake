using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confused : SpecialModifier
{
    private float timer;
    public float maxTime = 5f;

    public override void Activate(GameplayController controller) {
        base.Activate(controller);
        Debug.Log($"{SnakeSegment} is confused!");
    }

    public override void Deactivate()
    {
        Debug.Log($"{SnakeSegment} is back to normal!");
        base.Deactivate();
    }

    public override void FixedUpdate() {
        timer += Time.deltaTime;
        if(timer > maxTime) Deactivate();
    }

    // Invert keys direction
    public override void DirectionModifier(ref Vector3 direction) {
        direction *= -1;
    }
}
