using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {
    private GameObject PausePanel { get => transform.Find("PausePanel").gameObject; }
    private GameObject GameOverPanel { get => transform.Find("GameOverPanel").gameObject; }

    public void ShowPausePanel() {
        PausePanel.SetActive(true);
    }

    public void HidePausePanel() {
        PausePanel.SetActive(false);
    }

    public void ShowGameOverPanel() {
        GameOverPanel.SetActive(true);
        Transform playerScores = GameOverPanel.transform.Find("PlayersScores");
        PlayerScoreLabel[] playerLabels = transform.GetComponentsInChildren<PlayerScoreLabel>();
        Vector3 offset = new Vector3(10, 10);
        foreach (var label in playerLabels) {
            label.gameObject.transform.SetParent(playerScores, false);
            label.transform.position += offset;
        }
    }

    public void HideGameOverPanel() {
        GameOverPanel.SetActive(false);
    }

    public void RemovePlayerLabels() {
        PlayerScoreLabel[] playerLabels = transform.GetComponentsInChildren<PlayerScoreLabel>();
        foreach (var label in playerLabels) {
            GameObject.Destroy(label.gameObject);
        }
    }

    public void AddPlayerLabel(Player player) {
        RectTransform lastTransform = lastLabelTransform();
        PlayerScoreLabel scoreLabel = Instantiate(PlayerScoreLabel.Prefab, transform);
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
        return (RectTransform) scoreLabels[lastIndex].transform;
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
