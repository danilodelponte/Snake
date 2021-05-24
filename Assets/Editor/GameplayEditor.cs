using System;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(GameplayController))]
public class GameplayEditor : Editor {

     public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if(GUILayout.Button("Spawn Snake")){
            GameplayController controller = (GameplayController) target;
            controller.SpawnSnake();
        };
    }
}