using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialModifier
{
    public SnakeSegment SnakeSegment { get; set; }
    protected GameplayController gameplayController;
    public GameObject Decoration { get => LoadDecoration(); }

    public virtual void Activate(GameplayController controller) {
        gameplayController = controller;
    }

    public virtual void Deactivate() {
        SnakeSegment.Modifier = null;
    }

    public virtual void ScoreGainModifier(ref int gain){}
    public virtual void DirectionModifier(ref Vector3 direction){}
    public virtual void MovementModifier(ref float maxDeltaTime){}
    public virtual bool CollisionModifier(SnakeSegment segmentCollided, Collider other) { return false; }
    public virtual bool DeathModifier(){ return false; }

    public virtual void FixedUpdate() {}

    private GameObject LoadDecoration() {
        return PrefabCache.Load<GameObject>($"SpecialModifiers/{ToString()}");
    }
}
