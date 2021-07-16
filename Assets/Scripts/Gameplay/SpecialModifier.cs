using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialModifier
{
    private GameplayMode _gameplayMode;
    private SnakeSegment _segment;
    private GameObject _decoration;
    private bool _active = false;

    public GameplayMode GameplayMode { get => _gameplayMode; }
    public SnakeSegment Segment { get => _segment; }
    public GameObject Decoration { get => PrefabCache.Load<GameObject>($"SpecialModifiers/{ToString()}"); }
    public bool IsActive { get => _active; }
    
    public virtual void Activate(SnakeSegment segment, GameplayMode gameplayMode) {
        if(_active) return;
        _active = true;
        _gameplayMode = gameplayMode;
        _segment = segment;

        var specialComponent = _segment.gameObject.GetComponent<SpecialComponent>();
        if(specialComponent.Modifier != null) specialComponent.Modifier.Deactivate();
        specialComponent.Modifier = this;

        AddDecoration();
    }

    public virtual void Deactivate() {
        if(!_active) return;
        _active = false;

        RemoveDecoration();

        var specialComponent = _segment.gameObject.GetComponent<SpecialComponent>();
        specialComponent.Modifier = null;
        _segment = null;
    }

    private void AddDecoration() {
        if(_decoration == null) {
            _decoration = GameObject.Instantiate(Decoration, _segment.transform);
        }
    }

    private void RemoveDecoration() {
        if(_decoration != null) GameObject.Destroy(_decoration);
    }

    public virtual void FixedUpdate() {}
    public virtual void ScoreGainModifier(ref int gain){}
    public virtual void DirectionModifier(ref Vector3 direction){}
    public virtual void MovementModifier(ref float maxDeltaTime){}
    public virtual bool CollisionModifier(SnakeSegment segmentCollided, GameObject other) { return false; }
    public virtual bool DeathModifier(){ return false; }
}
