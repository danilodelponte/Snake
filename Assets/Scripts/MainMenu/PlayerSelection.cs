using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerSelection : MonoBehaviour
{
    public static PlayerSelection Prefab { get => PrefabCache.Load<PlayerSelection>("UI/PlayerSelection"); }
    private static GameObject SnakeSelectionPrefab { get => PrefabCache.Load<GameObject>("UI/SelectionSnake"); }

    [SerializeField] private TextMeshProUGUI playerNameLabel;
    [SerializeField] private TextMeshProUGUI playerLeftKeyLabel;
    [SerializeField] private TextMeshProUGUI playerRightKeyLabel;
    [SerializeField] private GameObject selectionSnake;

    private Player player;
    public Player Player { get => player; set => SetPlayer(value);}

    public void SetPlayer(Player player) {
        this.player = player;
        playerNameLabel.text = player.Name;
        playerLeftKeyLabel.text = player.LeftKey.ToString();
        playerRightKeyLabel.text = player.RightKey.ToString();
        SetColor(player.Color); 
        UpdateSnakeTemplate();
    }

    private void SetColor(Color color) {
        SnakeSegment[] segments = gameObject.GetComponentsInChildren<SnakeSegment>();
        foreach (var segment in segments) {
            segment.GetComponent<MeshRenderer>().material.color = color;
        }
    }

    public void UpdateSnakeTemplate() {
        Vector3 position = selectionSnake.transform.position;
        GameObject.Destroy(selectionSnake);
        selectionSnake = GameObject.Instantiate(SnakeSelectionPrefab, position, Quaternion.identity, transform);

        SpecialModifier[] modifiers = player.SnakeTemplate.Modifiers;
        for(int i = 0; i < modifiers.Length; i++) {
            Transform child = selectionSnake.transform.GetChild(i);
            GameObject.Instantiate(modifiers[i].Decoration, child);
        }
        SetColor(player.Color); 
    }
}
