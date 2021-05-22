using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBomb : SpecialModifier
{
    private static Bomb prefab;

    private float timer;
    private float maxTime = 10f;
    private static Bomb Prefab { get => LoadPrefab(); }

    public override void Activate()
    {
        Debug.Log("Got Bomb!");
        base.Activate();
    }
    
    private static Bomb LoadPrefab() {
        if(prefab == null) prefab = Resources.Load<Bomb>("Prefabs/Bomb");
        return prefab;
    }

    public override void FixedUpdate() {
        // explode after some time
        timer += Time.deltaTime;
        if(timer > maxTime) Explode();
    }

    public override bool CollisionModifier(SnakeSegment segmentCollided, Collider other)
    {
        // when another collectable is picked up they do not activate special Modifiers
        if(other.gameObject.GetComponent<Collectable>() != null) {
            Collectable collectable = other.gameObject.GetComponent<Collectable>();
            collectable.Modifier = null;
            GameplayController.Singleton.CollectablePickedUp(segmentCollided, collectable);
            Debug.Log("Got Bomb!");
            return true;
        }

        // when another snake is hit, it also dies
        else if(other.gameObject.GetComponent<SnakeSegment>() != null) {
            SnakeSegment otherSegment = other.gameObject.GetComponent<SnakeSegment>();
            GameplayController.Singleton.KillSnake(otherSegment.Snake);
        }
        return false;
    }

    private void Explode() {
        Deactivate();
        Snake snake = SnakeSegment.Snake;

        // all segments accumulated in front of the head are severed off
        List<SnakeSegment> segments = new List<SnakeSegment>();
        while(snake.Head != SnakeSegment) {
            segments.Add(snake.Head);
            snake.Head.gameObject.SetActive(false);
            snake.Head = snake.Head.NextSegment;
        }

        // head bomb is also removed
        snake.Head = SnakeSegment.NextSegment;
        SnakeSegment.gameObject.SetActive(false);
        GameObject.Destroy(SnakeSegment.gameObject);

        // segments accumulated in front of the head are shot like bullets following their current direction
        foreach (var segment in segments) {
            Vector3 direction = segment.CurrentDirection;
            Vector3 position = segment.gameObject.transform.position + direction;
            GameObject.Destroy(segment.gameObject);
            Bomb bomb = GameObject.Instantiate(Prefab, position, Quaternion.Euler(0,0,0));
            bomb.Shoot(direction);
        }
    }
}
