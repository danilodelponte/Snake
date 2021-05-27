using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMode : GameplayMode
{
    private GameplayController gameController;
    private Player[] Players { get => GameManager.Instance.Players; }

    public override void Start(GameplayController gameController, GUIController gui) {
        this.gameController = gameController;
        gameController.CreateArena();

        if(Players == null || Players.Length < 1) return;

        foreach (Player player in Players) {
            gui.AddPlayerLabel(player);
            gameController.SpawnPlayerSnake(player);
            gameController.SpawnEnemySnake();
            gameController.SpawnCollectable();
        }
    }

    public override SpecialModifier GenerateModifier() {
        int chance = UnityEngine.Random.Range(0,100);
        SpecialModifier modifier = null;
        
        if(chance < 10) modifier = new HeadBomb();
        else if(chance < 30) modifier = new EnginePower();
        else if(chance < 40) modifier = new BatteringRam();
        else if(chance < 50) modifier = new Confused();
        else if(chance < 60) modifier = new TimeTravel();
        else if(chance < 70) modifier = new Trap();
        else if(chance < 80) modifier = new DoubleScore();
        return modifier;
    }

    public override void GameStateCheck() {
        GameObject[] snakes = GameObject.FindGameObjectsWithTag("Snake");
        List<GameObject> playerSnakes = new List<GameObject>();
        List<GameObject> aiSnakes = new List<GameObject>();

        foreach (var snake in snakes) { 
            if(snake.GetComponent<PlayerControl>() != null) playerSnakes.Add(snake);
            else if(snake.GetComponent<AIControl>() != null) aiSnakes.Add(snake);
        }

        int aiSnakeDiff = Players.Length - aiSnakes.Count;
        for(int i = 0; i < aiSnakeDiff; i++) gameController.SpawnEnemySnake();

        if(playerSnakes.Count < 1) gameController.GameOver();
    }
}
