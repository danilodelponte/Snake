using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeSegment segmentPrefab;
    [SerializeField] private float baseMovingDeltaTime;
    [SerializeField] private float maxMovingDeltaTime;
    [SerializeField] private float minMovingDeltaTime;

    public Player Player { get; set; }
    public bool IsPlayer { get => Player != null; }
    public bool isAI { get => GetComponent<SnakeControl>() != null; }
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

    public SnakeSegment AddSegment(SpecialModifier modifier = null) {
        Vector3 newHeadPosition = Head.transform.position + Head.CurrentDirection;
        Quaternion newHeadRotation = Head.transform.rotation;
	    SnakeSegment newHead = Instantiate<SnakeSegment>(
            segmentPrefab, newHeadPosition, newHeadRotation, transform
        );

        newHead.NextSegment = Head;
        newHead.CurrentDirection = Head.CurrentDirection;
        newHead.Color = Color;
        Head = newHead;

        if(modifier != null) {
            Head.Modifier = modifier;
            Head.Modifier.Activate();
        }

	    return Head;
    }

    private void SetColor(Color color) {
        this.color = color;
        Head.Color = color;
    }

    public List<SpecialModifier> Modifiers() {
        SpecialComponent[] components = GetComponentsInChildren<SpecialComponent>();
        List<SpecialModifier> modifiers = new List<SpecialModifier>();
        foreach (SpecialComponent component in components) {
            if(component.Modifier != null) {
                modifiers.Add(component.Modifier);
            }
        }
        return modifiers;
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

    public int EvaluateScoreGain(int gain) {
        return Head.EvaluateScoreGain(gain);
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
