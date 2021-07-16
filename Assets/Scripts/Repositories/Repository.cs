using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repository<T> : MonoBehaviour
    where T : MonoBehaviour {

    private static List<T> _objects = new List<T>();

    public static List<T> All() {
        _objects.RemoveAll(obj => obj == null);
        return _objects;
    }

    public static List<T> FindAll(System.Predicate<T> match) {
        _objects.RemoveAll(obj => obj == null);
        return _objects.FindAll(match);
    }

     public static void DestroyAll() {
        foreach (var obj in _objects) {
            if(obj != null) Destroy(obj.gameObject);
        }

        _objects = new List<T>();
    }

    private void Start() {
        _objects.Add(gameObject.GetComponent<T>());
    }

    private void OnDestroy() {
        _objects.Remove(gameObject.GetComponent<T>());
    }
}
