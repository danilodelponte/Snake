using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Singleton;
    [SerializeField] private GUIController GUI;
    [SerializeField] private Arena arena;

    enum GameState
    {
        RUNNING,
        PAUSED,
        GAMEOVER
    }
    private GameState state = GameState.RUNNING;
    public GameplayMode GameMode { get; set; }

    void Start()
    {
        CreateSingleton();
        GameMode = GameManager.Instance.GameMode;
        GameMode.Start(this, GUI);
    }

    private void CreateSingleton() {
        if (Singleton != null) {
            Destroy(gameObject);
            return;
        }
        Singleton = this;
    }

    private void FixedUpdate() {
        GameObject[] snakes = GameObject.FindGameObjectsWithTag("Snake");
        foreach (var snake in snakes) {
            snake.GetComponent<Snake>().UpdateMovement();
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(Time.timeScale == 0) ResumeGame();
            else PauseGame();
        }

        arena.UpdateGrid();
        GameObject[] snakes = GameObject.FindGameObjectsWithTag("Snake");
        foreach (var snake in snakes) {
            snake.GetComponent<Snake>().UpdateDirection();
        }
    }

    public void ResumeGame() {
        if(state != GameState.PAUSED) return;
        state = GameState.RUNNING;
        GUI.HidePausePanel();
        Time.timeScale = 1;
    }

    public void Restart() {
        if(state != GameState.GAMEOVER) return;
        GUI.RemovePlayerLabels();
        GUI.HideGameOverPanel();
        DestroyAll();
        GameMode.Start(this, GUI);
    }

    public void PauseGame() {
        if(state != GameState.RUNNING) return;
        state = GameState.PAUSED;
        Time.timeScale = 0;
        GUI.ShowPausePanel();
    }

    public void GameOver() {
        state = GameState.GAMEOVER;
        GUI.ShowGameOverPanel();
    }

    public void LoadSelectionMenu() {
        Time.timeScale = 1;
        GameManager.Instance.LoadMainMenu();
    }

    public void CreateArena() {
        arena = GameObject.FindObjectOfType<Arena>();
        if(arena != null) return;

        arena = new GameObject("Arena").AddComponent<Arena>();
        arena.Generate(40, 20);
        arena.GridDebug();
    }

    public void SpawnPlayerSnake(Player player) {
        var snake = SpawnSnake(player.SnakeTemplate);
        snake.Player = player;
        snake.Color = player.Color;
        PlayerControl playerControl = snake.gameObject.AddComponent<PlayerControl>();
        playerControl.SetKeys(snake.Player.LeftKey, snake.Player.RightKey);
    }

    public Snake SpawnEnemySnake() {
        Snake enemySnake = SpawnSnake();
        // enemySnake.baseMovingDeltaTime = 0.01f;
        // enemySnake.minMovingDeltaTime = 0.01f;
        // enemySnake.maxMovingDeltaTime = 0.01f;
        enemySnake.Color = UnityEngine.Random.ColorHSV(0,1,.3f,.5f,.3f,.5f);
        AIControl aiControl = enemySnake.gameObject.AddComponent<AIControl>();
        return enemySnake;
    }

    public Snake SpawnDummySnake(int numberOfSegments = 8) {
        Snake snake = SpawnSnake();
        snake.Color = Color.gray;
        for(int i = 3; i < numberOfSegments; i++) {
            snake.AddSegment();
        }
        snake.Head.CurrentDirection = Vector3.zero;
        return snake;
    }

    public Snake SpawnSnake(SnakeTemplate template = null) {
        var snake = Instantiate(Snake.Prefab, arena.EquallyDistributedPosition(), Quaternion.identity);
        snake.Init(template);
        return snake;
    }
    
    public Collectable SpawnCollectable(SpecialModifier modifier = null) {
        Collectable collectable = Instantiate(Collectable.Prefab, arena.EquallyDistributedPosition(), Quaternion.identity);
        collectable.Modifier = modifier;
        return collectable;
    }

    public void HandleCollision(SnakeSegment segment, Collider other) {
        if(other.gameObject.GetComponent<Collectable>() != null) {
            CollectablePickedUp(segment, other.gameObject.GetComponent<Collectable>());
        }
        else if(other.gameObject.GetComponent<SnakeSegment>() != null) {
            SnakeCrash(segment, other.gameObject.GetComponent<SnakeSegment>());
        }
    }

    public void CollectablePickedUp(SnakeSegment segment, Collectable collectable) {
        collectable.gameObject.SetActive(false);
        GameObject.Destroy(collectable.gameObject);
        SpawnCollectable(GameMode.GenerateModifier());

        Snake snake = segment.Snake;
        if(snake.isAI) collectable.Modifier = null;
        SnakeSegment newSegment = snake.AddSegment(collectable.Modifier);
        
        IncrementPlayerScore(snake, collectable.Score);
    }

    public void IncrementPlayerScore(Snake snake, int increment) {
        if(!snake.IsPlayer) return;

        Player player = snake.Player;
        increment = snake.EvaluateScoreGain(increment);
        player.Score += increment;
        GUI.UpdatePlayerScore(player);
    }

    public void SnakeCrash(SnakeSegment segment1, SnakeSegment segment2) {
        if(segment1.IsHead) KillSnake(segment1.Snake);
    }

    public void KillSnake(Snake snake) {
        if(snake.Die()) GameMode.GameStateCheck();
    }

    public Snapshot CreateSnapshot() {
        return Snapshot.Create();
    }

    public void LoadSnapshot(Snapshot snapshot) {
        DestroyAll();
        snapshot.Load();
    }

    private void DestroyAll() {
        GameObject[] snakes = GameObject.FindGameObjectsWithTag("Snake");
        foreach (var snake in snakes) {
            snake.SetActive(false);
            GameObject.Destroy(snake);
        }

        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (var collectable in collectables) {
            collectable.SetActive(false);
            GameObject.Destroy(collectable);
        }
    }
}
