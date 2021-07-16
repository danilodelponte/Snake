using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOnlyMode : GameplayMode
{
    public int numberOfSnakes { get; set; }

    public AIOnlyMode(GameData gameData, GameplayController controller, int numberOfSnakes = 1) : base(gameData, controller) {
        this.numberOfSnakes = numberOfSnakes;
    }

    public override void Start() {
        CreateArena();
        for(int i = 0; i < numberOfSnakes; i++) {
            SpawnEnemySnake();
            SpawnCollectable();
        }
    }
}
