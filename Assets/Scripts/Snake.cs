using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeSegment segmentPrefab;
    private SnakeSegment head;
    [SerializeField] private float speed = 1;
    private float movementTimer = 0;
    [SerializeField] private float movementTimerMax = .5f;

    [SerializeField] private float movementTimerAddIncrease = .1f;

    private Vector3 intendedDirection = Vector3.up; // starting direction

    private void Start() {
        InitializeSegments();
    }

    private void InitializeSegments() {
        head = Instantiate<SnakeSegment>(segmentPrefab);
        head.CurrentDirection = intendedDirection;
        AddSegment();
        AddSegment();
    }
    public void AddSegment() {
        Vector3 newHeadPosition = head.transform.position + intendedDirection;
        Quaternion newHeadRotation = head.transform.rotation;
	    SnakeSegment newHead = Instantiate<SnakeSegment>(segmentPrefab, newHeadPosition, newHeadRotation, transform);

        newHead.NextSegment = head;
        newHead.CurrentDirection = head.CurrentDirection;
	    head = newHead;
        movementTimerMax += movementTimerAddIncrease;
    }
    
    private void FixedUpdate() {
        //when speed 

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
