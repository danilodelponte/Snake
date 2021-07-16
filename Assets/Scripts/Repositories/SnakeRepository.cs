using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeRepository : Repository<Snake>
{
    public static Snake Prefab { get => PrefabCache.Load<Snake>("Snake"); }

    public static Snake Build(Vector3 position, Quaternion rotation){
        Debug.Log("Building snake");
        var snake = GameObject.Instantiate(Prefab, position, rotation);
        snake.gameObject.AddComponent<SnakeRepository>();

        snake.AddHead();
        snake.AddSegment(Vector3.up);
        snake.AddSegment(Vector3.up);

        return snake;
    }

    public static void Destroy(Snake snake) {
        GameObject.Destroy(snake.gameObject);
    }
}
