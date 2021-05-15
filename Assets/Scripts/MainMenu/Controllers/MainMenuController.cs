using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private Transform backgroundPanel;
    [SerializeField] private PlayerSelectionController playerSelectionPrefab;

    [SerializeField] TextMeshProUGUI conflictedKeys;

    [SerializeField] Component addingPlayerOptions;
    [SerializeField] TextMeshProUGUI holdTwoToAdd;
    [SerializeField] TextMeshProUGUI holdToAdd;
    [SerializeField] TextMeshProUGUI releaseToAddPlayer;

    [SerializeField] Component editingPlayerOptions;
    [SerializeField] TextMeshProUGUI holdBothToChange;
    [SerializeField] TextMeshProUGUI holdToChange;
    [SerializeField] TextMeshProUGUI releaseToSelectType;

    private enum State { Conflicted, Editing, Adding, Waiting, Ready }
    private State state = State.Waiting;    

    private HashSet<KeyCode> pressedKeys = new HashSet<KeyCode>();
    private float twoKeysHeldTimer = 0;

    private List<Player> players = new List<Player>();
    private Dictionary<KeyCode, Player> keyPlayerMapping = new Dictionary<KeyCode, Player>();

    private void OnGUI() {
        Event e = Event.current;
        KeyCode keyCode = e.keyCode;
        if(!e.isKey || !AllowedKeyCodes.IsAllowed(keyCode)) return;

        if(e.type == EventType.KeyDown) KeyDown(keyCode);
        if(e.type == EventType.KeyUp) KeyUp(keyCode);
    }

    private void KeyDown(KeyCode keyCode) {
        if(pressedKeys.Contains(keyCode)) return;

        pressedKeys.Add(keyCode);
        twoKeysHeldTimer = 0;
        UpdateState();
        UpdateGUI();
    }

    private void FixedUpdate() {
        if(pressedKeys.Count == 2) twoKeysHeldTimer += Time.deltaTime;
        if(state == State.Adding && twoKeysHeldTimer > 1) {
            AddPlayer(pressedKeys.ToArray());
            UpdateState();
            UpdateGUI();
        }
        else if(state == State.Editing && twoKeysHeldTimer % 3 == 0) {
            ChangePlayerType(FirstKeyPlayer());
            UpdateState();
            UpdateGUI();
        }
    }

    private void UpdateState() {
        if(pressedKeys.Count == 0) { state = State.Waiting; }
        else if(pressedKeys.Count > 2) { state = State.Conflicted; }
        else if(pressedKeys.Count == 1) {
            if(FirstKeyPlayer() != null) { state = State.Editing; }
            else { state = State.Adding; }
        }
        else if(pressedKeys.Count == 2) {
            if(FirstKeyPlayer() != SecondKeyPlayer()) { state = State.Conflicted; }
            else if(FirstKeyPlayer() != null) {  state = State.Editing; }
            else { state = State.Adding; }
        }
    }

    private KeyCode FirstKey() {
        return pressedKeys.ToArray()[0];
    }

    private KeyCode SecondKey() {
        return pressedKeys.ToArray()[1];
    }

    private Player FirstKeyPlayer() {
        if(!keyPlayerMapping.ContainsKey(FirstKey())) return null;

        return keyPlayerMapping[FirstKey()];
    }

    private Player SecondKeyPlayer() {
        if(!keyPlayerMapping.ContainsKey(SecondKey())) return null;

        return keyPlayerMapping[SecondKey()];
    }

    private void UpdateGUI() {
        SetActive(conflictedKeys, state == State.Conflicted);
        UpdateAddingPlayerOptions();
        UpdateEditingPlayerOptions();
    }

    private void UpdateAddingPlayerOptions() {
        SetActive(addingPlayerOptions,  state == State.Adding);
        SetActive(holdTwoToAdd,         pressedKeys.Count == 1);
        SetActive(holdToAdd,            pressedKeys.Count == 2 && twoKeysHeldTimer < 1);
        SetActive(releaseToAddPlayer,   twoKeysHeldTimer > 1);
    }

    private void UpdateEditingPlayerOptions() {
        SetActive(editingPlayerOptions, state == State.Editing);
        SetActive(holdBothToChange,     pressedKeys.Count == 1);
        SetActive(holdToChange,         pressedKeys.Count == 2 && twoKeysHeldTimer < 3);
        SetActive(releaseToSelectType,  twoKeysHeldTimer > 3);
    }

    private void KeyUp(KeyCode keyCode) {
        pressedKeys.Remove(keyCode);
        UpdateState();
        UpdateGUI();
    }

    private void ChangePlayerType(Player player) {

    }

    private void AddPlayer(KeyCode[] playerKeys) {
        int playersCount = players.Count();
        string name = $"Player { playersCount + 1 }";
        Player player = new Player(name, playerKeys[0], playerKeys[1]);
        players.Add(player);
        keyPlayerMapping.Add(playerKeys[0], player);
        keyPlayerMapping.Add(playerKeys[1], player);

        var playerSelection = Instantiate(playerSelectionPrefab, backgroundPanel);
        float offsetX = ((RectTransform) playerSelection.transform).rect.width * playersCount;
        playerSelection.transform.localPosition += new Vector3(offsetX, 0, 0);
        playerSelection.SetPlayer(player);
    }

    private void SetActive(Component component, bool active) {
        component.gameObject.SetActive(active);
    }

    public void StartGame() {
        GameManager.Instance.LoadGameplay(players.ToArray());
    }
}
