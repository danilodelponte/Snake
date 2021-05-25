using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SnakeSegment : MonoBehaviour
{
    [SerializeField] private float movingDeltaIncrease = .01f;

    private SpecialComponent SpecialComponent { get => gameObject.GetComponent<SpecialComponent>(); }

    public Color Color { set => SetColor(value); }
    public Vector3 CurrentDirection { get; set; }
    public SnakeSegment NextSegment { get; set; }
    public Snake Snake { get => transform.parent.gameObject.GetComponent<Snake>(); }
    public BezierShape Body { get => GetComponentInChildren<BezierShape>(); }
    public bool IsTail { get => NextSegment == null; }
    public bool IsHead { get => Snake.Head == this; }
    public SpecialModifier Modifier { get => GetModifier(); set => SetModifier(value); }

    private void Start() {
        UpdateBody();
    }

    private void SetModifier(SpecialModifier modifier) {
        if(modifier != null) {
            modifier.SnakeSegment = this;
            Transform modifierDecoration = transform.Find(modifier.ToString());
            if(modifierDecoration != null) { gameObject.SetActive(true); }
        }
        SpecialComponent.Modifier = modifier;
    }

    private SpecialModifier GetModifier() {
        return SpecialComponent.Modifier;
    }

    public void Move(Vector3 direction) {
        if(direction == Vector3.zero) return;

        if((direction + CurrentDirection) == Vector3.zero) direction = CurrentDirection;
        transform.position += direction;
        transform.rotation = GetRotation(direction);
        if(NextSegment != null) {
            NextSegment.Move(CurrentDirection);
        }
        CurrentDirection = direction;
    }

    private Quaternion GetRotation(Vector3 direction) {
        Quaternion rotation = Quaternion.identity;
        if(direction.x == -1) rotation = Quaternion.Euler(0,0,90);
        else if(direction.y == -1) rotation = Quaternion.Euler(0,0,180);
        else if(direction.x == 1) rotation = Quaternion.Euler(0,0,270);
        return rotation;
    }

    public void UpdateBody() {
        if(IsTail) return;
        
        List<BezierPoint> points = new List<BezierPoint>();
        BezierPoint thisPoint = new BezierPoint();
        thisPoint.rotation = transform.localRotation;
        points.Add(thisPoint);

        BezierPoint nextPoint = new BezierPoint();
        nextPoint.SetPosition(new Vector3(0,-1,0));
        nextPoint.rotation = transform.localRotation;
        points.Add(nextPoint);

        Body.points = points;
        MeshRenderer rendered = Body.gameObject.GetComponent<MeshRenderer>();
        rendered.material.color = Snake.Color;
        Body.Refresh();
    }

    public int EvaluateScoreGain(int gain) {
        if(Modifier != null) gain = Modifier.ScoreGainModifier(gain);
        if(!IsTail) return NextSegment.EvaluateScoreGain(gain);

        return gain;
    }

    public bool EvaluateDeath() {
        if(Modifier != null && Modifier.DeathModifier()) return true;
        if(!IsTail) return NextSegment.EvaluateDeath();

        return false;
    }

    private void OnTriggerEnter(Collider other) {
        Snake.EvaluateCollision(this, other);
    }

    public bool EvaluateCollision(SnakeSegment segmentCollided, Collider other) {
        if(Modifier != null && Modifier.CollisionModifier(segmentCollided, other)) return true;
        if(!IsTail) return NextSegment.EvaluateCollision(segmentCollided, other);

        return false;
    }

    public Vector3 EvaluateDirection(Vector3 direction) {
        if(Modifier != null) direction = Modifier.DirectionModifier(direction);
        if(!IsTail) return NextSegment.EvaluateDirection(direction);

        return direction;
    }

    public float EvaluateMovementDelta(float movingDelta){
        movingDelta += movingDeltaIncrease;
        if(Modifier != null) movingDelta = Modifier.MovementModifier(movingDelta);
        if(!IsTail) return NextSegment.EvaluateMovementDelta(movingDelta);

        return movingDelta;
    }

    private void SetColor(Color color) {
        var renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = color;
        if(!IsTail) NextSegment.Color = color;
    }
}
