using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeControl : MonoBehaviour
{
    protected Snake Snake { get => GetSnake(); }

    public virtual Vector3 GetDirection() {
        return Vector3.up;
    }

    private Snake GetSnake() {
        return gameObject.GetComponent<Snake>();
    }
}
