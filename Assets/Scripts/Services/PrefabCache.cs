using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefabCache
{
    private static Dictionary<string, Object> cache = new Dictionary<string, Object>();

    public static T Load<T>(string name) where T : Object {
        if(cache.ContainsKey(name)) return (T) cache[name];
        T prefab = Resources.Load<T>($"Prefabs/{name}");
        cache.Add(name, prefab);
        return prefab;
    }
}
