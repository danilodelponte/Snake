using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpecialModifierSnapshot
{
    private SpecialModifierSnapshot ModifierSnapshot;
    private Dictionary<Type, Type> map = new Dictionary<Type, Type>() {
        // { typeof(TimeTravel), typeof(TimeTravelSnapshot) }
    };

    public SpecialModifierSnapshot(SpecialModifier modifier) {
        
    }

    // private SpecialModifierSnapshot CreateSnapshot() {
    //     Activator.CreateInstance(type);
    // }

    // public SpecialModifier Load(SnakeSegment segment) {
    //     Type t = Modifier.GetType();
    //     SpecialModifier modifier = (SpecialModifier) segment.gameObject.AddComponent(t);
    //     if(t == typeof(TimeTravel)) TimeTravelLoad((TimeTravel) modifier);
    //     return modifier;
    // }

    // private class TimeTravelSnapshot {
    //     private Snapshot snapshot;

    //     public TimeTravelSnapshot(TimeTravel timeTravel) {
    //         snapshot = timeTravel.snapshot;
    //     }

    //     public TimeTravel Load(SnakeSegment segment) {
    //         TimeTravel modifier = segment.gameObject.AddComponent<TimeTravel>();
    //         modifier.snapshot = snapshot;
    //         return modifier;
    //     }
    // }

    // private class ConfusedSnapshot {
    //     private float timerSnapshot;

    //     public ConfusedSnapshot(Confused modifier) {
    //         timerSnapshot = modifier.Timer;
    //     }

    //     public Confused Load(SnakeSegment segment) {
    //         Confused modifier = segment.gameObject.AddComponent<Confused>();
    //         modifier.Timer = timerSnapshot;
    //         return modifier;
    //     }
    // }
}
