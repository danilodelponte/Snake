using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialComponent : MonoBehaviour
{
    private SpecialModifier modifier;
    public SpecialModifier Modifier { get => modifier; set => SetModifier(value); }
    public SnakeSegment Segment {
        get => gameObject.GetComponent<SnakeSegment>();
    }

    public void Start() {
        // gameObject.transform.Find(Power.ToString()).gameObject.SetActive(true);
    }

    public void SetModifier(SpecialModifier modifier) {
        this.modifier = modifier;
    }

    private void FixedUpdate() {
        if(Modifier!= null) Modifier.FixedUpdate();
    }
}
