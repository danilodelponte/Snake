using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialModifier
{
    private SnakeSegment attachedSegment;
    public SnakeSegment SnakeSegment { get => attachedSegment; set => SetSnakeSegment(value); }
    public Snake Snake { get => SnakeSegment.Snake; }
    protected GameplayController gameplayController;
    public GameObject Decoration { get => LoadDecoration(); }

    public virtual void Activate(GameplayController controller) {
        gameplayController = controller;
    }

    public virtual void Deactivate() {}
    public virtual void ScoreGainModifier(ref int gain){}
    public virtual void DirectionModifier(ref Vector3 direction){}
    public virtual void MovementModifier(ref float maxDeltaTime){}
    public virtual bool CollisionModifier(SnakeSegment segmentCollided, Collider other) { return false; }
    public virtual bool DeathModifier(){ return false; }

    public virtual void FixedUpdate() {}

    private GameObject LoadDecoration() {
        return PrefabCache.Load<GameObject>($"SpecialModifiers/{ToString()}");
    }

    private void SetSnakeSegment(SnakeSegment segment) {
        SnakeSegment exSegment = attachedSegment;
        attachedSegment = segment;

        if(exSegment != null) exSegment.Modifier = null;
    }
}
