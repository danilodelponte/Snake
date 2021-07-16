using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BombRepository {

    public static Bomb Prefab { get => PrefabCache.Load<Bomb>("Bomb"); }

    public static Bomb Build(Vector3 position, GameplayMode gameplayMode) {
        Bomb bomb = GameObject.Instantiate(Prefab, position, Quaternion.identity);
        bomb.GameplayMode = gameplayMode;
        return bomb;
    }
}
