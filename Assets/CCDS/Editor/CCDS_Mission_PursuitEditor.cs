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
using UnityEngine.AI;

[CustomEditor(typeof(CCDS_MissionObjective_Pursuit))]
public class CCDS_Mission_PursuitEditor : Editor {

    CCDS_MissionObjective_Pursuit prop;
    GUISkin skin;
    Color guiColor;
    static bool showInfo;

    ACCDS_Vehicle pursuitVehicle;
    NavMeshData navMeshData;
    NavMeshDataInstance NavMeshDataInstance;
    Vector3 BoundsCenter = Vector3.zero;
    Vector3 BoundsSize = new Vector3(2500, 2500, 2500);

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

        if (CCDS_Settings.Instance.pursuitVehicle != null)
            pursuitVehicle = CCDS_Settings.Instance.pursuitVehicle;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_MissionObjective_Pursuit)target;
        serializedObject.Update();
        GUI.skin = skin;

        if (!EditorApplication.isPlaying)
            prop.GetVehicle();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("AI pursuit vehicle must be child object of this manager. AI vehicles will use the selected AI path to escape from the player.", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        if (prop.pursuitVehicle == null) {

            string error = "Pursuit vehicle needed, please select it in the scene or create a new pursuit vehicle!";
            EditorGUILayout.HelpBox(error, MessageType.Error);

            pursuitVehicle = (ACCDS_Vehicle)EditorGUILayout.ObjectField("Pursuit Vehicle Prefab To Crate", pursuitVehicle, typeof(ACCDS_Vehicle), false);

            EditorGUILayout.Space();

            if (pursuitVehicle == null)
                GUI.enabled = false;

            GUI.color = Color.cyan;

            if (GUILayout.Button("Create New Pursuit Vehicle")) {

                Selection.activeGameObject = CreateNewAIVehicle().gameObject;
                Selection.activeGameObject.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
                SceneView.FrameLastActiveSceneView();

                EditorUtility.SetDirty(prop);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            }

            GUI.color = guiColor;

            GUI.enabled = true;

        }

        if (prop.waypointPath == null) {

            string error = "Waypoint Path is not selected for the pursuit vehicle. Please assign a waypoint path, or create one.";
            EditorGUILayout.HelpBox(error, MessageType.Error);

            RCCP_AIWaypointsContainer[] waypointPaths = FindObjectsByType<RCCP_AIWaypointsContainer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            if (waypointPaths != null && waypointPaths.Length > 0) {

                EditorGUILayout.HelpBox("Existing waypoint paths are listed below;", MessageType.None);

                for (int i = 0; i < waypointPaths.Length; i++) {

                    EditorGUILayout.ObjectField("WaypointPath_" + i.ToString(), waypointPaths[i], typeof(RCCP_AIWaypointsContainer), true);

                }

            }

            if (GUILayout.Button("Create New Waypoint Path")) {

                EditorApplication.ExecuteMenuItem("Tools/BoneCracker Games/Realistic Car Controller Pro/Create/Scene/Add AI Waypoints Container To Scene");

                EditorApplication.delayCall += () => {

                    RCCP_AIWaypointsContainer[] containers = FindObjectsByType<RCCP_AIWaypointsContainer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

                    foreach (var item in containers) {

                        if (item != null && item.waypoints.Count == 0)
                            prop.waypointPath = item;

                    }

                };

            }

        }

        EditorGUILayout.Space();

        showInfo = EditorGUILayout.ToggleLeft("Show Info", showInfo);

        if (showInfo)
            EditorGUILayout.HelpBox("Be sure your scene has proper NavMesh, because AI controller will be using the NavMesh for pathfinding. You can create NavMesh from AI --> Navigation (Obsolete). Please download the 'AI Navigation' package from the package manager.", MessageType.None);

        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();

        if (GUILayout.Button("Mission Manager"))
            Selection.activeGameObject = CCDS_MissionObjectiveManager.Instance.gameObject;

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        serializedObject.ApplyModifiedProperties();

    }
    public ACCDS_Vehicle CreateNewAIVehicle() {

        GameObject created = Instantiate(pursuitVehicle.gameObject, Vector3.zero, Quaternion.identity);
        created.transform.name = pursuitVehicle.transform.name;

        created.transform.SetParent(prop.transform);
        created.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        prop.SetVehicle(created.GetComponent<ACCDS_Vehicle>());

        return created.GetComponent<ACCDS_Vehicle>();

    }

}
