using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public Arena arena;
    public SpecialModifier Modifier { get; set; }
    public int Score { get => score; set => score = value; }

    private int score = 1;

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
