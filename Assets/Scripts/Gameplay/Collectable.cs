using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public SpecialPower SpecialPower { get; set; }

    public Collectable Snapshot() {
        bool wasActive = gameObject.activeSelf;
        gameObject.SetActive(false);
        Collectable copy = Instantiate(this, transform.parent);
        copy.SpecialPower = SpecialPower;
        gameObject.SetActive(wasActive);
        return copy;
    }
}
