using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Snake : MonoBehaviour
{
    public static Snake Prefab { get => PrefabCache.Load<Snake>("Snake"); }

    [SerializeField] public float baseMovingDeltaTime;
    [SerializeField] public float maxMovingDeltaTime;
    [SerializeField] public float minMovingDeltaTime;

    public Player Player { get; set; }
    public bool IsPlayer { get => Player != null; }
    public bool isAI { get => GetComponent<AIControl>() != null; }
    public Color Color { get => color; set => SetColor(value); }
    public SnakeSegment Head { get; set; }

    private Color color;
    private float movementDeltaTimer = 0;
    private Vector3 intendedDirection = Vector3.up;

    public void Init(SnakeTemplate template = null) {
        if(Head !=null) return;
        if(template == null) template = new SnakeTemplate();

        Head = Instantiate<SnakeSegment>(SnakeSegment.Prefab, transform);
        Head.CurrentDirection = Vector3.up;
        Head.Color = color;
        Head.Modifier = template.Modifier(0);
        if(Head.Modifier!=null) {
            Head.Modifier.Activate();
        }
        AddSegment(template.Modifier(1));
        AddSegment(template.Modifier(2));
    }

    public SnakeSegment AddSegment(SpecialModifier modifier = null) {
        Vector3 newHeadPosition = Head.transform.position + Head.CurrentDirection;
        Quaternion newHeadRotation = Head.transform.rotation;
	    SnakeSegment newHead = Instantiate<SnakeSegment>(
            SnakeSegment.Prefab, newHeadPosition, newHeadRotation, transform
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

    public void UpdateMovement() {
        movementDeltaTimer += Time.deltaTime;
        float movingDelta = EvaluateMovementDelta();

        if(movementDeltaTimer >= movingDelta) {
            movementDeltaTimer -= movingDelta;
            Move(intendedDirection);
        }
    }

    public void UpdateDirection() {
        intendedDirection = EvaluateDirection();
    }

    public void Move(Vector3 direction) {
        Head.Move(direction);
    }

    public Vector3 ControlDirection() {
        SnakeControl control = GetComponent<SnakeControl>();
        if(control == null) return Head.CurrentDirection;

        return control.GetDirection();
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
