using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravel : SpecialModifier
{
    public Snapshot snapshot;

    public override void Activate() {
        DisablePrevious();
        if(snapshot == null) snapshot = GameplayController.Singleton.CreateSnapshot();
        Debug.Log($"{SnakeSegment.Snake.name} saved time!");
    }

    public override void Deactivate()
    {
        Debug.Log($"{SnakeSegment.Snake.name} deactivating time travel.");
        snapshot = null;
        base.Deactivate();
    }

    private void DisablePrevious(){
        List<SpecialModifier> specialModifiers = SnakeSegment.Snake.Modifiers();
        foreach (var specialModifier in specialModifiers) {
            if(specialModifier == this) continue;

            if(specialModifier is TimeTravel) {
                specialModifier.Deactivate();
            }
        }
    }

    public override bool DeathModifier() {
        if(snapshot == null) return false;

        Snapshot travelTo = this.snapshot;
        Debug.Log($"{SnakeSegment.Snake.name} is time travelling!");
        Deactivate();
        GameplayController.Singleton.LoadSnapshot(travelTo);
        return true;
    }
}
