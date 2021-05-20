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

        Player[] players = GameManager.Instance.Players;
        foreach (Player player in players) {
            InitPlayer(player);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(Time.timeScale == 0) ResumeGame();
            else PauseGame();
        }
    }

    public void ResumeGame() {
        gUI.HidePausePanel();
        Time.timeScale = 1;
    }

    public void PauseGame() {
        Time.timeScale = 0;
        gUI.ShowPausePanel();
    }

    public void LoadSelectionMenu() {
        Time.timeScale = 1;
        GameManager.Instance.LoadMainMenu();
    }

    private void AiOnly(int numberOfSnakes) {
        for(int i = 0; i < numberOfSnakes; i++) {
            Snake enemySnake = SpawnSnake();
            enemySnake.gameObject.name = $"snake {i}";
            enemySnake.Color = UnityEngine.Random.ColorHSV(0,1,0,1,.5f,.7f);
            AIControl aiControl = enemySnake.gameObject.AddComponent<AIControl>();
            SpawnCollectable();
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
        enemySnake.Color = UnityEngine.Random.ColorHSV(0,1,.3f,.5f,.3f,.5f);
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
        SnakeSegment newSegment = snake.AddSegment();
        if(collectable.SpecialPower != null) {
            newSegment.SpecialPower = collectable.SpecialPower;
            newSegment.SpecialPower.Activate();
        }

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
        Debug.Log($"{snake} has died!");
        snake.gameObject.SetActive(false);
        GameObject.Destroy(snake.gameObject);
    }

    public void IncrementPlayerScore(Player player, int score) {
        player.Score += score;
        gUI.UpdatePlayerScore(player);
    }
}
