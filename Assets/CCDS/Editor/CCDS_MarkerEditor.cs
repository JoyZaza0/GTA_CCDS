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

[CustomEditor(typeof(CCDS_Marker))]
public class CCDS_MarkerEditor : Editor {

    CCDS_Marker prop;
    GUISkin skin;
    Color guiColor;
    CCDS_GameModes.Mode gameMode;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_Marker)target;
        serializedObject.Update();
        GUI.skin = skin;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("Marker for starting the target mission. Each marker has specific mission (Mission Objective). Markers can't work without connected missions. New mission type can be created for this marker below.\n\nEach marker needs:\n\n * A mission position to transport the player vehicle to\n * A mission objective", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        if (prop.connectedMission == null) {

            EditorGUILayout.HelpBox("Missing connected mission, please create or add a connected mission to this marker", MessageType.Error);

            //if (GUILayout.Button("Mission Manager")) {

            //    if (CCDS_MissionObjectiveManager.Instance != null)
            //        Selection.activeGameObject = CCDS_MissionObjectiveManager.Instance.gameObject;
            //    else
            //        EditorUtility.DisplayDialog("Missing CCDS_MissionObjectiveManager", "Scene is missing CCDS_MissionObjectiveManager, please create it to use missions on your markers.", "Ok");

            //}

            GUI.color = Color.green;

            if (GUILayout.Button("Create / Select Mission")) {

                EditorApplication.delayCall += () => {

                    CCDS_FinderWindow.OpenWindow(typeof(ACCDS_Mission), "Finder", "These missions are eligible for assigning for this marker", "Assign This Mission For " + prop.transform.name, "Create New Mission For \n" + prop.transform.name);
                    CCDS_FinderWindow.typeActionEvent.RemoveListener(CCDS_FinderListener);
                    CCDS_FinderWindow.typeActionEvent.AddListener(CCDS_FinderListener);

                };

            }

            GUI.color = guiColor;

            EditorGUILayout.EndVertical();

            if (GUI.changed)
                EditorUtility.SetDirty(prop);

            if (!EditorApplication.isPlaying && prop.GetComponentInParent<CCDS_MarkerManager>(true) != null)
                prop.GetComponentInParent<CCDS_MarkerManager>(true).GetAllMarkers();

            serializedObject.ApplyModifiedProperties();

            return;

        }

        EditorGUILayout.BeginVertical(GUI.skin.box);

        if (prop.connectedMission == null) {

            string error = "Missing 'Mission Objective', please select it in the scene or create a new one.";

            EditorGUILayout.HelpBox(error, MessageType.Error);

            gameMode = (CCDS_GameModes.Mode)EditorGUILayout.EnumPopup("Mission Type", gameMode);

            GUI.color = Color.cyan;

            string butString = "Create New Mission Objective";

            butString += " [" + gameMode.ToString() + "]";

            EditorGUILayout.Space();

            if (GUILayout.Button(butString)) {

                ACCDS_Mission newMission = CCDS_MissionObjectiveManager.Instance.CreateNewMissionObjective(gameMode);
                prop.connectedMission = newMission;
                Selection.activeGameObject = newMission.gameObject;
                SceneView.lastActiveSceneView.ShowNotification(new GUIContent("New mission has been created and assigned, now you can edit the mission."), 3);

            }

        } else {

            GUI.color = Color.green;

            string info = "This marker has been connected to the mission " + prop.connectedMission.ToString() + ".";
            EditorGUILayout.HelpBox(info, MessageType.Error);

            GUI.color = Color.cyan;

            string butString2 = "Select Mission Objective";

            butString2 += " [" + prop.connectedMission.transform.name.ToString() + "]";

            if (GUILayout.Button(butString2))
                Selection.activeGameObject = prop.connectedMission.gameObject;

        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();

        GUI.color = guiColor;

        if (prop.connectedMission.transportToThisLocation == null) {

            EditorGUILayout.BeginVertical(GUI.skin.box);

            string error = "Missing 'Transport To This Location', please select it in the scene or create a new one.";
            EditorGUILayout.HelpBox(error, MessageType.Error);

            //if (GUILayout.Button("Create New Mission Start Point")) {

            //    prop.connectedMission.transportToThisLocation = CCDS_MissionObjectivePositionsManager.Instance.CreateNewPosition();
            //    prop.connectedMission.transportToThisLocation.transform.position = prop.transform.position;
            //    prop.connectedMission.transportToThisLocation.transform.rotation = prop.transform.rotation;
            //    prop.connectedMission.transportToThisLocation.transform.position += prop.transform.forward * 10f;
            //    Selection.activeGameObject = prop.connectedMission.transportToThisLocation.gameObject;

            //    SceneView.FrameLastActiveSceneView();

            //    EditorUtility.SetDirty(prop);
            //    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            //}

            if (GUILayout.Button("Create / Select Mission Start Point")) {

                EditorApplication.delayCall += () => {

                    CCDS_FinderWindow.OpenWindow(typeof(CCDS_MissionObjectivePosition), "Finder", "These spawn positions are eligible for assigning for this marker", "Assign This Spawn Position For  \n" + prop.transform.name, "Create New Spawn Position For " + prop.transform.name);
                    CCDS_FinderWindow.typeActionEvent.RemoveListener(CCDS_FinderListener);
                    CCDS_FinderWindow.typeActionEvent.AddListener(CCDS_FinderListener);

                };

            }

            EditorGUILayout.EndVertical();

        }

        EditorGUILayout.Separator();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Separator();

        if (prop.connectedMission != null && prop.connectedMission.transportToThisLocation != null) {

            if (GUILayout.Button("Duplicate With Same Mission Objective & Mission Position")) {

                EditorApplication.delayCall += () => {

                    bool answer = (EditorUtility.DisplayDialog("Duplicating Marker", "Are you sure you want to duplicate this marker with the same mission objective and mission position?", "Duplicate", "Cancel"));

                    if (answer) {

                        GameObject dpMarker = Instantiate(prop.gameObject, prop.transform.root);
                        dpMarker.transform.SetPositionAndRotation(prop.transform.position, prop.transform.rotation);
                        dpMarker.transform.position += Vector3.forward * 5f;
                        dpMarker.transform.name = prop.transform.name;

                        GameObject dpMission = Instantiate(prop.connectedMission.gameObject, prop.connectedMission.transform.root);
                        dpMission.transform.SetPositionAndRotation(prop.connectedMission.transform.position, prop.connectedMission.transform.rotation);
                        dpMission.transform.name = prop.connectedMission.transform.name;

                        GameObject dpPos = Instantiate(prop.connectedMission.transportToThisLocation.gameObject, prop.connectedMission.transportToThisLocation.transform.root);
                        dpPos.transform.SetPositionAndRotation(prop.connectedMission.transportToThisLocation.transform.position, prop.connectedMission.transportToThisLocation.transform.rotation);
                        dpPos.transform.name = prop.connectedMission.transportToThisLocation.transform.name;

                        dpMarker.GetComponent<CCDS_Marker>().connectedMission = dpMission.GetComponent<ACCDS_Mission>();
                        dpMarker.GetComponent<CCDS_Marker>().connectedMission.transportToThisLocation = dpPos.GetComponent<CCDS_MissionObjectivePosition>();

                        Selection.activeGameObject = dpMarker;

                        EditorUtility.SetDirty(prop);
                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                    }

                };

            }

        }

        if (GUILayout.Button("Marker Manager"))
            Selection.activeGameObject = CCDS_MarkerManager.Instance.gameObject;

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        if (!EditorApplication.isPlaying && prop.GetComponentInParent<CCDS_MarkerManager>(true) != null)
            prop.GetComponentInParent<CCDS_MarkerManager>(true).GetAllMarkers();

        serializedObject.ApplyModifiedProperties();

    }

    public void CCDS_FinderListener(int id) {

        ACCDS_Mission mission = null;
        ACCDS_Mission[] missions = FindObjectsByType<ACCDS_Mission>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < missions.Length; i++) {

            if (missions[i].gameObject.GetInstanceID() == id) {

                mission = missions[i];
                break;

            }

        }

        if (mission != null)
            prop.connectedMission = mission;

        CCDS_MissionObjectivePosition spawnPoint = null;
        CCDS_MissionObjectivePosition[] spawnPoints = FindObjectsByType<CCDS_MissionObjectivePosition>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < spawnPoints.Length; i++) {

            if (spawnPoints[i].gameObject.GetInstanceID() == id) {

                spawnPoint = spawnPoints[i];
                break;

            }

        }

        if (spawnPoint != null)
            prop.connectedMission.transportToThisLocation = spawnPoint;

        //prop = null;

        EditorUtility.SetDirty(prop);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

    }

}
