using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMode : GameplayMode
{
    private Player[] Players { get => GameManager.Instance.Players; }

    public MultiplayerMode(GameData gameData, GameplayController controller) : base(gameData, controller) {}

    public override void Start() {
        CreateArena();

        if(Players == null || Players.Length < 1) return;

        foreach (Player player in Players) {
            controller.GUI.AddPlayerLabel(player);
            SpawnPlayerSnake(player);
            SpawnEnemySnake();
            SpawnCollectable();
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
        List<Snake> snakes = SnakeRepository.FindAll(obj => obj.isActiveAndEnabled);
        int playerSnakesCount = 0;
        int aiSnakesCount = 0;

        foreach (var snake in snakes) { 
            if(snake.GetComponent<PlayerControl>() != null) playerSnakesCount++;
            else if(snake.GetComponent<AIControl>() != null) aiSnakesCount++;
        }

        int aiSnakeDiff = Players.Length - aiSnakesCount;
        for(int i = 0; i < aiSnakeDiff; i++) SpawnEnemySnake();

        if(playerSnakesCount < 1) controller.GameOver();
    }
}
