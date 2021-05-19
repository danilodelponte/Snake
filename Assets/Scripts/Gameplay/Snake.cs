using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeSegment segmentPrefab;
    [SerializeField] private float movingDeltaTime;

    public Player Player { get; set; }
    public SnakeSegment Head { get; set; }
    public Vector3 Direction { get => intendedDirection; set => intendedDirection = value; }

    private Vector3 intendedDirection = Vector3.up;
    private float movementDeltaTimer = 0;
    private List<SpecialPower> specialPowers = new List<SpecialPower>();
    public List<SpecialPower> SpecialPowers { get => specialPowers; }

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

    public SnakeSegment AddSegment(Type powerType = null) {
        Vector3 newHeadPosition = Head.transform.position + Direction;
        Quaternion newHeadRotation = Head.transform.rotation;
	    SnakeSegment newHead = Instantiate<SnakeSegment>(
            segmentPrefab, newHeadPosition, newHeadRotation, transform
        );
        newHead.NextSegment = Head;
        newHead.CurrentDirection = Head.CurrentDirection;
        if(powerType != null) AddPower(powerType, newHead);

	    return Head = newHead;
    }

    private void AddPower(Type powerType, SnakeSegment segment) {
        SpecialPower specialPower = (SpecialPower) segment.gameObject.AddComponent(powerType);
        specialPowers.Add(specialPower);
    }

    public void RemovePower(SpecialPower specialPower) {
        specialPowers.Remove(specialPower);
        Destroy(specialPower);
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

    public float EvaluateMovementDelta(){
        float movingDelta = Head.EvaluateMovementDelta(movingDeltaTime);
        foreach (var power in SpecialPowers) {
            movingDelta = power.SpecialMovement(movingDelta);
        }
        if(movingDelta > .8f) movingDelta = .8f;
        return movingDelta;
    }

    public void EvaluateCollision(SnakeSegment segment, Collider other) {
        bool specialCollision = false;
        SpecialPower[] powers = specialPowers.ToArray();
        foreach (var power in powers) {
            specialCollision |= power.SpecialCollision(segment, other);
        }
        if(specialCollision) return;
        else GameplayController.Singleton.HandleCollision(segment, other);
    }
}
