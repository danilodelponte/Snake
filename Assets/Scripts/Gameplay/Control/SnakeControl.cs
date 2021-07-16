using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeControl : MonoBehaviour
{
    protected Snake Snake { get => GetSnake(); }
    public Vector3 DefaultDirection { get => Vector3.up; }

    public virtual Vector3 GetDirection() {
        return DefaultDirection;
    }

    private Snake GetSnake() {
        return gameObject.GetComponent<Snake>();
    }
}
