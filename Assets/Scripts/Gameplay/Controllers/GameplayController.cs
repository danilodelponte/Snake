using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Singleton;

    [SerializeField] private GUIController gUI;
    [SerializeField] private Arena arena;

    void Awake()
    {
        InitSingleton();

        arena.GenerateGrid();
        arena.GridDebug();

        for(int i = 0; i < 4; i++) {
            Snake enemySnake = arena.SpawnSnake();
            enemySnake.name = $"snake {i}";
            AIControl aiControl = enemySnake.gameObject.AddComponent<AIControl>();
            arena.SpawnCollectable();
        }

        // Player[] players = GameManager.Instance.Players;
        // if(players == null) {
        //     Player player = new Player("dan", KeyCode.A, KeyCode.S);
        //     players = new Player[1];
        //     players[0] = player;
        // }
        // foreach (Player player in players) {
        //     InitPlayer(player);
        // }
    }

    private void InitSingleton() {
        if (Singleton != null) {
            Destroy(gameObject);
            return;
        }
        Singleton = this;
    }

    private void InitPlayer(Player player) {
        gUI.AddPlayerLabel(player);

        var snake = arena.SpawnSnake();
        snake.Player = player;
        PlayerControl playerControl = snake.gameObject.AddComponent<PlayerControl>();
        playerControl.SetKeys(snake.Player.LeftKey, snake.Player.RightKey);
        
        Snake enemySnake = arena.SpawnSnake();
        AIControl aiControl = enemySnake.gameObject.AddComponent<AIControl>();
        arena.SpawnCollectable();
    }

    public void HandleCollision(SnakeSegment segment, Collider other) {
        if(other.gameObject.GetComponent<Collectable>() != null) {
            CollectablePickedUp(segment, other.gameObject.GetComponent<Collectable>());
        }
        if(other.gameObject.GetComponent<SnakeSegment>() != null) {
            // SnakeCrash(segment, other.gameObject.GetComponent<SnakeSegment>());
        }
        if(other.gameObject.GetComponent<Portal>() != null) {
            Teleport(segment, other.gameObject.GetComponent<Portal>());
        }
    }

    public void CollectablePickedUp(SnakeSegment segment, Collectable collectable) {
        collectable.gameObject.SetActive(false);
        GameObject.Destroy(collectable.gameObject);

        Snake snake = segment.ParentSnake;
        SnakeSegment newSegment = snake.AddSegment(collectable.PowerType);

        arena.SpawnCollectable();
        if(snake.Player != null) IncrementPlayerScore(snake.Player, collectable.Score);
    }

    public void SnakeCrash(SnakeSegment segment1, SnakeSegment segment2) {
        if(segment1.IsHead) KillSnake(segment1.ParentSnake);
        if(segment2.IsHead) KillSnake(segment2.ParentSnake);
    }

    public void Teleport(SnakeSegment segment, Portal portal) {
        // portal.Teleport(segment);
    }

    private void KillSnake(Snake snake) {
        snake.gameObject.SetActive(false);
        GameObject.Destroy(snake.gameObject);
    }

    public void IncrementPlayerScore(Player player, int score) {
        player.Score += score;
        gUI.UpdatePlayerScore(player);
    }
}
