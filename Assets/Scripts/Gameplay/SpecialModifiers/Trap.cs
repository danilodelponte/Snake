using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : SpecialModifier
{
    private float timer;
    private float maxTime = 5f;

    public override void Activate()
    {
        Debug.Log($"{SnakeSegment.Snake} got trapped!");
        base.Activate();
    }
    
    public override void FixedUpdate() {
        // explode after some time
        timer += Time.deltaTime;
        if(timer > maxTime) {
            Debug.Log($"{SnakeSegment.Snake} is released!");
            Deactivate();
        }
    }

    public override Vector3 DirectionModifier(Vector3 direction){
        return Vector3.zero;
    }
}
