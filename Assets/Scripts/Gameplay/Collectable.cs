using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    public Arena arena;
    public System.Type PowerType { get; set; }
    public int Score { get => score; }

    private int score = 1;

    public Collectable Snapshot() {
        bool wasActive = gameObject.activeSelf;
        gameObject.SetActive(false);
        Collectable copy = Instantiate(this, transform.parent);
        copy.PowerType = PowerType;
        copy.arena = arena;
        gameObject.SetActive(wasActive);
        return copy;
    }

    private void SetNodePath(){
        arena.SetNode(transform.position, PathNodeType.COLLECTABLE);
    }

    private void FreeNodePath(){
        arena.SetNode(transform.position, PathNodeType.FREE);
    }

    private void OnEnable() {
        arena = GameObject.Find("Arena").GetComponent<Arena>();
        SetNodePath();
    }

    private void OnDisable() {
        FreeNodePath();
    }

    private void OnDestroy() {
        FreeNodePath();
    }
}
