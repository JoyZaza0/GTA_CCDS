//----------------------------------------------
//        City Car Driving Simulator
//
// Copyright � 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(CCDS_GameplayManager))]
public class CCDS_GameplayManagerEditor : Editor {

    CCDS_GameplayManager prop;
    GUISkin skin;
    Color guiColor;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_GameplayManager)target;
        serializedObject.Update();
        GUI.skin = skin;

        EditorGUILayout.HelpBox("Manages and observes all managers in the scene. Green buttons are meaning that manager is active in the scene and you can select it. Red buttons are meaning that manager doesn't exist in the scene, you can create it by simply clicking on it.\n\nNew managers can't be created at runtime.", MessageType.None);

        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUI.indentLevel++;

        GUI.enabled = true;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnPoint"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("countdownToStart"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isDay"));
	    //EditorGUILayout.PropertyField(serializedObject.FindProperty("UsePlayerCharacer"));

        EditorGUILayout.Space();

        EditorGUI.indentLevel--;
        EditorGUILayout.HelpBox("Below fields are not editable during gameplay, they will be overridden. But they're public, you can still access them.", MessageType.Info);
        EditorGUI.indentLevel++;

        GUI.enabled = false;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("player"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gameState"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lastSelectedVehicleIndex"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("timeLimit"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("timeSinceGameStart"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("currentMission"));
        GUI.enabled = true;

        EditorGUI.indentLevel--;

        EditorGUILayout.EndVertical();

        prop.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        serializedObject.ApplyModifiedProperties();

    }

}
