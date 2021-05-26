using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Transform backgroundPanel;

    [SerializeField] TextMeshProUGUI conflictedKeys;
    [SerializeField] TextMeshProUGUI pressLeftToAdd;

    [SerializeField] Component addingPlayerOptions;
    [SerializeField] TextMeshProUGUI holdTwoToAdd;
    [SerializeField] TextMeshProUGUI holdToAdd;

    [SerializeField] Component editingPlayerOptions;
    [SerializeField] TextMeshProUGUI holdBothToChange;
    [SerializeField] TextMeshProUGUI holdToChange;

    private enum State { Conflicted, Editing, Adding, Waiting, Ready }
    private State state = State.Waiting;    

    private HashSet<KeyCode> pressedKeys = new HashSet<KeyCode>();
    private float twoKeysHeldTimer = 0;

    private List<Player> players = new List<Player>();
    private Dictionary<KeyCode, Player> keyPlayerMapping = new Dictionary<KeyCode, Player>();

    private SnakeTemplate[] templates;

    public void StartGame() {
        GameManager.Instance.LoadGameplay(players.ToArray());
    }

    private void SetActive(Component component, bool active) {
        component.gameObject.SetActive(active);
    }

    private void Start() {
        GenerateTemplates();
        UpdateState();
        UpdateGUI();
    }

    private void GenerateTemplates() {
        templates = SnakeTemplate.GetTemplates();
    }

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

    private void KeyUp(KeyCode keyCode) {
        pressedKeys.Remove(keyCode);
        UpdateState();
        UpdateGUI();
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

    private void Update() {
        if(pressedKeys.Count == 2) twoKeysHeldTimer += Time.deltaTime;
        if(state == State.Adding && twoKeysHeldTimer > 1) {
            AddPlayer(pressedKeys.ToArray());
            UpdateState();
            UpdateGUI();
        }
        else if(state == State.Editing && twoKeysHeldTimer > 1) {
            twoKeysHeldTimer = 0;
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

    private void UpdateGUI() {
        SetActive(pressLeftToAdd, state == State.Waiting);
        SetActive(conflictedKeys, state == State.Conflicted);
        UpdateAddingPlayerOptions();
        UpdateEditingPlayerOptions();
    }

    private void UpdateAddingPlayerOptions() {
        SetActive(addingPlayerOptions,  state == State.Adding);
        SetActive(holdTwoToAdd,         pressedKeys.Count == 1);
        SetActive(holdToAdd,            pressedKeys.Count == 2);
    }

    private void UpdateEditingPlayerOptions() {
        SetActive(editingPlayerOptions, state == State.Editing);
        SetActive(holdBothToChange,     pressedKeys.Count == 1);
        SetActive(holdToChange,         pressedKeys.Count == 2);
    }

    private void ChangePlayerType(Player player) {
        int playerTemplateIndex = Array.IndexOf(templates, player.SnakeTemplate);
        playerTemplateIndex += 1;
        playerTemplateIndex %= templates.Length;
        player.SnakeTemplate = templates[playerTemplateIndex];

        GetSelectionFor(player).UpdateSnakeTemplate();
    }

    private PlayerSelection GetSelectionFor(Player player) {
        GameObject[] selectionObjects = GameObject.FindGameObjectsWithTag("PlayerSelection");
        foreach (var selectionObject in selectionObjects) {
            PlayerSelection selection = selectionObject.GetComponent<PlayerSelection>();
            if(selection.Player == player) return selection;
        }
        return null;
    }

    private void AddPlayer(KeyCode[] playerKeys) {
        KeyCode leftKey = playerKeys[0];
        KeyCode rightKey = playerKeys[1];
        Player player = new Player(leftKey, rightKey);
        player.SnakeTemplate = templates[0];
        players.Add(player);

        keyPlayerMapping.Add(leftKey, player);
        keyPlayerMapping.Add(rightKey, player);

        var playerSelection = Instantiate(PlayerSelection.Prefab, backgroundPanel);
        playerSelection.SetPlayer(player);
        float offsetX = ((RectTransform) playerSelection.transform).rect.width * (players.Count()-1);
        playerSelection.transform.localPosition += new Vector3(offsetX, 0, 0);
    }
}
