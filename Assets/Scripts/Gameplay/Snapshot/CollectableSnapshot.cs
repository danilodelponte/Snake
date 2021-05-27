using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSnapshot {

    private static GameObject collectablePrefab = Resources.Load("Prefabs/Collectable") as GameObject;

    public SpecialModifier Modifier { get; set; }
    public int Score { get; set; }
    public Vector3 Position { get; set; }

    public CollectableSnapshot(Collectable collectable) {
        Modifier = collectable.Modifier;
        Score = collectable.Score;
        Position = collectable.transform.position;
    }

    public Collectable Load() {
        GameObject go = (GameObject) GameObject.Instantiate(collectablePrefab, Position, Quaternion.identity);
        Collectable collectable = (Collectable) go.GetComponent<Collectable>();
        collectable.Modifier = Modifier;
        collectable.Score = Score;
        return collectable;
    }
}
