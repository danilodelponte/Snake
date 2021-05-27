using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOnlyMode : GameplayMode
{
    public int numberOfSnakes { get; set; }

    public AIOnlyMode(int numberOfSnakes = 5) {
        this.numberOfSnakes = numberOfSnakes;
    }

    public override void Start(GameplayController gameController, GUIController gui) {
        gameController.CreateArena();
        for(int i = 0; i < numberOfSnakes; i++) {
            gameController.SpawnEnemySnake();
            gameController.SpawnCollectable();
        }
    }
}
