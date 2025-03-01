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

[CustomEditor(typeof(CCDS_MissionObjective_TrailblazerItem))]
public class CCDS_Mission_TrailblazerItemEditor : Editor {

    CCDS_MissionObjective_TrailblazerItem prop;
    GUISkin skin;
    Color guiColor;

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmos(CCDS_MissionObjective_TrailblazerItem waypoint, GizmoType gizmoType) {

        CCDS_MissionObjective_Trailblazer manager = waypoint.GetComponentInParent<CCDS_MissionObjective_Trailblazer>(true);
        manager.GetAllTrailblazerObstacles();

        for (int i = 0; i < manager.obstacles.Count; i++) {

            if (manager.obstacles[i] != null && i < (manager.obstacles.Count - 1))
                Gizmos.DrawLine(manager.obstacles[i].transform.position, manager.obstacles[i + 1].transform.position);

        }

    }

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_MissionObjective_TrailblazerItem)target;
        serializedObject.Update();
        GUI.skin = skin;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("An obstacle with trigger enabled collider. When the player vehicle triggers it, trailblazer manager will be used to interact with the player. Must have rigidbody and a collider with trigger enabled.", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        EditorGUILayout.Separator();
        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Duplicate")) {

            CCDS_MissionObjective_TrailblazerItem dup = Instantiate(prop.gameObject, prop.transform.position, prop.transform.rotation).GetComponent<CCDS_MissionObjective_TrailblazerItem>();
            dup.transform.name = prop.transform.name;
            dup.transform.position += Vector3.forward * 2f;
            dup.transform.SetParent(prop.transform.parent, true);

            Selection.activeGameObject = dup.gameObject;
            EditorUtility.SetDirty(prop);

        }

        if (GUILayout.Button("Mission Manager"))
            Selection.activeGameObject = CCDS_MissionObjectiveManager.Instance.gameObject;

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        if (!EditorApplication.isPlaying)
            prop.GetComponentInParent<CCDS_MissionObjective_Trailblazer>(true).GetAllTrailblazerObstacles();

        serializedObject.ApplyModifiedProperties();

    }

}
