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
            Vector3 position = RandomVector3(width, height);
            SpawnSnake(player, position);
            SpawnCollectible();
        }
    }

    private void InitSingleton() {
        if (Singleton != null) {
            Destroy(gameObject);
            return;
        }
        Singleton = this;
    }

    private void SpawnSnake(Player player, Vector3 position) {
        var snake = Instantiate(snakePrefab, position, Quaternion.Euler(Vector3.zero));
        PlayerControl playerControl = snake.gameObject.AddComponent<PlayerControl>();
        playerControl.LeftKey = player.LeftKey;
        playerControl.RightKey = player.RightKey;
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

    private Vector3 RandomVector3(int maxX, int maxY, int maxZ = 0) {
        return new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), 0);
    }
}
