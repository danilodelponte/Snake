using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SnakeSegment : MonoBehaviour
{
    private SpecialComponent _specialComponent;
    private Snake snake;
    
    public Snake Snake { get => GetSnake(); }
    public SnakeSegment NextSegment { get; set; }
    public SpecialComponent SpecialComponent { get => GetSpecialComponent(); }
    public SpecialModifier Modifier { get => GetModifier(); }
    public BezierShape Body { get => GetComponentInChildren<BezierShape>(); }
    public Color Color { set => SetColor(value); }

    private void Start() {
        UpdateBody();
    }

    private Snake GetSnake() {
        if(snake == null) snake = transform.parent.gameObject.GetComponent<Snake>();

        return snake;
    }

    public SpecialModifier GetModifier() {
        return GetSpecialComponent().Modifier;
    }

    public SpecialComponent GetSpecialComponent(){
        if(_specialComponent == null) _specialComponent = gameObject.GetComponent<SpecialComponent>();

        return _specialComponent;
    }

    public void UpdateBody() {
        if(NextSegment == null) return;
        
        var points = new List<BezierPoint>();
        var thisPoint = new BezierPoint() { rotation = transform.localRotation };
        points.Add(thisPoint);

        var nextPoint = new BezierPoint() { rotation = transform.localRotation };
        nextPoint.SetPosition(new Vector3(0,-1,0));
        points.Add(nextPoint);

        Body.points = points;
        MeshRenderer rendered = Body.gameObject.GetComponent<MeshRenderer>();
        rendered.material.color = Snake.Color;
        Body.Refresh();
    }

    private void SetColor(Color color) {
        var renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = color;
        if(NextSegment != null) NextSegment.Color = color;
    }
}
