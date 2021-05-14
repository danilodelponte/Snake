using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public Player Player { get; set; }
    [SerializeField] private SnakeSegment segmentPrefab;
    private SnakeSegment head;
    public SnakeSegment Head { get { return head; } }
    private float movementTimer = 0;
    [SerializeField] private float movementTimerMax = .01f;

    [SerializeField] private float movementTimerAddIncrease = .01f;

    private Vector3 intendedDirection = Vector3.up; // starting direction
    public Vector3 Direction { get { return intendedDirection;} }

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
