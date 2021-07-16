using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTemplate {

    public SpecialModifier[] Modifiers { get => GetModifiers(); private set => _modifiers = value; }
    private SpecialModifier[] _modifiers;

    public static SnakeTemplate[] GetTemplates() {
        SnakeTemplate[] templates = {
            new SnakeTemplate(new EnginePower(), new EnginePower(), new EnginePower()),
            new SnakeTemplate(new BatteringRam(), new BatteringRam(), new BatteringRam()),
            new SnakeTemplate(new EnginePower(), new EnginePower(), new BatteringRam())
        };
        return templates;
    }

    // MOVER PARA SCRIPTABLE

    public SnakeTemplate() {
        Modifiers = new SpecialModifier[0];
    }

    public SnakeTemplate(SpecialModifier mod1, SpecialModifier mod2, SpecialModifier mod3) {
        Modifiers = new SpecialModifier[] { mod1, mod2, mod3 };
    }

    public void Apply(Snake snake, GameplayMode gameplayMode) {
        IEnumerator<SnakeSegment> segments = snake.Segments().GetEnumerator();
        for(int i = 0; i < Modifiers.Length; i++) {
            segments.MoveNext();
            Modifiers[i].Activate(segments.Current, gameplayMode);
        }
    }

    public SpecialModifier Modifier(int index) {
        return Modifiers[index];
    }

    private SpecialModifier[] GetModifiers() {
        if(_modifiers == null || _modifiers.Length != 3) _modifiers = new SpecialModifier[0];

        return _modifiers;
    }
    
}
