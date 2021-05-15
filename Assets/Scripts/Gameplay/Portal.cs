using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal OtherEnd { get; set; }
    public Vector3 TransportFilter { get; set; }
    public Vector3 OffSet { get; set; }
    
    public void Teleport(SnakeSegment segment) {
        Vector3 positionDiff = OtherEnd.transform.position - segment.transform.position;
        positionDiff = Vector3.Scale(positionDiff, TransportFilter);
        segment.transform.position += positionDiff + OffSet;
    }
}
