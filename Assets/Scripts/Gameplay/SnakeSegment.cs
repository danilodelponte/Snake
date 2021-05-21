using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    [SerializeField] private float movingDeltaIncrease = .01f;

    private SpecialComponent SpecialComponent { get => gameObject.GetComponent<SpecialComponent>(); }

    public Color Color { set => SetColor(value); }
    public Arena arena;
    public Vector3 CurrentDirection { get; set; }
    public SnakeSegment NextSegment { get; set; }
    public Snake Snake { get => transform.parent.gameObject.GetComponent<Snake>(); }
    public bool IsTail { get => NextSegment == null; }
    public bool IsHead { get => Snake.Head == this; }
    public SpecialPower SpecialPower { get => GetSpecialPower(); set => SetSpecialPower(value); }

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

    private void SetSpecialPower(SpecialPower specialPower) {
        if(specialPower != null) {
            specialPower.SnakeSegment = this;
            Transform powerObject = transform.Find(specialPower.ToString());
            if(powerObject != null) { gameObject.SetActive(true); }
        }
        SpecialComponent.SpecialPower = specialPower;
    }

    private SpecialPower GetSpecialPower() {
        return SpecialComponent.SpecialPower;
    }

    public void Move(Vector3 direction) {
        if(direction == Vector3.zero) return;

        if((direction + CurrentDirection) == Vector3.zero) direction = CurrentDirection;
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
        Snake.EvaluateCollision(this, other);
    }

    public bool EvaluateCollision(SnakeSegment segmentCollided, Collider other) {
        if(SpecialPower != null && SpecialPower.SpecialCollision(segmentCollided, other)) return true;
        if(!IsTail) return NextSegment.EvaluateCollision(segmentCollided, other);

        return false;
    }

    public Vector3 EvaluateDirection(Vector3 direction) {
        if(SpecialPower != null) direction = SpecialPower.SpecialDirection(direction);
        if(!IsTail) return NextSegment.EvaluateDirection(direction);

        return direction;
    }

    public float EvaluateMovementDelta(float movingDelta){
        movingDelta += movingDeltaIncrease;
        if(SpecialPower != null) movingDelta = SpecialPower.SpecialMovement(movingDelta);
        if(!IsTail) return NextSegment.EvaluateMovementDelta(movingDelta);

        return movingDelta;
    }

    private void SetColor(Color color) {
        var renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = color;
        if(!IsTail) NextSegment.Color = color;
    }

    public override string ToString() {
        return $"SnakeSegment {Snake.name}";
    }
}
