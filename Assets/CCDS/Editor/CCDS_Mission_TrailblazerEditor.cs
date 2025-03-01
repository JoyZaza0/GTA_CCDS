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

[CustomEditor(typeof(CCDS_MissionObjective_Trailblazer))]
public class CCDS_Mission_TrailblazerEditor : Editor {

    CCDS_MissionObjective_Trailblazer prop;
    GUISkin skin;
    Color guiColor;

    GameObject trailblazerPrefabToCreate;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

        if (CCDS_Settings.Instance.trailBlazerObstacle != null)
            trailblazerPrefabToCreate = CCDS_Settings.Instance.trailBlazerObstacle.gameObject;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_MissionObjective_Trailblazer)target;
        serializedObject.Update();
        GUI.skin = skin;

        if (!EditorApplication.isPlaying)
            prop.GetAllTrailblazerObstacles();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("Create and place trailblazer obstacles in the scene. All these trailblazer obstacles must be selected. Manager will observe all child trailblazer obstacles interaction with the player vehicle.", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        EditorGUILayout.Separator();

        if (prop.obstacles != null && prop.obstacles.Count < 1) {

            string error = "One trailblazer obstacle needed at least!";
            EditorGUILayout.HelpBox(error, MessageType.Error);

        }

        EditorGUILayout.Separator();

        trailblazerPrefabToCreate = (GameObject)EditorGUILayout.ObjectField("Trailblazer Obstacle Prefab To Crate", trailblazerPrefabToCreate, typeof(GameObject), false);

        EditorGUILayout.Space();

        if (trailblazerPrefabToCreate == null)
            GUI.enabled = false;

        GUI.color = Color.cyan;

        if (GUILayout.Button("Create New Trailblazer Obstacle")) {

            Selection.activeGameObject = CreateNewTrailblazerObstacle().gameObject;
            Selection.activeGameObject.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
            SceneView.FrameLastActiveSceneView();
            SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Place the item on the scene!"), 3);

            EditorUtility.SetDirty(prop);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        }

        GUI.color = guiColor;

        GUI.enabled = true;

        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();

        if (GUILayout.Button("Mission Manager"))
            Selection.activeGameObject = CCDS_MissionObjectiveManager.Instance.gameObject;

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        serializedObject.ApplyModifiedProperties();

    }

    public CCDS_MissionObjective_TrailblazerItem CreateNewTrailblazerObstacle() {

        GameObject created = Instantiate(trailblazerPrefabToCreate, Vector3.zero, Quaternion.identity);
        created.transform.name = trailblazerPrefabToCreate.transform.name;

        if (created.GetComponent<CCDS_MissionObjective_TrailblazerItem>() == null)
            created.AddComponent<CCDS_MissionObjective_TrailblazerItem>();

        created.transform.SetParent(prop.transform);
        created.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        prop.AddTrailblazerObstacle(created.GetComponent<CCDS_MissionObjective_TrailblazerItem>());

        return created.GetComponent<CCDS_MissionObjective_TrailblazerItem>();

    }

}
