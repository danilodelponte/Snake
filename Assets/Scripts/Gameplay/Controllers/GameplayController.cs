using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField] public GUIController GUI;

    enum GameState
    {
        RUNNING,
        PAUSED,
        GAMEOVER
    }
    private GameState state = GameState.RUNNING;
    public GameplayMode GameplayMode { get; set; }

    void Start() {
        GameData gameData = GetComponent<GameData>();
        GameplayMode = GameplayMode.Build(GameManager.Instance.GameMode, gameData, this);
        GameplayMode.Start();
    }

    private void FixedUpdate() {
        GameplayMode.FixedUpdate();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(Time.timeScale == 0) ResumeGame();
            else PauseGame();
        }
    }

    public void ResumeGame() {
        if(state != GameState.PAUSED) return;
        state = GameState.RUNNING;
        GUI.HidePausePanel();
        Time.timeScale = 1;
    }

    public void Restart() {
        if(state != GameState.GAMEOVER) return;
        GUI.RemovePlayerLabels();
        GUI.HideGameOverPanel();
        GameplayMode.Restart();
    }

    public void PauseGame() {
        if(state != GameState.RUNNING) return;
        state = GameState.PAUSED;
        Time.timeScale = 0;
        GUI.ShowPausePanel();
    }

    public void GameOver() {
        state = GameState.GAMEOVER;
        GUI.ShowGameOverPanel();
    }

    public void LoadSelectionMenu() {
        Time.timeScale = 1;
        GameManager.Instance.LoadMainMenu();
    }
}
