using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Snake snake;
    void Start()
    {
        snake = gameObject.GetComponent<Snake>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
             direction = Vector3.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            direction = Vector3.down;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            direction = Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            direction = Vector3.right;
        }

        if(direction != Vector3.zero) snake.SetDirection(direction);
    }
}
