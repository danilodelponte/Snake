using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBomb : SpecialModifier
{
    public float Timer;
    public float MaxTime = 10f;

    public override void FixedUpdate() {
        // explode after some time
        Timer += Time.fixedDeltaTime;
        if(Timer > MaxTime) Explode();
    }

    public override bool CollisionModifier(SnakeSegment segmentCollided, GameObject other) {
        Snake snake = segmentCollided.Snake;
        if(segmentCollided != snake.Head) return false;

        // when another collectable is picked up they do not activate special Modifiers
        if(other.GetComponent<Collectable>() != null) {
            Collectable collectable = other.GetComponent<Collectable>();
            collectable.Modifier = null;
            GameplayMode.CollectablePickUp(segmentCollided, collectable);
            Debug.Log("Got Bomb!");
            return true;
        }

        // when another snake is hit, it also dies
        else if(other.GetComponent<SnakeSegment>() != null) {
            SnakeSegment otherSegment = other.GetComponent<SnakeSegment>();
            GameplayMode.KillSnake(otherSegment.Snake);
            Explode();
            return true;
        }
        return false;
    }

    private void Explode() {
        Snake snake = Segment.Snake;

        // all segments accumulated in front of the head are severed off
        List<SnakeSegment> segments = new List<SnakeSegment>();
        while(snake.Head != Segment) {
            segments.Add(snake.Head);
            snake.Head.gameObject.SetActive(false);
            snake.Head = snake.Head.NextSegment;
        }

        // head bomb is also removed
        snake.Head = Segment.NextSegment;
        Segment.gameObject.SetActive(false);
        GameObject.Destroy(Segment.gameObject);

        // segments accumulated in front of the head are shot like bullets following their current direction
        Vector3 direction = snake.GetComponent<SnakeMovement>().CurrentDirection;
        foreach (var segment in segments) {
            Vector3 segmentPosition = segment.gameObject.transform.position;
            Vector3 bombPosition = segmentPosition + direction;
            GameObject.Destroy(segment.gameObject);
            Bomb bomb = BombRepository.Build(bombPosition, GameplayMode);
            bomb.Shoot(direction);
        }
    }
}
