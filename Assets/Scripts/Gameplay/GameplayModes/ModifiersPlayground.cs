using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModifiersPlayground : GameplayMode
{
    private Player player;
    private GameplayController gameController;

    public SpecialModifier[] SpecialModifiers { get; set; }
    private int currentModifier = -1;

    public ModifiersPlayground(SpecialModifier[] modifiers = null) {
        if(modifiers == null) {
            SpecialModifiers = new SpecialModifier[] { new EnginePower() };
        } else SpecialModifiers = modifiers;
    }

    public override void Start(GameplayController gameController, GUIController gui) {
        player = new Player(KeyCode.A, KeyCode.S);
        this.gameController = gameController;
        gameController.CreateArena();
        gameController.SpawnPlayerSnake(player);
        gameController.SpawnDummySnake();
        gameController.SpawnCollectable(GenerateModifier());
        gui.AddPlayerLabel(player);
    }

    public override SpecialModifier GenerateModifier() {
        currentModifier ++;
        currentModifier %= SpecialModifiers.Length;
        Type modifierType = SpecialModifiers[currentModifier].GetType();
        return (SpecialModifier) Activator.CreateInstance(modifierType);  
    }
    
    public override void GameStateCheck() {
        GameObject[] snakes = GameObject.FindGameObjectsWithTag("Snake");
        List<GameObject> playerSnakes = new List<GameObject>();
        List<GameObject> dummySnakes = new List<GameObject>();

        foreach (var snake in snakes) { 
            if(snake.GetComponent<PlayerControl>() != null) playerSnakes.Add(snake);
            else dummySnakes.Add(snake);
        }

        if(playerSnakes.Count < 1) gameController.SpawnPlayerSnake(player);
        if(dummySnakes.Count < 1) gameController.SpawnDummySnake();
    }
}
