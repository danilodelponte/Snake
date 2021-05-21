﻿using System.Collections;
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
    public SnakeControl Control { get; }

    public SnakeSnapshot(Snake snake) {
        Player = snake.Player;
        Name = snake.gameObject.name;
        Color = snake.Color;
        Position = snake.transform.position;
        Control = snake.GetComponent<SnakeControl>();
        Head = new SnakeSegmentSnapshot(snake.Head);
    }

    public Snake Load() {
        GameObject go = (GameObject) GameObject.Instantiate(snakePrefab, Position, Quaternion.Euler(0,0,0));
        Snake snake = (Snake) go.GetComponent<Snake>();
        snake.Player = Player;
        snake.gameObject.name = Name;
        snake.Head = Head.Load(snake);
        snake.Color = Color;
        if(Control) snake.gameObject.AddComponent(Control.GetType());
        return snake;
    }
}