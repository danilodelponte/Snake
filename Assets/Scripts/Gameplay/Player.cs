using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

    public string Name { get; }
    public KeyCode LeftKey { get; }
    public KeyCode RightKey { get; }
    public int SnakeType { get; set; }
    public int Score { get; set; }

    public Player(string name, KeyCode leftKey, KeyCode rightKey){
        Name = name;
        LeftKey = leftKey;
        RightKey = rightKey;
        Score = 0;
    }

    // snake type..
}
