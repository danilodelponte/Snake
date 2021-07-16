using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [SerializeField] private SnakeMovementData playerMovementData;
    [SerializeField] private SnakeMovementData aiMovementData;

    public SnakeMovementData PlayerMovementData { get => playerMovementData; }
    public SnakeMovementData AiMovementData { get => aiMovementData; }
}
