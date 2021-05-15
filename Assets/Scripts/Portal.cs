using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Portal otherEnd;
    [SerializeField] private Vector3 transportFilter = Vector3.one;
    [SerializeField] private Vector3 offSet = Vector3.zero;
    
    public void Teleport(SnakeSegment segment) {
        Vector3 positionDiff = otherEnd.transform.position - segment.transform.position;
        positionDiff = Vector3.Scale(positionDiff, transportFilter);
        segment.transform.position += positionDiff + offSet;
    }
}
