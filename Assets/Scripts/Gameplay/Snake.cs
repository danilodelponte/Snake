using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeSegment segmentPrefab;
    [SerializeField] private float movingMaxDeltaTime;

    public Player Player { get; set; }
    public SnakeSegment Head { get; set; }
    public Vector3 Direction { get => intendedDirection; set => intendedDirection = value; }

    private Vector3 intendedDirection = Vector3.up;
    private float movementDeltaTimer = 0;

    public Snake Snapshot(){
        bool wasActive = gameObject.activeSelf;
        gameObject.SetActive(false);
        Snake copy = Instantiate(this, transform.parent);
        SnakeSegment[] children = copy.transform.GetComponentsInChildren<SnakeSegment>();
        foreach (var segment in children) { GameObject.Destroy(segment.gameObject); }
        copy.Player = Player;
        copy.Direction = Direction;
        copy.Head = Head.Snapshot(copy, 0);
        gameObject.SetActive(wasActive);
        return copy;
    }

    public void AddHead() {
        if(Head !=null) return;

        Head = Instantiate<SnakeSegment>(segmentPrefab, transform);
        Head.CurrentDirection = Direction;
    }

    public SnakeSegment AddSegment(SpecialPower specialPower = null) {
        Vector3 newHeadPosition = Head.transform.position + Direction;
        Quaternion newHeadRotation = Head.transform.rotation;
	    SnakeSegment newHead = Instantiate<SnakeSegment>(
            segmentPrefab, newHeadPosition, newHeadRotation, transform
        );
        newHead.SpecialPower = specialPower;
        newHead.NextSegment = Head;
        newHead.CurrentDirection = Head.CurrentDirection;
	    return Head = newHead;
    }
    
    private void FixedUpdate() {
        movementDeltaTimer += Time.deltaTime;

        float maxMovingDelta = Head.EvaluateMovementDelta(movingMaxDeltaTime);

        if(movementDeltaTimer >= maxMovingDelta) {
            movementDeltaTimer -= maxMovingDelta;
            Head.Move(Direction);
        }
    }

    public void SetDirection(Vector3 direction) {
        if((direction + Head.CurrentDirection) == Vector3.zero) return;
        Direction = direction;
    }
}
