using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confused : SpecialModifier
{
    private float timer;
    public float maxTime = 5f;

    public override void Activate() {
        Debug.Log($"{SnakeSegment} is confused!");
    }

    public override void Deactivate()
    {
        base.Deactivate();
        Debug.Log($"{SnakeSegment} is back to normal!");
    }

    public override void FixedUpdate() {
        timer += Time.deltaTime;
        if(timer > maxTime) Deactivate();
    }

    public override Vector3 DirectionModifier(Vector3 direction)
    {
        return direction * -1;
    }
}
