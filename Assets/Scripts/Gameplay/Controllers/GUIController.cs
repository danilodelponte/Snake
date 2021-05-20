using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    [SerializeField] private PlayerScoreLabel scoreLabelPrefab;
    private GameObject PausePanel { get => transform.Find("PausePanel").gameObject; }

    public void ShowPausePanel() {
        PausePanel.SetActive(true);
    }

    public void HidePausePanel() {
        PausePanel.SetActive(false);
    }

    public void AddPlayerLabel(Player player) {
        RectTransform lastTransform = lastLabelTransform();
        PlayerScoreLabel scoreLabel = Instantiate(scoreLabelPrefab, transform);
        float offsetY = ((RectTransform) scoreLabel.transform).rect.height * ScoreLabels().Length;
        scoreLabel.transform.localPosition -= new Vector3(0, offsetY, 0);
        scoreLabel.Player = player;
        scoreLabel.SetColor(player.Color);
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
