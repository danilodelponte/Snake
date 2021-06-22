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
        if(modifier != null) {
            RemoveModifierDecoration();
            modifier.SnakeSegment = null;
        }
        modifier = newModifier;
        
        if(modifier != null) {
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
