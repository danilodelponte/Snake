using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapshot
{
    public Snake[] Snakes { get; set; }
    public Collectable[] Collectables { get; set; }

    public Snapshot(Snake[] snakes, Collectable[] collectables) {
        Snakes = snakes;
        Collectables = collectables;
    }
}
