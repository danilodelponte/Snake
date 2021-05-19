using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapshot
{
    public Snake[] Snakes { get; set; }
    public Collectable[] Collectables { get; set; }

    public Snapshot(Snake[] snakes, Collectable[] collectables) {
        Snakes = snakes;
        Collectables = collectables;
    }

    public static Snapshot Create() {
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

    public static void Destroy(Snapshot snapshot) {
        foreach (var snake in snapshot.Snakes) {
            snake.gameObject.SetActive(false);
            GameObject.Destroy(snake.gameObject);
        };

        foreach (var collectable in snapshot.Collectables) {
            collectable.gameObject.SetActive(false);
            GameObject.Destroy(collectable.gameObject);
        };
    }

    public static void Load(Snapshot snapshot) {
        Snake[] snakes = GameObject.FindObjectsOfType<Snake>();
        foreach (var snake in snakes) {
            if(!snake.gameObject.activeSelf) continue;

            snake.gameObject.SetActive(false);
            GameObject.Destroy(snake.gameObject);
        }

        Collectable[] collectables = GameObject.FindObjectsOfType<Collectable>();
        foreach (var collectable in collectables) {
            if(!collectable.gameObject.activeSelf) continue;

            collectable.gameObject.SetActive(false);
            GameObject.Destroy(collectable.gameObject);
        }

        foreach (var snake in snapshot.Snakes) {
            snake.gameObject.SetActive(true);
            if(snake.Player != null) {
                PlayerControl playerControl = snake.gameObject.AddComponent<PlayerControl>();
                playerControl.SetKeys(snake.Player.LeftKey, snake.Player.RightKey);
            } else {
                AIControl aiControl = snake.gameObject.AddComponent<AIControl>();
            }
        };

        foreach (var collectable in snapshot.Collectables) {
            collectable.gameObject.SetActive(true);
        };
    }
}
