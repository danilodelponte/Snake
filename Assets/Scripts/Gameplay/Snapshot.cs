using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Snapshot
{
    private static GameObject snakePrefab = Resources.Load("Prefabs/Snake") as GameObject;
    private static GameObject segmentPrefab = Resources.Load("Prefabs/SnakeSegment") as GameObject;
    private static GameObject collectablePrefab = Resources.Load("Collectable") as GameObject;

    public SnakeSnapshot[] SnappedSnakes { get; set; }
    public CollectableSnapshot[] SnappedCollectables { get; set; }

    public class SnakeSnapshot {
        public Player Player { get; set; }
        public SnakeSegmentSnapshot Head { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 Position { get; set; }

        public SnakeSnapshot(Snake snake) {
            Player = snake.Player;
            Direction = snake.Direction;
            Position = snake.transform.position;
            Head = new SnakeSegmentSnapshot(snake.Head);
        }

        public Snake Load() {
            GameObject go = (GameObject) GameObject.Instantiate(snakePrefab, Position, Quaternion.Euler(0,0,0));
            Snake snake = (Snake) go.GetComponent<Snake>();
            snake.Player = Player;
            snake.Direction = Direction;
            snake.Head = Head.Load(snake);
            return snake;
        }
    }

    public class SnakeSegmentSnapshot {

        static private GameObject prefab;
        public SnakeSegmentSnapshot NextSegment { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; } 
        public Vector3 CurrentDirection { get; set; }
        public SpecialPower SpecialPower { get; set; }

        public SnakeSegmentSnapshot(SnakeSegment segment) {
            if(segment.NextSegment != null) NextSegment = new SnakeSegmentSnapshot(segment.NextSegment);
            CurrentDirection = segment.CurrentDirection;
            Position = segment.transform.position;
            Rotation = segment.transform.rotation;
            SpecialPower = segment.SpecialPower;
        }

        public SnakeSegment Load(Snake parent) { 
            GameObject go = (GameObject) GameObject.Instantiate(segmentPrefab, Position, Rotation, parent.transform);
            SnakeSegment segment = (SnakeSegment) go.GetComponent<SnakeSegment>();
            if(NextSegment != null) segment.NextSegment = NextSegment.Load(parent);
            segment.CurrentDirection = CurrentDirection;
            segment.SpecialPower = SpecialPower;
            return segment;
        }
    }

    public class CollectableSnapshot {
        public SpecialPower SpecialPower { get; set; }
        public int Score { get; set; }
        public Vector3 Position { get; set; }

        public CollectableSnapshot(Collectable collectable) {
            SpecialPower = collectable.SpecialPower;
            Score = collectable.Score;
        }

        public Collectable Load() {
            GameObject go = (GameObject) GameObject.Instantiate(collectablePrefab, Position, Quaternion.Euler(0,0,0));
            Collectable collectable = (Collectable) go.GetComponent<Collectable>();
            collectable.SpecialPower = SpecialPower;
            collectable.Score = Score;
            return collectable;
        }
    }

    public Snapshot(SnakeSnapshot[] snakes, CollectableSnapshot[] collectables) {
        SnappedSnakes = snakes;
        SnappedCollectables = collectables;
    }

    public static Snapshot Create() {
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
