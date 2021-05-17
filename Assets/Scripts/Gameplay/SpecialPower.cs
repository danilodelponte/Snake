using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPower : MonoBehaviour
{
    public SnakeSegment Segment {
        get => gameObject.GetComponent<SnakeSegment>();
    }

    public virtual void Start() {
        // gameObject.transform.Find(SpecialPower.ToString()).gameObject.SetActive(true);
    }

    public virtual float SpecialMovement(float maxDeltaTime){ return maxDeltaTime; }
    public virtual bool SpecialCollision(SnakeSegment segment, Collider other) { return false; }
}
