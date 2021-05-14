using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControl : MonoBehaviour
{
    private Snake snake;
    public KeyCode LeftKey { get; set; }
    public KeyCode RightKey { get; set; }

    private Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

    void Awake()
    {
        snake = gameObject.GetComponent<Snake>();
    }

    // Update is called once per frame
    void Update()
    {
        var dirIndex = Array.IndexOf(directions, snake.Direction);
        if (Input.GetKeyDown(LeftKey)) {
            dirIndex--;
            if(dirIndex < 0) dirIndex = directions.Length -1;
        }
        if (Input.GetKeyDown(RightKey)) {
            dirIndex++;
            if(dirIndex > directions.Length -1) dirIndex = 0;
        }
        snake.SetDirection(directions[dirIndex]);
    }
}
