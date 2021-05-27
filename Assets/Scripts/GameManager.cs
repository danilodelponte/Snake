using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    static class Scenes
    {
        public const string Gameplay = "Gameplay";
        public const string MainMenu = "MainMenu";
    }

    private Player[] players;
    public Player[] Players { get => players; }
    public GameplayMode GameMode = new MultiplayerMode();

    public void LoadGameplay(Player[] players) {
        this.players = players;
        SceneManager.LoadSceneAsync(Scenes.Gameplay);
    }
    
    public void LoadMainMenu() {
        SceneManager.LoadSceneAsync(Scenes.MainMenu);
    }
}
