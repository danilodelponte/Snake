using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravel : SpecialPower
{
    private Snapshot snapshot;

    public override void Activate() {
        DisablePrevious();
        this.snapshot = GameplayController.Singleton.CreateSnapshot();
    }

    public override void Deactivate()
    {
        snapshot = null;
        base.Deactivate();
    }

    private void DisablePrevious(){
        List<SpecialPower> specialPowers = SnakeSegment.ParentSnake.SpecialPowers();
        foreach (var specialPower in specialPowers) {
            if(specialPower == this) continue;

            if(specialPower is TimeTravel) {
                specialPower.Deactivate();
            }
        }
    }

    public override bool SpecialCollision(SnakeSegment segmentCollided, Collider other) {
        if(!segmentCollided.IsHead) return false;
        if(other.gameObject.GetComponent<SnakeSegment>() == null) return false;

        GameplayController.Singleton.LoadSnapshot(this.snapshot);
        GameplayController.Singleton.SpawnCollectable();
        Deactivate();
        return true;
    }
}
