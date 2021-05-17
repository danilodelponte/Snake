using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravel : SpecialPower
{
    private Snapshot snapshot;

    public override void Activate(){ 
        this.snapshot = Snapshot.Create();
    }

    public override bool HandleCollision(SnakeSegment segment, Collider other) {
        if(other.gameObject.GetComponent<SnakeSegment>() == null) return false;

        Snapshot.Load(snapshot);
        GameplayController.Singleton.SpawnCollectable();
        return true;
    }
}
