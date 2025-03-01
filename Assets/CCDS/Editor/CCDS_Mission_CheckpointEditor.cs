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

[CustomEditor(typeof(CCDS_MissionObjective_Checkpoint))]
public class CCDS_Mission_CheckpointEditor : Editor {

    CCDS_MissionObjective_Checkpoint prop;
    GUISkin skin;
    Color guiColor;

    GameObject checkpointToCreate;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

        if (CCDS_Settings.Instance.checkpoint != null)
            checkpointToCreate = CCDS_Settings.Instance.checkpoint.gameObject;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_MissionObjective_Checkpoint)target;
        serializedObject.Update();
        GUI.skin = skin;

        if (!EditorApplication.isPlaying)
            prop.GetAllCheckpoints();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("Create and place checkpoints in the scene. All these checkpoints must be selected. Manager will observe all child checkpoints interaction with the player vehicle.", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        EditorGUILayout.Separator();

        if (prop.checkpoints != null && prop.checkpoints.Count < 1) {

            string error = "One checkpoint needed at least!";
            EditorGUILayout.HelpBox(error, MessageType.Error);

        }

        EditorGUILayout.Separator();

        checkpointToCreate = (GameObject)EditorGUILayout.ObjectField("Checkpoint Prefab To Crate", checkpointToCreate, typeof(GameObject), false);

        EditorGUILayout.Space();

        if (checkpointToCreate == null)
            GUI.enabled = false;

        GUI.color = Color.cyan;

        if (GUILayout.Button("Create New Checkpoint")) {

            Selection.activeGameObject = CreateNewCheckpoint().gameObject;
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

    public CCDS_MissionObjective_CheckpointItem CreateNewCheckpoint() {

        GameObject created = Instantiate(checkpointToCreate, Vector3.zero, Quaternion.identity);
        created.transform.name = checkpointToCreate.transform.name;

        if (created.GetComponent<CCDS_MissionObjective_CheckpointItem>() == null)
            created.AddComponent<CCDS_MissionObjective_CheckpointItem>();

        created.transform.SetParent(prop.transform);
        created.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        prop.AddCheckpoint(created.GetComponent<CCDS_MissionObjective_CheckpointItem>());

        return created.GetComponent<CCDS_MissionObjective_CheckpointItem>();

    }

}
