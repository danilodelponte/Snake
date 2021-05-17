using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    [SerializeField] private float movingDeltaIncrease = .01f;

    public Vector3 CurrentDirection { get; set; }
    public SnakeSegment NextSegment { get; set; }
    public Snake ParentSnake { get { return parentSnake; } }
    public bool IsTail { get => NextSegment == null; }
    public bool IsHead { get => ParentSnake.Head == this; }

    private Snake parentSnake;

    public SnakeSegment Snapshot(Snake parenSnakeCopy, int i) {
        SnakeSegment nextSegmentCopy = !IsTail ? NextSegment.Snapshot(parenSnakeCopy, i+1) : null;
        SnakeSegment copy = Instantiate(this, parenSnakeCopy.transform);
        copy.gameObject.name = $"segment {i}";
        copy.CurrentDirection = CurrentDirection;
        copy.NextSegment = nextSegmentCopy;
        return copy;
    }

    private void Start() {
        parentSnake = transform.parent.gameObject.GetComponent<Snake>();
    }

    // public void DecorateWithPower(SpecialPower specialPower) {
    //     if(specialPower == null) return;

    //     transform.Find(specialPower.ToString()).gameObject.SetActive(true);
    // }

    public void Move(Vector3 direction) {
        transform.position += direction;
        transform.rotation = Quaternion.Euler(direction * 90);
        if(NextSegment != null) NextSegment.Move(CurrentDirection);
        CurrentDirection = direction;
    }

    private void OnTriggerEnter(Collider other) {
        ParentSnake.EvaluateCollision(this, other);
    }

    public float EvaluateMovementDelta(float movingDelta){
        movingDelta += movingDeltaIncrease;
        if(!IsTail) return NextSegment.EvaluateMovementDelta(movingDelta);

        return movingDelta;
    }
}
