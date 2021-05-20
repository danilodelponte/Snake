using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Snapshot
{
    public SnakeSnapshot[] SnappedSnakes { get; set; }
    public CollectableSnapshot[] SnappedCollectables { get; set; }

    public Snapshot(SnakeSnapshot[] snakes, CollectableSnapshot[] collectables) {
        SnappedSnakes = snakes;
        SnappedCollectables = collectables;
    }

    public static Snapshot Create() {
        Debug.Log("Snapshot Create");
        Snake[] snakes = GameObject.FindObjectsOfType<Snake>();
        SnakeSnapshot[] snappedSnakes = new SnakeSnapshot[snakes.Length];
        for (int i = 0; i < snakes.Length; i++) {
            snappedSnakes[i] = new SnakeSnapshot(snakes[i]);
        }

        Collectable[] collectables = GameObject.FindObjectsOfType<Collectable>();
        CollectableSnapshot[] snapedCollectables = new CollectableSnapshot[collectables.Length];
        for (int i = 0; i < collectables.Length; i++) {
            snapedCollectables[i] = new CollectableSnapshot(collectables[i]);
        }

        return new Snapshot(snappedSnakes, snapedCollectables);
    }

    public void Load() {
        Debug.Log("Snapshot Load");
        foreach (var snapSnake in SnappedSnakes) {
            var snake = snapSnake.Load();
            if(snake.Player != null) {
                PlayerControl playerControl = snake.gameObject.AddComponent<PlayerControl>();
                playerControl.SetKeys(snake.Player.LeftKey, snake.Player.RightKey);
            } else {
                AIControl aiControl = snake.gameObject.AddComponent<AIControl>();
            }
        };

        foreach (var collectable in SnappedCollectables) {
            collectable.Load();
        };
    }
}
