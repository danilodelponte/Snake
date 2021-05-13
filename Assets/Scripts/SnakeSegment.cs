using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    public Vector3 CurrentDirection { get; set; }
    public SnakeSegment NextSegment { get; set; }

    public void Move(Vector3 direction) {
        transform.position += direction;
        if(NextSegment != null) NextSegment.Move(CurrentDirection);
        CurrentDirection = direction;
    }
}
