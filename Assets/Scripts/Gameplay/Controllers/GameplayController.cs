using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Singleton;

    [SerializeField] private Collectable collectablePrefab;
    [SerializeField] private Snake snakePrefab;
    [SerializeField] private GUIController gUI;
    [SerializeField] private Arena arena;

    void Awake()
    {
        InitSingleton();

        arena.GenerateGrid();
        // arena.GridDebug();

        // for(int i = 0; i < 3; i++) {
        //     Snake enemySnake = SpawnSnake();
        //     enemySnake.gameObject.name = $"snake {i}";
        //     enemySnake.Color = UnityEngine.Random.ColorHSV();
        //     AIControl aiControl = enemySnake.gameObject.AddComponent<AIControl>();
        //     SpawnCollectable();
        // }

        Player[] players = GameManager.Instance.Players;
        // if(players == null) {
        //     Player player = new Player("dan", KeyCode.A, KeyCode.S);
        //     players = new Player[1];
        //     players[0] = player;
        // }
        foreach (Player player in players) {
            InitPlayer(player);
        }
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

        var snake = SpawnSnake();
        snake.Player = player;
        snake.Color = player.Color;
        PlayerControl playerControl = snake.gameObject.AddComponent<PlayerControl>();
        playerControl.SetKeys(snake.Player.LeftKey, snake.Player.RightKey);
        
        Snake enemySnake = SpawnSnake();
        AIControl aiControl = enemySnake.gameObject.AddComponent<AIControl>();
        SpawnCollectable();
    }

    public Snake SpawnSnake() {
        var snake = Instantiate(snakePrefab, arena.RandomPosition(), Quaternion.Euler(0,0,0));
        snake.AddHead();
        snake.AddSegment();
        snake.AddSegment();
        return snake;
    }

    public Collectable SpawnCollectable() {
        Collectable collectable = Instantiate(collectablePrefab, arena.RandomPosition(), Quaternion.Euler(0, 0, 0));

        int chance = UnityEngine.Random.Range(0,10);
        if(chance > 2) {
            collectable.SpecialPower = new TimeTravel();
            Debug.Log("spawned time travel!");
        }
        return collectable;
    }

    public void HandleCollision(SnakeSegment segment, Collider other) {
        if(other.gameObject.GetComponent<Collectable>() != null) {
            CollectablePickedUp(segment, other.gameObject.GetComponent<Collectable>());
        }
        if(other.gameObject.GetComponent<SnakeSegment>() != null) {
            SnakeCrash(segment, other.gameObject.GetComponent<SnakeSegment>());
        }
    }

    public void CollectablePickedUp(SnakeSegment segment, Collectable collectable) {
        collectable.gameObject.SetActive(false);
        GameObject.Destroy(collectable.gameObject);

        Snake snake = segment.ParentSnake;
        SnakeSegment newSegment = snake.AddSegment(collectable.SpecialPower);

        SpawnCollectable();
        if(snake.Player != null) IncrementPlayerScore(snake.Player, collectable.Score);
    }

    public void SnakeCrash(SnakeSegment segment1, SnakeSegment segment2) {
        if(segment1.IsHead) KillSnake(segment1.ParentSnake);
        if(segment2.IsHead) KillSnake(segment2.ParentSnake);
    }

    public Snapshot CreateSnapshot() {
        return Snapshot.Create();
    }

    public void LoadSnapshot(Snapshot snapshot) {
        DestroyAll();
        snapshot.Load();
    }

    private void DestroyAll() {
        Snake[] snakes = GameObject.FindObjectsOfType<Snake>();
        foreach (var snake in snakes) {
            GameObject.Destroy(snake.gameObject);
        }

        Collectable[] collectables = GameObject.FindObjectsOfType<Collectable>();
        foreach (var collectable in collectables) {
            GameObject.Destroy(collectable.gameObject);
        }
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
