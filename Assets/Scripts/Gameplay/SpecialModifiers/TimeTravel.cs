﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravel : SpecialModifier
{
    public Snapshot snapshot;

    public override void Activate(GameplayController controller) {
        base.Activate(controller);
        DisablePrevious();
        if(snapshot == null) snapshot = gameplayController.CreateSnapshot();
        Debug.Log($"{SnakeSegment.Snake.name} saved time!");
    }

    public override void Deactivate()
    {
        Debug.Log($"{SnakeSegment.Snake.name} deactivating time travel.");
        snapshot = null;
        SnakeSegment = null;
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
        gameplayController.LoadSnapshot(travelTo);
        Deactivate();
        return true;
    }
}
