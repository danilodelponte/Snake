using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSelectionController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameLabel;
    [SerializeField] private TextMeshProUGUI playerLeftKeyLabel;
    [SerializeField] private TextMeshProUGUI playerRightKeyLabel;

    public void SetPlayer(Player player) {
        playerNameLabel.text = player.Name;
        playerLeftKeyLabel.text = player.LeftKey.ToString();
        playerRightKeyLabel.text = player.RightKey.ToString();
        SetColor(player.Color); 
    }

    private void SetColor(Color color) {
        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers) {
            renderer.material.color = color;
        }
    }
}
