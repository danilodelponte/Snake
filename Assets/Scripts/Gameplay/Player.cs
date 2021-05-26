using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

    public string Name { get; }
    public KeyCode LeftKey { get; }
    public KeyCode RightKey { get; }
    public int SnakeType { get; set; }
    public int Score { get; set; }
    public Color Color { get; set; }
    public SnakeTemplate SnakeTemplate { get; set; }

    public Player(KeyCode leftKey, KeyCode rightKey){
        Name = $"Player {leftKey}{rightKey}";
        LeftKey = leftKey;
        RightKey = rightKey;
        Color = UnityEngine.Random.ColorHSV(0,1,.97f,1,.97f,1);
        Score = 0;
    }
}
