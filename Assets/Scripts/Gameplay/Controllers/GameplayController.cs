using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Singleton;

    [SerializeField] private GUIController gUI;
    [SerializeField] private Collectable collectablePrefab;
    [SerializeField] private Snake snakePrefab;
    [SerializeField] private Arena arena;

    void Awake()
    {
        InitSingleton();
        Player[] players = GameManager.Instance.Players;
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
        SpawnSnake(player);
        gUI.AddPlayerLabel(player);
    }

    private void SpawnSnake(Player player) {
        Vector3 position = arena.RandomPosition();
        var snake = Instantiate(snakePrefab, position, Quaternion.Euler(Vector3.zero));
        snake.AddHead();
        snake.AddSegment();
        snake.AddSegment();
        snake.Player = player;
        PlayerControl playerControl = snake.gameObject.AddComponent<PlayerControl>();
        playerControl.SetKeys(snake.Player.LeftKey, snake.Player.RightKey);

        SpawnCollectable();
    }

    public void SpawnCollectable() {
        Vector3 position = arena.RandomPosition();
        Quaternion rotation = Quaternion.Euler(Vector3.zero);
        Collectable collectable = Instantiate(collectablePrefab, position, rotation);
        int chance = UnityEngine.Random.Range(0,10);
        if(chance > 4) {
            collectable.SpecialPower = new TimeTravel();
            Debug.Log("spawned time travel!");
        }
    }

    public void HandleCollision(SnakeSegment segment, Collider other) {
        if(segment.SpecialPower != null && segment.SpecialPower.HandleCollision(segment, other)) return;

        if(other.gameObject.GetComponent<Collectable>() != null) {
            CollectablePickedUp(segment, other.gameObject.GetComponent<Collectable>());
        }
        if(other.gameObject.GetComponent<SnakeSegment>() != null) {
            SnakeCrash(segment, other.gameObject.GetComponent<SnakeSegment>());
        }
        if(other.gameObject.GetComponent<Portal>() != null) {
            Teleport(segment, other.gameObject.GetComponent<Portal>());
        }
    }

    public void CollectablePickedUp(SnakeSegment segment, Collectable collectable) {
        collectable.gameObject.SetActive(false);
        GameObject.Destroy(collectable.gameObject);
        Snake snake = segment.ParentSnake;
        SnakeSegment newSegment = snake.AddSegment(collectable.SpecialPower);
        if(newSegment.SpecialPower != null) newSegment.SpecialPower.Activate();
        IncrementPlayerScore(snake.Player, +1);
        SpawnCollectable();
    }

    public void SnakeCrash(SnakeSegment segment1, SnakeSegment segment2) {
        if(segment1.IsHead) KillSnake(segment1.ParentSnake);
        if(segment2.IsHead) KillSnake(segment2.ParentSnake);
    }

    public void Teleport(SnakeSegment segment, Portal portal) {
        portal.Teleport(segment);
    }

    private void KillSnake(Snake snake) {
        snake.gameObject.SetActive(false);
        GameObject.Destroy(snake.gameObject);
    }

    public void IncrementPlayerScore(Player player, int score) {
        player.Score += score;
        gUI.UpdatePlayerScore(player);
    }

    public Snapshot SaveSnapshot(){
        Snake[] snakes = GameObject.FindObjectsOfType<Snake>();
        Snake[] savedSnakes = new Snake[snakes.Length];
        for (int i = 0; i < snakes.Length; i++) {
            Snake snake = snakes[i];
            if(!snake.gameObject.activeSelf) continue;

            savedSnakes[i] = snake.Snapshot();
        };

        Collectable[] collectables = GameObject.FindObjectsOfType<Collectable>();
        Collectable[] savedCollectables = new Collectable[collectables.Length];
        for (int i = 0; i < collectables.Length; i++) {
            Collectable collectable = collectables[i];
            if(!collectable.gameObject.activeSelf) continue;

            savedCollectables[i] = collectable.Snapshot();
        };

        return new Snapshot(savedSnakes, savedCollectables);
    }

    public void LoadSnapshot(Snapshot snapshot) {
        Snake[] snakes = GameObject.FindObjectsOfType<Snake>();
        foreach (var snake in snakes) {
            if(!snake.gameObject.activeSelf) continue;

            snake.gameObject.SetActive(false);
            Destroy(snake.gameObject);
        }

        Collectable[] collectables = GameObject.FindObjectsOfType<Collectable>();
        foreach (var collectable in collectables) {
            if(!collectable.gameObject.activeSelf) continue;

            collectable.gameObject.SetActive(false);
            Destroy(collectable.gameObject);
        }

        foreach (var snake in snapshot.Snakes) {
            PlayerControl playerControl = snake.gameObject.AddComponent<PlayerControl>();
            playerControl.SetKeys(snake.Player.LeftKey, snake.Player.RightKey);
            snake.gameObject.SetActive(true);
        };

        foreach (var collectable in snapshot.Collectables) {
            collectable.gameObject.SetActive(true);
        };
    }
}
