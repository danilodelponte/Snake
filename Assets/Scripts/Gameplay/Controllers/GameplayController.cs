﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Singleton;

    [SerializeField] private GUIController gUI;
    [SerializeField] private Fruit fruitPrefab;
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
        snake.Player = player;

        PlayerControl playerControl = snake.gameObject.AddComponent<PlayerControl>();
        playerControl.SetKeys(player.LeftKey, player.RightKey);

        SpawnCollectible();
    }

    private void SpawnCollectible() {
        Vector3 position = arena.RandomPosition();
        Instantiate(fruitPrefab, position, Quaternion.Euler(Vector3.zero));
    }

    public void HandleCollision(SnakeSegment segment, Collider other) {
        if(other.gameObject.GetComponent<Fruit>() != null) {
            SnakeEatsFruit(segment, other.gameObject.GetComponent<Fruit>());
        }
        if(other.gameObject.GetComponent<SnakeSegment>() != null) {
            SnakeCrash(segment, other.gameObject.GetComponent<SnakeSegment>());
        }
        if(other.gameObject.GetComponent<Portal>() != null) {
            Teleport(segment, other.gameObject.GetComponent<Portal>());
        }
    }

    public void SnakeEatsFruit(SnakeSegment segment, Fruit fruit) {
        GameObject.Destroy(fruit.gameObject);
        Snake snake = segment.ParentSnake;
        snake.AddSegment();
        IncrementPlayerScore(snake.Player, +1);
        SpawnCollectible();
    }

    public void SnakeCrash(SnakeSegment segment1, SnakeSegment segment2) {
        if(segment1.IsHead()) KillSnake(segment1.ParentSnake);
        if(segment2.IsHead()) KillSnake(segment2.ParentSnake);
    }

    public void Teleport(SnakeSegment segment, Portal portal) {
        portal.Teleport(segment);
    }

    private void KillSnake(Snake snake) {
        GameObject.Destroy(snake.gameObject);
    }

    public void IncrementPlayerScore(Player player, int score) {
        player.Score += score;
        gUI.UpdatePlayerScore(player);
    }
}
