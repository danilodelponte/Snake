using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    public System.Type PowerType { get; set; }
    public int Score { get => score; }

    private int score = 1;

    public Collectable Snapshot() {
        bool wasActive = gameObject.activeSelf;
        gameObject.SetActive(false);
        Collectable copy = Instantiate(this, transform.parent);
        copy.PowerType = PowerType;
        gameObject.SetActive(wasActive);
        return copy;
    }
}
