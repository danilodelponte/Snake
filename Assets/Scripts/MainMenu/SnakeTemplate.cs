using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTemplate {

    public SpecialModifier[] Modifiers { get => GetModifiers(); private set => modifiers = value; }
    private SpecialModifier[] modifiers;

    public static SnakeTemplate[] GetTemplates() {
        SnakeTemplate[] templates = {
            new SnakeTemplate(new EnginePower(), new EnginePower(), new EnginePower()),
            new SnakeTemplate(new BatteringRam(), new BatteringRam(), new BatteringRam()),
            new SnakeTemplate(new EnginePower(), null, new TimeTravel()),
            new SnakeTemplate(new BatteringRam(), null, new TimeTravel())
        };
        return templates;
    }

    public SnakeTemplate(SpecialModifier mod1, SpecialModifier mod2, SpecialModifier mod3) {
        Modifiers = new SpecialModifier[] { mod1, mod2, mod3 };
    }

    private SpecialModifier[] GetModifiers() {
        if(modifiers == null || modifiers.Length != 3) modifiers = new SpecialModifier[3];

        return modifiers;
    }
    
}
