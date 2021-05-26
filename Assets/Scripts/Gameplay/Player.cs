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
    public SpecialModifier[] SnakeTemplate { get; set; }

    public Player(string name, KeyCode leftKey, KeyCode rightKey, Color color){
        Name = name;
        LeftKey = leftKey;
        RightKey = rightKey;
        Color = color;
        Score = 0;
    }

    // snake type..
}
