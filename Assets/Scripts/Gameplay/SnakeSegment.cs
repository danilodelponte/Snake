using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    [SerializeField] private float movingDeltaIncrease = .01f;

    public Vector3 CurrentDirection { get; set; }
    public SnakeSegment NextSegment { get; set; }
    public SpecialPower SpecialPower { get; set; }
    public Snake ParentSnake { get { return parentSnake; } }

    private Snake parentSnake;

    private void Awake() {
        parentSnake = transform.parent.gameObject.GetComponent<Snake>();
    }

    private void Start() {
        if(SpecialPower != null) {
            Debug.Log(SpecialPower.ToString());
            transform.Find(SpecialPower.ToString()).gameObject.SetActive(true);
        }
    }

    public void Move(Vector3 direction) {
        transform.position += direction;
        if(NextSegment != null) NextSegment.Move(CurrentDirection);
        CurrentDirection = direction;
    }

    private void OnTriggerEnter(Collider other) {
        var gameplay = GameplayController.Singleton;
        gameplay.HandleCollision(this, other);
    }

    public float EvaluateMovementDelta(float movingDelta){
        movingDelta += movingDeltaIncrease;
        if(SpecialPower != null) movingDelta = SpecialPower.EvaluateMovementDelta(movingDelta);
        if(!IsTail()) return NextSegment.EvaluateMovementDelta(movingDelta);

        return movingDelta;
    }

    public bool IsTail() {
        return NextSegment == null;
    }

    public bool IsHead() {
        return ParentSnake.Head == this;
    }
}
