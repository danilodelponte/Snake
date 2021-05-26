using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSnapshot {

    public string Name { get; }
    public Player Player { get; }
    public SnakeSegmentSnapshot Head { get; }
    public Vector3 Direction { get; }
    public Vector3 Position { get; }
    public Color Color { get; }
    public SnakeControl Control { get; }
    public float baseMovingDeltaTime;
    public float maxMovingDeltaTime;
    public float minMovingDeltaTime;

    public SnakeSnapshot(Snake snake) {
        Player = snake.Player;
        Name = snake.gameObject.name;
        Color = snake.Color;
        Position = snake.transform.position;
        baseMovingDeltaTime = snake.baseMovingDeltaTime;
        minMovingDeltaTime = snake.minMovingDeltaTime;
        maxMovingDeltaTime = snake.maxMovingDeltaTime;
        Control = snake.GetComponent<SnakeControl>();
        Head = new SnakeSegmentSnapshot(snake.Head);
    }

    public Snake Load() {
        Snake snake = GameObject.Instantiate(Snake.Prefab, Position, Quaternion.identity);
        snake.Player = Player;
        snake.gameObject.name = Name;
        snake.baseMovingDeltaTime = baseMovingDeltaTime;
        snake.minMovingDeltaTime = minMovingDeltaTime;
        snake.maxMovingDeltaTime = maxMovingDeltaTime;
        snake.Head = Head.Load(snake);
        snake.Color = Color;
        if(Control != null) snake.gameObject.AddComponent(Control.GetType());
        return snake;
    }
}
