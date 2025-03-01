//----------------------------------------------
//        City Car Driving Simulator
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(CCDS_MissionObjectivePositionsManager))]
public class CCDS_MissionPositionsManagerEditor : Editor {

    CCDS_MissionObjectivePositionsManager prop;
    GUISkin skin;
    Color guiColor;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_MissionObjectivePositionsManager)target;
        serializedObject.Update();
        GUI.skin = skin;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("CCDS_MissionPositionsManager is responsible for storing the target transforms for the any mission. Player vehicle will be using these transforms to be transported when the mission starts. All markers have been using the mission class, and this mission class has 'Transport To Target Location' transform target. In order to use any transforms, be sure to select the target location. ", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        GUI.color = Color.cyan;

        EditorGUILayout.Space();

        if (GUILayout.Button("Create New Position")) {

            Selection.activeGameObject = CCDS_MissionObjectivePositionsManager.Instance.CreateNewPosition().gameObject;
            Selection.activeGameObject.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
            SceneView.FrameLastActiveSceneView();

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorUtility.SetDirty(prop.gameObject);

        }

        GUI.color = guiColor;

        EditorGUILayout.Separator();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Separator();

        if (!EditorApplication.isPlaying)
            prop.GetAllPositions();

        prop.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        serializedObject.ApplyModifiedProperties();

    }

}
