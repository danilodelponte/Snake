using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControl : SnakeControl
{
    public KeyCode LeftKey { get; set; }
    public KeyCode RightKey { get; set; }
    private Vector3 intendedDirection = Vector3.up;

    private Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

    public override Vector3 GetDirection()
    {
        return intendedDirection;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(LeftKey)) Turn(-1);
        else if (Input.GetKeyDown(RightKey)) Turn(+1);
    }

    private void Turn(int direction) {
        var dirIndex = Array.IndexOf(directions, Snake.Head.CurrentDirection);
        dirIndex += direction;
        if(dirIndex < 0) dirIndex = directions.Length -1;
        else if(dirIndex > directions.Length -1) dirIndex = 0;
        intendedDirection = directions[dirIndex];
    }

    public void SetKeys(KeyCode left, KeyCode right) {
        LeftKey = left;
        RightKey = right;
    }
}
