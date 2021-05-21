﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confused : SpecialPower
{
    private float timer;
    private float maxTime = 5f;

    public override void Activate() {
        Debug.Log($"{SnakeSegment.Snake} is confused!");
    }

    public override void Deactivate()
    {
        base.Deactivate();
        Debug.Log($"{SnakeSegment.Snake} is back to normal!");
    }

    public override void FixedUpdate() {
        timer += Time.deltaTime;
        if(timer > maxTime) Deactivate();
    }

    public override Vector3 SpecialDirection(Vector3 direction)
    {
        return direction * -1;
    }
}
