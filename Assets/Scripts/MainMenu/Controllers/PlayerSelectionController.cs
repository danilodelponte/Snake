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
    }
}
