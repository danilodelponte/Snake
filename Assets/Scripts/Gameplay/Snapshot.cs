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
        SnakeSnapshot[] snappedSnakes = SnapshotSnakes(SnakeRepository.All());
        CollectableSnapshot[] snapedCollectables = SnapshotCollectable(CollectableRepository.All());

        return new Snapshot(snappedSnakes, snapedCollectables);
    }

    private static SnakeSnapshot[] SnapshotSnakes(List<Snake> snakes) {
        SnakeSnapshot[] snappedSnakes = new SnakeSnapshot[snakes.Count];
        for (int i = 0; i < snakes.Count; i++) {
            snappedSnakes[i] = new SnakeSnapshot(snakes[i]);
        }
        return snappedSnakes;
    }

    private static CollectableSnapshot[] SnapshotCollectable(List<Collectable> collectables) {
        CollectableSnapshot[] snapedCollectables = new CollectableSnapshot[collectables.Count];
        for (int i = 0; i < collectables.Count; i++) {
            snapedCollectables[i] = new CollectableSnapshot(collectables[i]);
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
