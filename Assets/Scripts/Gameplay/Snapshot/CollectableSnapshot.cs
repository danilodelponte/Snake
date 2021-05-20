using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSnapshot {

    private static GameObject collectablePrefab = Resources.Load("Prefabs/Collectable") as GameObject;

    public SpecialPower SpecialPower { get; set; }
    public int Score { get; set; }
    public Vector3 Position { get; set; }

    public CollectableSnapshot(Collectable collectable) {
        SpecialPower = collectable.SpecialPower;
        Score = collectable.Score;
    }

    public Collectable Load() {
        GameObject go = (GameObject) GameObject.Instantiate(collectablePrefab, Position, Quaternion.Euler(0,0,0));
        Collectable collectable = (Collectable) go.GetComponent<Collectable>();
        collectable.SpecialPower = SpecialPower;
        collectable.Score = Score;
        return collectable;
    }
}
