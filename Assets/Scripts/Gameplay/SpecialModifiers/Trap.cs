using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : SpecialModifier
{
    private float timer;
    public float maxTime = 5f;

    public override void Activate(GameplayController controller) {
        base.Activate(controller);
        Debug.Log($"{SnakeSegment} got trapped!");
    }

    public override void Deactivate() {
        Debug.Log($"{SnakeSegment} is released!");
        SnakeSegment = null;
    }
    
    public override void FixedUpdate() {
        // explode after some time
        timer += Time.deltaTime;
        if(timer > maxTime) Deactivate();
    }

    public override void DirectionModifier(ref Vector3 direction){
        direction = Vector3.zero;
    }
}
