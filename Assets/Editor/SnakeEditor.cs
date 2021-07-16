using System;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(Snake))]
public class SnakeEditor : Editor
{

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        Snake snake = (Snake) target;

        // if(GUILayout.Button("Move right")){
        //     snake.Move(new Vector3(1,0,0));
        // } 
        // else if(GUILayout.Button("Move up")){
        //     snake.Move(new Vector3(0,1,0));
        // }
        // else if(GUILayout.Button("Move down")){
        //     snake.Move(new Vector3(0,-1,0));
        // }
        // else if(GUILayout.Button("Move left")){
        //     snake.Move(new Vector3(-1,0,0));
        // }
    }
}