using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public float MovementTimer = 0;
    public Vector3 CurrentDirection = Vector3.up;
    public Snake Snake { get => GetSnake(); }

    private SnakeMovementData data;
    private GameplayMode gameplayMode;
    private Snake snake;

    public void Initialize(SnakeMovementData data, GameplayMode gameplayMode) {
        this.data = data;
        this.gameplayMode = gameplayMode;
    }

    public void UpdateMovement() {
        MovementTimer += Time.fixedDeltaTime;

        float movingDeltaTime = gameplayMode.EvaluateMovementDelta(Snake, data);

        if(MovementTimer >= movingDeltaTime) {
            MovementTimer -= movingDeltaTime;
            gameplayMode.MoveSnake(Snake);
        }
    }

    private Snake GetSnake() {
        if(snake == null) snake = GetComponent<Snake>();

        return snake;
    }
}
