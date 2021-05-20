using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialComponent : MonoBehaviour
{
    private SpecialPower specialPower;
    public SpecialPower SpecialPower { get => specialPower; set => SetSpecialPower(value); }
    public SnakeSegment Segment {
        get => gameObject.GetComponent<SnakeSegment>();
    }

    public void Start() {
        // gameObject.transform.Find(Power.ToString()).gameObject.SetActive(true);
    }

    public void SetSpecialPower(SpecialPower specialPower) {
        this.specialPower = specialPower;
    }
}
