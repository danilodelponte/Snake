using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    public Vector3 CurrentDirection { get; set; }
    public SnakeSegment NextSegment { get; set; }
    private Snake parentSnake;
    public Snake ParentSnake { get { return parentSnake; } }

    private void Awake() {
        parentSnake = transform.parent.gameObject.GetComponent<Snake>();
    }

    public void Move(Vector3 direction) {
        transform.position += direction;
        if(NextSegment != null) NextSegment.Move(CurrentDirection);
        CurrentDirection = direction;
    }
    private void OnTriggerEnter(Collider other) {
        var gameplay = GameplayController.Singleton;
        gameplay.HandleCollision(this, other);
    }

    public bool IsHead() {
        return ParentSnake.Head == this;
    }
}
