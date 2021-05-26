using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSelection : MonoBehaviour
{
    public static PlayerSelection Prefab { get => PrefabCache.Load<PlayerSelection>("PlayerSelection"); }

    [SerializeField] private TextMeshProUGUI playerNameLabel;
    [SerializeField] private TextMeshProUGUI playerLeftKeyLabel;
    [SerializeField] private TextMeshProUGUI playerRightKeyLabel;

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
        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers) {
            renderer.material.color = color;
        }
    }

    public void UpdateSnakeTemplate() {
        Transform selectionSnake = transform.Find("SelectionSnake");
        foreach (Transform child in selectionSnake) {
            GameObject.Destroy(child.gameObject);
        }

        SpecialModifier[] modifiers = player.SnakeTemplate.Modifiers;
        int yOffset = -30;

        foreach (var modifier in modifiers) {
            SnakeSegment segment = Instantiate<SnakeSegment>(SnakeSegment.Prefab, selectionSnake);
            segment.GetComponent<Rigidbody>().detectCollisions = false;
            segment.GetComponent<SpecialComponent>().enabled = false;
            segment.transform.localScale = Vector3.one * 30;
            segment.transform.localPosition += new Vector3(0, yOffset, 0);
            segment.Modifier = modifier;
            yOffset += 30;
        }
        SetColor(player.Color); 
    }
}
