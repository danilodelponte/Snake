using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravel : SpecialPower
{
    private Snapshot snapshot;
    private bool travelled = false;

    public override void Start() {
        base.Start();
        DisablePrevious();
        this.snapshot = Snapshot.Create();
    }

    private void DisablePrevious(){
        SpecialPower[] powers = Segment.ParentSnake.SpecialPowers.ToArray();
        foreach (var power in powers) {
            if(power == this) continue;

            if(power is TimeTravel) {
                Snapshot.Destroy(((TimeTravel) power).snapshot);
                Segment.ParentSnake.RemovePower(power);
            }
        }
    }

    public override bool SpecialCollision(SnakeSegment segment, Collider other) {
        if(other.gameObject.GetComponent<SnakeSegment>() == null) return false;
        Segment.ParentSnake.RemovePower(this);

        if(!travelled) {
            Snapshot.Load(snapshot);
            travelled = true;
        }
        return true;
    }
}
