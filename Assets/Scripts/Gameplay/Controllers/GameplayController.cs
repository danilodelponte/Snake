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
        CreateSingleton();
        CreateArena();

        // InitSpecialPowerTesting();
        InitWithPlayers();
        // InitWithAiOnly(5);
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

    private void InitSpecialPowerTesting() {
        Player player = new Player(KeyCode.A, KeyCode.S);
        SpawnPlayerSnake(player);
        SpawnDummySnake(8);
        SpawnCollectable(new DoubleScore());
        gUI.AddPlayerLabel(player);
    }

    private void InitWithPlayers() {
        Player[] players = GameManager.Instance.Players;
        foreach (Player player in players) {
            gUI.AddPlayerLabel(player);
            SpawnPlayerSnake(player);
            SpawnEnemySnake();
            SpawnCollectable();
        }
    }

    private void InitWithAiOnly(int numberOfSnakes) {
        for(int i = 0; i < numberOfSnakes; i++) {
            SpawnEnemySnake();
            SpawnCollectable();
        }
    }

    public void CreateArena() {
        arena = GameObject.FindObjectOfType<Arena>();
        if(arena != null) return;

        arena = new GameObject("Arena").AddComponent<Arena>();
        arena.Generate(40, 20);
        arena.GridDebug();
    }

    private void SpawnPlayerSnake(Player player) {
        var snake = SpawnSnake(player.SnakeTemplate);
        snake.Player = player;
        snake.Color = player.Color;
        PlayerControl playerControl = snake.gameObject.AddComponent<PlayerControl>();
        playerControl.SetKeys(snake.Player.LeftKey, snake.Player.RightKey);
    }

    private Snake SpawnEnemySnake() {
        Snake enemySnake = SpawnSnake();
        enemySnake.Color = UnityEngine.Random.ColorHSV(0,1,.3f,.5f,.3f,.5f);
        AIControl aiControl = enemySnake.gameObject.AddComponent<AIControl>();
        return enemySnake;
    }

    private Snake SpawnDummySnake(int numberOfSegments) {
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

    public Collectable SpawnCollectable() {
        int chance = UnityEngine.Random.Range(0,100);
        SpecialModifier modifier = null;
        
        if(chance < 10) modifier = new EnginePower();
        else if(chance < 30) modifier = new HeadBomb();
        else if(chance < 80) modifier = new BatteringRam();
        else if(chance < 60) modifier = new Confused();
        else if(chance < 100) modifier = new TimeTravel();
        return SpawnCollectable(modifier);
    }
    
    public Collectable SpawnCollectable(SpecialModifier modifier) {
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
        SpawnCollectable();

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
        gUI.UpdatePlayerScore(player);
    }

    public void SnakeCrash(SnakeSegment segment1, SnakeSegment segment2) {
        if(segment1.IsHead) KillSnake(segment1.Snake);
    }

    public void KillSnake(Snake snake) {
        if(snake.isAI) SpawnEnemySnake();
        snake.Die();
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
