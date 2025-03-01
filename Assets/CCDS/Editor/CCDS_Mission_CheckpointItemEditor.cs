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
using UnityEditor.SceneManagement;

[CustomEditor(typeof(CCDS_MissionObjective_CheckpointItem))]
public class CCDS_Mission_CheckpointItemEditor : Editor {

    CCDS_MissionObjective_CheckpointItem prop;
    GUISkin skin;
    Color guiColor;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_MissionObjective_CheckpointItem)target;
        serializedObject.Update();
        GUI.skin = skin;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("An object with trigger enabled collider. When the player vehicle triggers it, checkpoint manager will be used to interact with the player. Must have a collider with trigger enabled.", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        EditorGUILayout.Separator();
        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Duplicate")) {

            CCDS_MissionObjective_CheckpointItem dup = Instantiate(prop.gameObject, prop.transform.position, prop.transform.rotation).GetComponent<CCDS_MissionObjective_CheckpointItem>();
            dup.transform.name = prop.transform.name;
            dup.transform.position += dup.transform.forward * 15f;
            dup.transform.SetParent(prop.transform.parent, true);

            Selection.activeGameObject = dup.gameObject;

            EditorUtility.SetDirty(prop);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        }

        if (GUILayout.Button("Mission Manager"))
            Selection.activeGameObject = CCDS_MissionObjectiveManager.Instance.gameObject;

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        //if (!EditorApplication.isPlaying)
        //    prop.GetComponentInParent<CCDS_MissionObjective_Checkpoint>(true).GetAllCheckpoints();

        serializedObject.ApplyModifiedProperties();

    }

}
