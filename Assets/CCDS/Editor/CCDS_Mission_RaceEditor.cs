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

[CustomEditor(typeof(CCDS_MissionObjective_Race))]
public class CCDS_Mission_RaceEditor : Editor {

    CCDS_MissionObjective_Race prop;
    GUISkin skin;
    Color guiColor;
    static bool showInfo;

    ACCDS_Vehicle racerVehicle;
    CCDS_MissionObjective_Race_Finisher raceFinisher;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

        if (CCDS_Settings.Instance.racerVehicle != null)
            racerVehicle = CCDS_Settings.Instance.racerVehicle;

        if (CCDS_Settings.Instance.raceFinisher != null)
            raceFinisher = CCDS_Settings.Instance.raceFinisher;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_MissionObjective_Race)target;
        serializedObject.Update();
        GUI.skin = skin;

        if (!EditorApplication.isPlaying)
            prop.GetAllRacers();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("All AI vehicles must be child object of this manager. AI vehicles will use the selected AI path to race.", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        if (TotalRacers() < 4 && prop.racers == null || (prop.racers != null && prop.racers.Count == 0)) {

            string error = "One racer vehicle needed at least!";
            EditorGUILayout.HelpBox(error, MessageType.Error);

            racerVehicle = (ACCDS_Vehicle)EditorGUILayout.ObjectField("Racer Vehicle Prefab To Crate", racerVehicle, typeof(ACCDS_Vehicle), false);

            EditorGUILayout.Space();

            if (racerVehicle == null)
                GUI.enabled = false;

            GUI.color = Color.cyan;

            if (GUILayout.Button("Create New Racer Vehicle")) {

                Selection.activeGameObject = CreateNewAIVehicle().gameObject;
                Selection.activeGameObject.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
                SceneView.FrameLastActiveSceneView();

                EditorUtility.SetDirty(prop);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            }

            GUI.color = guiColor;

            GUI.enabled = true;

        }

        if (prop.finisher == null) {

            string error = "Race finisher couldn't found, assign it in the scene or create a new one!";
            EditorGUILayout.HelpBox(error, MessageType.Error);

            raceFinisher = (CCDS_MissionObjective_Race_Finisher)EditorGUILayout.ObjectField("Racer Vehicle Prefab To Crate", raceFinisher, typeof(CCDS_MissionObjective_Race_Finisher), false);

            EditorGUILayout.Space();

            if (raceFinisher == null)
                GUI.enabled = false;

            GUI.color = Color.cyan;

            if (GUILayout.Button("Create New Race Finisher")) {

                Selection.activeGameObject = CreateNewRaceFinisher().gameObject;
                Selection.activeGameObject.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
                SceneView.FrameLastActiveSceneView();

                EditorUtility.SetDirty(prop);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            }

            GUI.color = guiColor;

            GUI.enabled = true;

        }

        if (prop.waypointPath == null) {

            string error = "Waypoint Path is not selected for the racer vehicles. Please assign a waypoint path, or create one.";
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

    private int TotalRacers() {

        if (prop.racers == null || (prop.racers != null && prop.racers.Count == 0))
            return 0;

        int total = 0;

        for (int i = 0; i < prop.racers.Count; i++) {

            if (prop.racers[i] != null)
                total++;

        }

        return total;

    }

    public ACCDS_Vehicle CreateNewAIVehicle() {

        GameObject created = Instantiate(racerVehicle.gameObject, Vector3.zero, Quaternion.identity);
        created.transform.name = racerVehicle.transform.name;

        created.transform.SetParent(prop.transform);
        created.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        prop.SetRacer(created.GetComponent<ACCDS_Vehicle>());

        return created.GetComponent<ACCDS_Vehicle>();

    }

    public CCDS_MissionObjective_Race_Finisher CreateNewRaceFinisher() {

        GameObject created = Instantiate(raceFinisher.gameObject, Vector3.zero, Quaternion.identity);
        created.transform.name = raceFinisher.transform.name;

        created.transform.SetParent(prop.transform);
        created.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        prop.finisher = created.GetComponent<CCDS_MissionObjective_Race_Finisher>();

        return created.GetComponent<CCDS_MissionObjective_Race_Finisher>();

    }

}
