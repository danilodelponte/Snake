using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SnakeTemplates {

    public static SpecialModifier[][] GenerateTemplates() {
        SpecialModifier[][] templates = {
            new SpecialModifier[] { new EnginePower(), new EnginePower(), new EnginePower() },
            new SpecialModifier[] { new BatteringRam(), new BatteringRam(), new BatteringRam() },
            new SpecialModifier[] { new EnginePower(), null, new TimeTravel() },
            new SpecialModifier[] { new BatteringRam(), null, new TimeTravel() }
        };
        return templates;
    }
}
