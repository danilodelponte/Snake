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
        players.Add(new Player());

        foreach (Player player in players) {
            Vector3 position = Vector3.zero;
            PlaceSnake(player, position);
            PlaceCollectible();
        }
    }

    private void InitSingleton() {
        if (Singleton != null) {
            Destroy(gameObject);
            return;
        }
        Singleton = this;
    }

    private void PlaceSnake(Player player, Vector3 position) {
        var snake = Instantiate(snakePrefab, position, Quaternion.Euler(Vector3.zero));
        snake.gameObject.AddComponent(typeof(PlayerControl));
    }

    private void PlaceCollectible() {
        Vector3 pos = new Vector3(Random.Range(0, width), Random.Range(0, height), 0);
        Instantiate(fruitPrefab, pos, Quaternion.Euler(Vector3.zero));
    }
    public void SnakeEatsFruit(SnakeSegment segment, Fruit fruit) {
        GameObject.Destroy(fruit.gameObject);
        Snake snake = segment.ParentSnake;
        snake.AddSegment();
        PlaceCollectible();
    }
}
