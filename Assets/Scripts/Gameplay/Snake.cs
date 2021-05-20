using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeSegment segmentPrefab;
    [SerializeField] private float baseMovingDeltaTime;
    [SerializeField] private float maxMovingDeltaTime;
    [SerializeField] private float minMovingDeltaTime;

    public Player Player { get; set; }
    public Color Color { get => color; set => SetColor(value); }
    public SnakeSegment Head { get; set; }
    public Vector3 Direction { get => intendedDirection; set => intendedDirection = value; }

    private Vector3 intendedDirection = Vector3.up;
    private Color color;
    private float movementDeltaTimer = 0;

    public void AddHead() {
        if(Head !=null) return;

        Head = Instantiate<SnakeSegment>(segmentPrefab, transform);
        Head.CurrentDirection = Direction;
        Head.Color = color;
    }

    public SnakeSegment AddSegment(SpecialPower specialPower = null) {
        Vector3 newHeadPosition = Head.transform.position + Direction;
        Quaternion newHeadRotation = Head.transform.rotation;
	    SnakeSegment newHead = Instantiate<SnakeSegment>(
            segmentPrefab, newHeadPosition, newHeadRotation, transform
        );

        newHead.NextSegment = Head;
        newHead.CurrentDirection = Head.CurrentDirection;
        newHead.Color = Color;
        Head = newHead;

	    return Head;
    }

    private void SetColor(Color color) {
        this.color = color;
        Head.Color = color;
    }

    public List<SpecialPower> SpecialPowers() {
        SpecialComponent[] components = GetComponentsInChildren<SpecialComponent>();
        List<SpecialPower> specialPowers = new List<SpecialPower>();
        foreach (SpecialComponent component in components) {
            if(component.SpecialPower != null) {
                specialPowers.Add(component.SpecialPower);
            }
        }
        return specialPowers;
    }

    private void FixedUpdate() {
        movementDeltaTimer += Time.deltaTime;
        float movingDelta = EvaluateMovementDelta();

        if(movementDeltaTimer >= movingDelta) {
            movementDeltaTimer -= movingDelta;
            Move();
        }
    }

    public void SetDirection(Vector3 direction) {
        if((direction + Head.CurrentDirection) == Vector3.zero) return;
        Direction = direction;
    }

    public void Move() {
        AIControl aiControl = GetComponent<AIControl>();
        if(aiControl != null) Direction = aiControl.GetDirection();
        FreeNodePaths();
        Head.Move(Direction);
        SetNodePaths();
    }

    public void SetNodePaths(){
        Head.SetNodePath();
    }

    public void FreeNodePaths(){
        Head.FreeNodePath();
    }

    public void EvaluateCollision(SnakeSegment segmentCollided, Collider other){
        if(Head.EvaluateCollision(segmentCollided, other)) return;
        GameplayController.Singleton.HandleCollision(segmentCollided, other);
    }

    public float EvaluateMovementDelta(){
        float movingDelta = Head.EvaluateMovementDelta(baseMovingDeltaTime);
        movingDelta = Mathf.Clamp(movingDelta, minMovingDeltaTime, maxMovingDeltaTime);
        return movingDelta;
    }

    public override string ToString() {
        return $"Snake {name}";
    }
}
