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

[CustomEditor(typeof(CCDS_SpawnPoint))]
public class CCDS_SpawnPointEditor : Editor {

    CCDS_SpawnPoint prop;
    GUISkin skin;
    Color guiColor;

    GameObject checkpointToCreate;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmos(CCDS_SpawnPoint spawnPoint, GizmoType gizmoType) {

        Color gizmosColor = Gizmos.color;
	    Color targetColor = Color.blue;
        targetColor.a = .5f;
        Gizmos.color = targetColor;

        if (SceneView.lastActiveSceneView && Vector3.Distance(spawnPoint.transform.position, SceneView.lastActiveSceneView.camera.transform.position) < 100f)
            Handles.Label((spawnPoint.transform.position + Vector3.up * 1f) + (Vector3.forward * -0f), spawnPoint.transform.name, EditorStyles.boldLabel);

        Gizmos.matrix = spawnPoint.transform.localToWorldMatrix;

        Gizmos.DrawCube(Vector3.zero, new Vector3(1.5f, .5f, 3f));
        Gizmos.DrawLine(new Vector3(.5f, 0f, 1.5f), Vector3.forward * 3f);
        Gizmos.DrawLine(new Vector3(-.5f, 0f, 1.5f), Vector3.forward * 3f);

        Gizmos.color = gizmosColor;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_SpawnPoint)target;
        serializedObject.Update();
        GUI.skin = skin;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("Spawn position when the game starts.", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        EditorGUILayout.EndVertical();

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        serializedObject.ApplyModifiedProperties();

    }

}
