using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    public Vector3 CurrentDirection { get; set; }
    public SnakeSegment NextSegment { get; set; }

    public Snake ParentSnake {
        get { return transform.parent.gameObject.GetComponent<Snake>(); }
    }

    public void Move(Vector3 direction) {
        transform.position += direction;
        if(NextSegment != null) NextSegment.Move(CurrentDirection);
        CurrentDirection = direction;
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<Fruit>() != null) {
            var gameplay = GameplayController.Singleton;
            gameplay.SnakeEatsFruit(this, other.gameObject.GetComponent<Fruit>());
        }
    }
}
