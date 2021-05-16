using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeSegment segmentPrefab;
    [SerializeField] private float movingMaxDeltaTime;

    public Player Player { get; set; }
    public SnakeSegment Head { get { return head; } }
    public Vector3 Direction { get { return intendedDirection;} }

    private SnakeSegment head;
    private float movementDeltaTimer = 0;
    private Vector3 intendedDirection = Vector3.up;

    private void Awake() {
        AddHead();
        AddSegment();
        AddSegment();
    }

    public void AddHead() {
        if(head !=null) return;

        head = Instantiate<SnakeSegment>(segmentPrefab, transform);
        head.CurrentDirection = intendedDirection;
    }

    public SnakeSegment AddSegment(SpecialPower specialPower = null) {
        Vector3 newHeadPosition = head.transform.position + intendedDirection;
        Quaternion newHeadRotation = head.transform.rotation;
	    SnakeSegment newHead = Instantiate<SnakeSegment>(
            segmentPrefab, newHeadPosition, newHeadRotation, transform
        );
        newHead.SpecialPower = specialPower;
        newHead.NextSegment = head;
        newHead.CurrentDirection = head.CurrentDirection;
	    return head = newHead;
    }
    
    private void FixedUpdate() {
        movementDeltaTimer += Time.deltaTime;

        float maxMovingDelta = head.EvaluateMovementDelta(movingMaxDeltaTime);

        if(movementDeltaTimer >= maxMovingDelta) {
            movementDeltaTimer -= maxMovingDelta;
            head.Move(intendedDirection);
        }
    }

    public void SetDirection(Vector3 direction) {
        if((direction + head.CurrentDirection) == Vector3.zero) return;
        intendedDirection = direction;
    }
}
