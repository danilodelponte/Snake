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
        if(modifier != null) modifier.Activate();
        // gameObject.transform.Find(Power.ToString()).gameObject.SetActive(true);
    }

    public void SetModifier(SpecialModifier modifier) {
        this.modifier = modifier;
        if(modifier != null) {
            Transform modifierDecoration = transform.Find(modifier.ToString());
            if(modifierDecoration != null) { modifierDecoration.gameObject.SetActive(true); }
        }
    }

    private void FixedUpdate() {
        if(Modifier!= null) Modifier.FixedUpdate();
    }
}
