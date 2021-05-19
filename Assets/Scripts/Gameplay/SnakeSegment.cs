using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    [SerializeField] private float movingDeltaIncrease = .01f;

    public Arena arena;
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

    private void OnEnable() {
        arena = GameObject.Find("Arena").GetComponent<Arena>();
        KeepInsideArena();
        SetNodePath();
    }

    private void OnDisable() {
        FreeNodePath();
    }

    private void OnDestroy() {
        FreeNodePath();
    }

    // public void DecorateWithPower(SpecialPower specialPower) {
    //     if(specialPower == null) return;

    //     transform.Find(specialPower.ToString()).gameObject.SetActive(true);
    // }

    public void Move(Vector3 direction) {
        transform.position += direction;
        KeepInsideArena();
        transform.rotation = Quaternion.Euler(direction * 90);
        if(NextSegment != null) NextSegment.Move(CurrentDirection);
        CurrentDirection = direction;
    }

    public void KeepInsideArena() {
        Vector3 position = transform.position;
        if(position.x >= arena.Width) position.x = 0;
        if(position.x < 0) position.x = arena.Width - 1;

        if(position.y >= arena.Height) position.y = 0;
        if(position.y < 0) position.y = arena.Height - 1;

        transform.position = position;
    }

    public void FreeNodePath() {
        arena.SetNode(transform.position, PathNodeType.FREE);
        if(!IsTail) NextSegment.FreeNodePath();
    }

    public void SetNodePath(){
        arena.SetNode(transform.position, PathNodeType.SNAKE);
        if(!IsTail) NextSegment.SetNodePath();
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
