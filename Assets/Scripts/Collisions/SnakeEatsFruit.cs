using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEatsFruit
{
    public SnakeEatsFruit(SnakeSegment segment, Fruit fruit) {
        GameObject.Destroy(fruit.gameObject);
        Snake snake = segment.ParentSnake;
        snake.AddSegment();
    }
}
