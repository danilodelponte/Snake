using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Singleton;
    private int width = 10;
    private int height = 10;
    private List<Player> players = new List<Player>();
    [SerializeField] private Fruit fruitPrefab;
    [SerializeField] private Snake snakePrefab;
    
    void Awake()
    {
        InitSingleton();
        Player playerA = new Player();
        playerA.LeftKey = KeyCode.A;
        playerA.RightKey = KeyCode.S;
        players.Add(playerA);

        Player playerB = new Player();
        playerB.LeftKey = KeyCode.LeftArrow;
        playerB.RightKey = KeyCode.RightArrow;
        players.Add(playerB);

        foreach (Player player in players) {
            SpawnSnake(player);
        }
    }

    private void InitSingleton() {
        if (Singleton != null) {
            Destroy(gameObject);
            return;
        }
        Singleton = this;
    }

    private void SpawnSnake(Player player) {
        Vector3 position = RandomVector3(width, height);
        var snake = Instantiate(snakePrefab, position, Quaternion.Euler(Vector3.zero));
        snake.Player = player;

        PlayerControl playerControl = snake.gameObject.AddComponent<PlayerControl>();
        playerControl.SetKeys(player.LeftKey, player.RightKey);

        SpawnCollectible();
    }

    private void SpawnCollectible() {
        Vector3 pos = RandomVector3(width, height);
        Instantiate(fruitPrefab, pos, Quaternion.Euler(Vector3.zero));
    }
    public void SnakeEatsFruit(SnakeSegment segment, Fruit fruit) {
        GameObject.Destroy(fruit.gameObject);
        Snake snake = segment.ParentSnake;
        snake.AddSegment();
        SpawnCollectible();
    }
    public void SnakeCrash(SnakeSegment segment1, SnakeSegment segment2) {
        if(segment1.IsHead()) KillSnake(segment1.ParentSnake);
        if(segment2.IsHead()) KillSnake(segment2.ParentSnake);
    }

    private void KillSnake(Snake snake) {
        GameObject.Destroy(snake.gameObject);
    }

    private Vector3 RandomVector3(int maxX, int maxY, int maxZ = 0) {
        return new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), 0);
    }
}
