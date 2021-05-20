using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSnapshot {

    private static GameObject snakePrefab = Resources.Load("Prefabs/Snake") as GameObject;

    public string Name { get; }
    public Player Player { get; }
    public SnakeSegmentSnapshot Head { get; }
    public Vector3 Direction { get; }
    public Vector3 Position { get; }
    public Color Color { get; }

    public SnakeSnapshot(Snake snake) {
        Player = snake.Player;
        Name = snake.gameObject.name;
        Color = snake.Color;
        Direction = snake.Direction;
        Position = snake.transform.position;
        Head = new SnakeSegmentSnapshot(snake.Head);
    }

    public Snake Load() {
        GameObject go = (GameObject) GameObject.Instantiate(snakePrefab, Position, Quaternion.Euler(0,0,0));
        Snake snake = (Snake) go.GetComponent<Snake>();
        snake.Player = Player;
        snake.gameObject.name = Name;
        snake.Direction = Direction;
        snake.Head = Head.Load(snake);
        snake.Color = Color;
        return snake;
    }
}
