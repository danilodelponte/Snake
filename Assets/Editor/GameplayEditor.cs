using System;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(GameplayController))]
public class GameplayEditor : Editor {

     public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        GameplayController gameplay = (GameplayController) target;

        // if(GUILayout.Button("Spawn Snake")){
        //     gameplay.SpawnSnake();
        // };

        // if(GUILayout.Button("Generate Arena")){
        //     gameplay.CreateArena();
        // };
    }
}