using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModifiersPlayground : GameplayMode
{
    private Player player;

    public SpecialModifier[] SpecialModifiers { get; set; }
    private int currentModifier = -1;

    public ModifiersPlayground(GameData gameData, GameplayController controller, SpecialModifier[] modifiers = null) : base(gameData, controller) {
        if(modifiers == null) {
            SpecialModifiers = new SpecialModifier[] { new EnginePower() };
        } else SpecialModifiers = modifiers;
    }

    public override void Start() {
        player = new Player(KeyCode.A, KeyCode.S);
        CreateArena();
        SpawnPlayerSnake(player);
        SpawnDummySnake(8, Vector3.up);
        SpawnCollectable(GenerateModifier());
        controller.GUI.AddPlayerLabel(player);
    }

    public override SpecialModifier GenerateModifier() {
        currentModifier ++;
        currentModifier %= SpecialModifiers.Length;
        Type modifierType = SpecialModifiers[currentModifier].GetType();
        return (SpecialModifier) Activator.CreateInstance(modifierType);  
    }
    
    public override void GameStateCheck() {
        List<Snake> snakes = SnakeRepository.FindAll(obj => obj.isActiveAndEnabled);
        int playerSnakesCount = 0; 
        int dummySnakesCount = 0;

        foreach (var snake in snakes) { 
            if(snake.GetComponent<PlayerControl>() != null) playerSnakesCount++;
            else dummySnakesCount++;
        }

        if(playerSnakesCount < 1) SpawnPlayerSnake(player);
        if(dummySnakesCount < 1) SpawnDummySnake(8, Vector3.up);
    }
}
