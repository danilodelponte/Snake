using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScoreLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;

    public Player Player { get; set; }
    
    public void UpdateScore() {
        string text = Player.Name + " " + Player.Score;
        label.SetText(text);
    }

    public void SetColor(Color color) { 
        label.color = color;
    }
}
