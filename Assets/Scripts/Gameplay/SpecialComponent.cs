using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialComponent : MonoBehaviour
{
    public SpecialModifier Modifier { get; set; }

    private void FixedUpdate() {
        if(Modifier != null) Modifier.FixedUpdate();
    }
}
