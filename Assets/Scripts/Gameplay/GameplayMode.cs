using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMode
{
    public virtual void Start(GameplayController gameController, GUIController gui) {}
    public virtual SpecialModifier GenerateModifier() { return null; }
    public virtual void GameStateCheck() {}
}
