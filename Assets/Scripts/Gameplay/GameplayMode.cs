using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Multiplayer,
    AiOnly,
    ModifierPlayground
}

public class GameplayMode
{
    protected GameplayController controller { get; }
    protected Arena arena;
    protected GameData gameData { get; }

    public static GameplayMode Build(GameMode mode, GameData gameData, GameplayController controller) {
        switch (mode) {
            case GameMode.Multiplayer : return new MultiplayerMode(gameData, controller);
            case GameMode.AiOnly: return new AIOnlyMode(gameData, controller);
            case GameMode.ModifierPlayground: return new ModifiersPlayground(gameData, controller);
        }
        return new ModifiersPlayground(gameData, controller);
    }

    public GameplayMode(GameData gameData, GameplayController controller) {
        this.controller = controller;
        this.gameData = gameData;
    }

    public virtual void Start() {
        CreateArena();
    }

    public virtual void Restart() {
        DestroyAll();
        Start();        
    }

    public virtual SpecialModifier GenerateModifier() { return null; }
    public virtual void GameStateCheck() {}

    public virtual void FixedUpdate() {
        arena.UpdateGrid();
        foreach (var snake in SnakeRepository.All()) {
            snake.GetComponent<SnakeMovement>()?.UpdateMovement();
        }
    }

    // Evaluators 
    private bool EvaluateCollision(SnakeSegment segmentCollided, GameObject other) {
        Snake snake = segmentCollided.Snake;
        foreach (SnakeSegment segment in snake.Segments()) {
            SpecialModifier modifier = segment.GetComponent<SpecialComponent>().Modifier;
            if(modifier!=null && modifier.CollisionModifier(segmentCollided, other))
                return true;
        }

        return false;
    }

    public float EvaluateMovementDelta(Snake snake, SnakeMovementData movementData) {
        float movingDelta = movementData.BaseMovingDeltaTime;

        foreach (SnakeSegment segment in snake.Segments()) {
            movingDelta += movementData.MovingDeltaSegmentIncrease;
            SpecialModifier modifier = segment.GetComponent<SpecialComponent>().Modifier;
            modifier?.MovementModifier(ref movingDelta);
        }

        return Mathf.Clamp(movingDelta, movementData.MinMovingDeltaTime, movementData.MaxMovingDeltaTime);
    }

    public Vector3 EvaluateDirection(Snake snake, ref Vector3 currentDirection) {
        foreach (SnakeSegment segment in snake.Segments()) {
            SpecialModifier modifier = segment.GetComponent<SpecialComponent>().Modifier;
            modifier?.DirectionModifier(ref currentDirection);
        }

        return currentDirection;
    }

    public void EvaluateScoreGain(Snake snake, ref int gain) {
        foreach (SnakeSegment segment in snake.Segments()) {
            SpecialModifier modifier = segment.GetComponent<SpecialComponent>().Modifier;
            modifier?.ScoreGainModifier(ref gain);
        }
    }

    public bool EvaluateDeath(Snake snake) {
        foreach (SnakeSegment segment in snake.Segments()) {
            SpecialModifier modifier = segment.GetComponent<SpecialComponent>().Modifier;
            if(modifier != null && modifier.DeathModifier()) return true;
        }
        return false;
    }

    // Game Actions
    public void MoveSnake(Snake snake) {
        Vector3 direction = GetSnakeDirection(snake);
        if(direction == Vector3.zero) return; 

        Vector3 currentPosition;
        Vector3 nextPosition = snake.Head.transform.position + direction;
        arena.KeepWithinBounds(ref nextPosition);

        PathNode node = arena.GetNode(nextPosition);
        GameObject nodeObject = node.NodeObject as GameObject;
        if(nodeObject != null && HandleSnakeCollision(snake.Head, nodeObject)) return;

        foreach (SnakeSegment segment in snake.Segments()) {
            currentPosition = segment.transform.position;
            MoveSegment(segment, nextPosition);
            nextPosition = currentPosition;
        }
    }

    private Vector3 GetSnakeDirection(Snake snake) {
        SnakeControl control = snake.GetComponent<SnakeControl>();
        SnakeMovement snakeMovement = snake.GetComponent<SnakeMovement>();
        if(control == null || snakeMovement == null) return Vector3.zero;

        Vector3 intendedDirection = control.GetDirection();
        Vector3 currentDirection = snakeMovement.CurrentDirection;

        if((intendedDirection + currentDirection) != Vector3.zero)
            currentDirection = intendedDirection;
        
        EvaluateDirection(snake, ref currentDirection);
        snakeMovement.CurrentDirection = currentDirection;
        return snakeMovement.CurrentDirection;
    }

    private void MoveSegment(SnakeSegment segment, Vector3 nextPosition) {
        Vector3 direction = nextPosition - segment.transform.position;
        arena.SetNode(segment.transform.position, null);
        segment.transform.position = nextPosition;
        segment.transform.rotation = GetRotation(direction);
        arena.SetNode(segment.transform.position, segment.gameObject);
    }

    public bool HandleSnakeCollision(SnakeSegment snakeHead, GameObject other) {
        if(EvaluateCollision(snakeHead, other)) return true;
        
        if(other.GetComponent<Collectable>() != null)
            return CollectablePickUp(snakeHead, other.GetComponent<Collectable>());

        if(other.GetComponent<SnakeSegment>() != null)
            return SnakeCrash(snakeHead, other.GetComponent<SnakeSegment>());

        return false;
    }

    private Quaternion GetRotation(Vector3 direction) {
        direction.Normalize();
        Quaternion rotation = Quaternion.identity;
        if(direction.x == -1) rotation = Quaternion.Euler(0,0,90);
        else if(direction.y == -1) rotation = Quaternion.Euler(0,0,180);
        else if(direction.x == 1) rotation = Quaternion.Euler(0,0,270);
        return rotation;
    }

    public bool CollectablePickUp(SnakeSegment segment, Collectable collectable) {
        collectable.gameObject.SetActive(false);
        GameObject.Destroy(collectable.gameObject);
        SpawnCollectable(GenerateModifier());

        Snake snake = segment.Snake;
        if(snake.isAI) collectable.Modifier = null;
        Vector3 direction = collectable.transform.position - segment.transform.position;
        SnakeSegment newSegment = snake.AddSegment(direction);
        collectable.Modifier?.Activate(newSegment, this);
        
        IncrementPlayerScore(snake, collectable.Score);
        return true;
    }

    public void IncrementPlayerScore(Snake snake, int increment) {
        if(!snake.IsPlayer) return;

        Player player = snake.Player;
        EvaluateScoreGain(snake, ref increment);
        player.Score += increment;
        controller.GUI.UpdatePlayerScore(player);
    }

    public bool SnakeCrash(SnakeSegment segment1, SnakeSegment segment2) {
        Snake snake1 = segment1.Snake;
        if(snake1.Head == segment1) {
            KillSnake(snake1);
            return true;
        }
        return false;
    }

    public void KillSnake(Snake snake) {
        if(EvaluateDeath(snake)) return;
        
        DestroySnake(snake);
        GameStateCheck();
    }

    // Snapshot 
    public Snapshot CreateSnapshot() {
        return Snapshot.Create();
    }

    public void LoadSnapshot(Snapshot snapshot) {
        DestroyAll();
        snapshot.Load();
    }

    //Spawning 
    public void CreateArena() {
        if(arena != null) return;

        arena = new GameObject("Arena").AddComponent<Arena>();
        arena.Generate(40, 20);
    }

    public Snake SpawnSnake(SnakeTemplate template = null) {
        if(template == null) template = new SnakeTemplate();
        Snake snake = SnakeRepository.Build(arena.EquallyDistributedPosition(), Quaternion.identity);        

        foreach (SnakeSegment segment in snake.Segments()) {
            Vector3 currentPosition = segment.transform.position;
            arena.KeepWithinBounds(ref currentPosition);
            segment.transform.position = currentPosition;
        }

        template.Apply(snake, this);
        return snake;
    }

    public void DestroySnake(Snake snake) {
        SnakeRepository.Destroy(snake);
    }

    public void SpawnPlayerSnake(Player player) {
        var snake = SpawnSnake(player.SnakeTemplate);
        snake.Player = player;
        snake.Color = player.Color;

        SnakeMovement movement = snake.gameObject.AddComponent<SnakeMovement>();
        movement.Initialize(gameData.PlayerMovementData, this);

        PlayerControl playerControl = snake.gameObject.AddComponent<PlayerControl>();
        playerControl.SetKeys(snake.Player.LeftKey, snake.Player.RightKey);
    }

    public Snake SpawnEnemySnake() {
        Snake enemySnake = SpawnSnake();
        enemySnake.Color = UnityEngine.Random.ColorHSV(0,1,.3f,.5f,.3f,.5f);
        AIControl aiControl = enemySnake.gameObject.AddComponent<AIControl>();

        SnakeMovement movement = enemySnake.gameObject.AddComponent<SnakeMovement>();
        movement.Initialize(gameData.AiMovementData, this);

        return enemySnake;
    }

    public Snake SpawnDummySnake(int numberOfSegments, Vector3 snakeDirection) {
        Snake snake = SpawnSnake();
        snake.Color = Color.gray;
        for(int i = 3; i < numberOfSegments; i++) {
            snake.AddSegment(snakeDirection);
        }
        return snake;
    }
    
    public Collectable SpawnCollectable(SpecialModifier modifier = null) {
        return CollectableRepository.Build(
            arena.EquallyDistributedPosition(), Quaternion.identity, modifier
        );
    }

    public void DestroyAll() {
        SnakeRepository.DestroyAll();
        CollectableRepository.DestroyAll();
    }
}
