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

    private Color color;
    private float movementDeltaTimer = 0;

    public void AddHead() {
        if(Head !=null) return;

        Head = Instantiate<SnakeSegment>(segmentPrefab, transform);
        Head.CurrentDirection = Vector3.up;
        Head.Color = color;
    }

    public SnakeSegment AddSegment(SpecialPower specialPower = null) {
        Vector3 newHeadPosition = Head.transform.position + Head.CurrentDirection;
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
            Vector3 direction = EvaluateDirection();
            Move(direction);
        }
    }

    public void Move(Vector3 direction) {
        FreeNodePaths();
        Head.Move(direction);
        SetNodePaths();
    }

    public Vector3 ControlDirection() {
        SnakeControl control = GetComponent<SnakeControl>();
        if(control == null) return Head.CurrentDirection;

        return control.GetDirection();
    }

    public void SetNodePaths(){
        Head.SetNodePath();
    }

    public void FreeNodePaths(){
        Head.FreeNodePath();
    }

    public void Die() {
        if(EvaluateDeath()) return; 

        Debug.Log($"{this} has died!");
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }

    public bool EvaluateDeath() {
        if(Head.EvaluateDeath()) return true;

        return false;
    }
    
    public Vector3 EvaluateDirection() {
        Vector3 direction = ControlDirection();
        direction = Head.EvaluateDirection(direction);
        return direction;
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
