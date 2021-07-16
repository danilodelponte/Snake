using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SnakeMovementData", menuName = "ScriptableObjects/SnakeMovementData", order = 1)]
public class SnakeMovementData : ScriptableObject {
    
    [SerializeField] private float baseMovingDeltaTime = .07f;
    [SerializeField] private float maxMovingDeltaTime = .15f;
    [SerializeField] private float minMovingDeltaTime = .03f;
    [SerializeField] private float movingDeltaSegmentIncrease = .01f;

    public float BaseMovingDeltaTime { get => baseMovingDeltaTime; }
    public float MaxMovingDeltaTime { get => maxMovingDeltaTime; }
    public float MinMovingDeltaTime { get => minMovingDeltaTime; }
    public float MovingDeltaSegmentIncrease { get => movingDeltaSegmentIncrease; }
}
