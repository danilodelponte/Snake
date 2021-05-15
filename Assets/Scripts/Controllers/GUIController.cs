using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    [SerializeField] private PlayerScoreLabel scoreLabelPrefab;   

    public void AddPlayerLabel(Player player) {
        RectTransform lastTransform = lastLabelTransform();
        PlayerScoreLabel scoreLabel = Instantiate(scoreLabelPrefab, transform);
        if(lastTransform != null) scoreLabel.transform.position -= new Vector3(0, lastTransform.rect.height, 0);
        scoreLabel.Player = player;
        scoreLabel.UpdateScore();
    }

    public RectTransform lastLabelTransform() {
        PlayerScoreLabel[] scoreLabels = ScoreLabels();
        if(scoreLabels.Length == 0) return null;

        int lastIndex = scoreLabels.Length - 1;
        return scoreLabels[lastIndex].transform as RectTransform;
    }

    private PlayerScoreLabel[] ScoreLabels() {
        return transform.GetComponentsInChildren<PlayerScoreLabel>();
    }

    public void UpdatePlayerScore (Player player) {
        foreach (var scoreLabel in ScoreLabels()) {
            if(scoreLabel.Player == player) {
                scoreLabel.UpdateScore();
                return;
            }
        }
    }
}
