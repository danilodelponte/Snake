﻿using System.Collections;
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
        snake.Player = player;

        PlayerControl playerControl = snake.gameObject.AddComponent<PlayerControl>();
        playerControl.SetKeys(player.LeftKey, player.RightKey);

        SpawnCollectable();
    }

    private void SpawnCollectable() {
        Vector3 position = arena.RandomPosition();
        Quaternion rotation = Quaternion.Euler(Vector3.zero);
        Collectable collectable = Instantiate(collectablePrefab, position, rotation);
        int chance = Random.Range(0,10);
        if(chance > 7) {
            collectable.SpecialPower = new EnginePower();
            Debug.Log("spawned engine!");
        }
    }

    public void HandleCollision(SnakeSegment segment, Collider other) {
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
        GameObject.Destroy(collectable.gameObject);
        Snake snake = segment.ParentSnake;
        SnakeSegment newSegment = snake.AddSegment(collectable.SpecialPower);
        IncrementPlayerScore(snake.Player, +1);
        SpawnCollectable();
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
