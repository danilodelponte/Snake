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
        SnakeSnapshot[] snappedSnakes = SnapshotSnakes();
        CollectableSnapshot[] snapedCollectables = SnapshotCollectable();

        return new Snapshot(snappedSnakes, snapedCollectables);
    }

    private static SnakeSnapshot[] SnapshotSnakes() {
        GameObject[] snakes = GameObject.FindGameObjectsWithTag("Snake");
        SnakeSnapshot[] snappedSnakes = new SnakeSnapshot[snakes.Length];
        for (int i = 0; i < snakes.Length; i++) {
            snappedSnakes[i] = new SnakeSnapshot(snakes[i].GetComponent<Snake>());
        }
        return snappedSnakes;
    }

    private static CollectableSnapshot[] SnapshotCollectable() {
        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");
        CollectableSnapshot[] snapedCollectables = new CollectableSnapshot[collectables.Length];
        for (int i = 0; i < collectables.Length; i++) {
            snapedCollectables[i] = new CollectableSnapshot(collectables[i].GetComponent<Collectable>());
        }
        return snapedCollectables;
    }

    public void Load() {
        foreach (var snapSnake in SnappedSnakes) {
            var snake = snapSnake.Load();
            if(snake.Player != null) {
                PlayerControl playerControl = snake.gameObject.GetComponent<PlayerControl>();
                playerControl.SetKeys(snake.Player.LeftKey, snake.Player.RightKey);
            }
        };

        foreach (var collectable in SnappedCollectables) {
            collectable.Load();
        };
    }
}
