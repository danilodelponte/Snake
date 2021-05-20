using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravel : SpecialPower
{
    private Snapshot snapshot;

    public override void Activate() {
        DisablePrevious();
        Debug.Log($"TimeTravel Activate on {SnakeSegment}");
        if(snapshot == null) snapshot = GameplayController.Singleton.CreateSnapshot();
    }

    public override void Deactivate()
    {
        Debug.Log($"TimeTravel Deactivate on {SnakeSegment}");
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
        if(snapshot == null) return false;
        if(!segmentCollided.IsHead) return false;
        if(other.gameObject.GetComponent<SnakeSegment>() == null) return false;

        Snapshot travelTo = this.snapshot;
        Deactivate();
        Debug.Log($"TimeTravel SpecialCollision on {SnakeSegment}");
        GameplayController.Singleton.LoadSnapshot(travelTo);
        GameplayController.Singleton.SpawnCollectable();
        return true;
    }
}
