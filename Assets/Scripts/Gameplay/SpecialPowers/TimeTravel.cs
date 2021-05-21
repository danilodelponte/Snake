using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravel : SpecialPower
{
    public Snapshot snapshot;

    public override void Activate() {
        DisablePrevious();
        Debug.Log($"{SnakeSegment.Snake} saved time!");
        if(snapshot == null) snapshot = GameplayController.Singleton.CreateSnapshot();
    }

    public override void Deactivate()
    {
        snapshot = null;
        base.Deactivate();
    }

    private void DisablePrevious(){
        List<SpecialPower> specialPowers = SnakeSegment.Snake.SpecialPowers();
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
        Debug.Log($"{SnakeSegment.Snake} is time travelling!");
        Deactivate();
        GameplayController.Singleton.LoadSnapshot(travelTo);
        GameplayController.Singleton.SpawnCollectable();
        return true;
    }
}
