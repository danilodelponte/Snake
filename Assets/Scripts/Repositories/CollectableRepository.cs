using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableRepository : Repository<Collectable>
{
    public static Collectable Prefab { get => PrefabCache.Load<Collectable>("Collectable"); }

    public static Collectable Build(Vector3 position, Quaternion rotation, SpecialModifier modifier) {
        Collectable collectable = GameObject.Instantiate(Prefab, position, rotation);
        collectable.gameObject.AddComponent<CollectableRepository>();
        collectable.Modifier = modifier;
        return collectable;
    }

    public static void Destroy(Collectable collectable) {
        GameObject.Destroy(collectable.gameObject);
    }

}

