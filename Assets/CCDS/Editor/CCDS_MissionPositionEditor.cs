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

[CustomEditor(typeof(CCDS_MissionObjectivePosition))]
public class CCDS_MissionPositionEditor : Editor {

    CCDS_MissionObjectivePosition prop;
    GUISkin skin;
    Color guiColor;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmos(CCDS_MissionObjectivePosition waypoint, GizmoType gizmoType) {

        if (SceneView.lastActiveSceneView && Vector3.Distance(waypoint.transform.position, SceneView.lastActiveSceneView.camera.transform.position) < 100f)
            Handles.Label((waypoint.transform.position + Vector3.up * 1f) + (Vector3.forward * -0f), waypoint.transform.name, EditorStyles.boldLabel);

        Color gizmosColor = Gizmos.color;
        Color targetColor = Color.green;
        targetColor.a = .5f;
        Gizmos.color = targetColor;

        Gizmos.matrix = waypoint.transform.localToWorldMatrix;

        Gizmos.DrawCube(Vector3.zero, new Vector3(1.5f, .5f, 3f));
        Gizmos.DrawLine(new Vector3(.5f, 0f, 1.5f), Vector3.forward * 3f);
        Gizmos.DrawLine(new Vector3(-.5f, 0f, 1.5f), Vector3.forward * 3f);

        Gizmos.color = gizmosColor;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_MissionObjectivePosition)target;
        serializedObject.Update();
        GUI.skin = skin;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("Target locations to transport the player vehicle. All mission locations must be child gameobject of this manager.", MessageType.None);
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
