using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialComponent : MonoBehaviour
{
    private SpecialModifier modifier;
    private GameObject decoration;
    public SpecialModifier Modifier { get => modifier; set => SetModifier(value); }
    public SnakeSegment Segment {
        get => gameObject.GetComponent<SnakeSegment>();
    }

    public void SetModifier(SpecialModifier newModifier) {
        if(newModifier == null) {
            RemoveModifierDecoration();
            modifier = null;
        } else {
            modifier = newModifier;
            modifier.SnakeSegment = Segment;
            AddModifierDecoration();
        }
    }

    private void FixedUpdate() {
        if(Modifier!= null) Modifier.FixedUpdate();
    }

    private void AddModifierDecoration() {
        if(modifier.Decoration != null) {
            decoration = Instantiate(modifier.Decoration, transform);
        }
    }

    private void RemoveModifierDecoration() {
        if(decoration != null) GameObject.Destroy(decoration);
    }
}
