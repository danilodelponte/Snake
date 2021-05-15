using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeSegment segmentPrefab;
    [SerializeField] private float movementTimerMax = .01f;
    [SerializeField] private float movementTimerAddIncrease = .01f;

    public Player Player { get; set; }
    public SnakeSegment Head { get { return head; } }
    public Vector3 Direction { get { return intendedDirection;} }

    private SnakeSegment head;
    private float movementTimer = 0;
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

    public void AddSegment() {
        Vector3 newHeadPosition = head.transform.position + intendedDirection;
        Quaternion newHeadRotation = head.transform.rotation;
	    SnakeSegment newHead = Instantiate<SnakeSegment>(
            segmentPrefab, newHeadPosition, newHeadRotation, transform
        );

        newHead.NextSegment = head;
        newHead.CurrentDirection = head.CurrentDirection;
	    head = newHead;
        movementTimerMax += movementTimerAddIncrease;
    }
    
    private void FixedUpdate() {
        movementTimer += Time.deltaTime;
        if(movementTimer >= movementTimerMax) {
            movementTimer -= movementTimerMax;
            head.Move(intendedDirection);
        }
    }

    public void SetDirection(Vector3 direction) {
        if((direction + head.CurrentDirection) == Vector3.zero) return;
        intendedDirection = direction;
    }
}
