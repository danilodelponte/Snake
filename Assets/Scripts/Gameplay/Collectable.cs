using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public static Collectable Prefab { get => PrefabCache.Load<Collectable>("Collectable"); }

    public Arena arena;
    public SpecialModifier Modifier { get; set; }
    public int Score { get => score; set => score = value; }

    private int score = 1;
}
