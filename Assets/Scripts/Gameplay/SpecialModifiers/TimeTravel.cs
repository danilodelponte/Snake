using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravel : SpecialModifier
{
    public Snapshot snapshot;

    public override void Activate(SnakeSegment segment, GameplayMode gameplayMode) {
        base.Activate(segment, gameplayMode);
        DisablePrevious();
        if(snapshot == null) snapshot = gameplayMode.CreateSnapshot();
        Debug.Log("saved time!");
    }

    public override void Deactivate()
    {
        Debug.Log("deactivating time travel.");
        snapshot = null;
        base.Deactivate();
    }

    private void DisablePrevious(){
        Snake snake = Segment.Snake;
        foreach (SnakeSegment segment in snake.Segments()) {
            SpecialModifier modifier = segment.GetComponent<SpecialComponent>().Modifier;
            if(modifier == this) continue;
            else if(modifier is TimeTravel) modifier.Deactivate();
        }
    }

    public override bool DeathModifier() {
        if(snapshot == null) return false;

        Snapshot travelTo = this.snapshot;
        Debug.Log($"time travelling!");
        GameplayMode.LoadSnapshot(travelTo);
        Deactivate();
        return true;
    }
}
