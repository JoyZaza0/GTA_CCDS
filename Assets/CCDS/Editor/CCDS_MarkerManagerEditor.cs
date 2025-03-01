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

[CustomEditor(typeof(CCDS_MarkerManager))]
public class CCDS_MarkerManagerEditor : Editor {

    CCDS_MarkerManager prop;
    GUISkin skin;
    Color guiColor;

    CCDS_Marker processingThisMarker;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_MarkerManager)target;
        serializedObject.Update();
        GUI.skin = skin;

        if (!EditorApplication.isPlaying)
            prop.GetAllMarkers();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("CCDS_MarkerManager is responsible for storing the markers to start any mission. Each marker has specific mission (Mission Objective). Markers can't work without connected missions. New mission types can be created via CCDS_MissionManager.\n\nEach marker needs:\n\n * A mission position to transport the player vehicle to\n * A mission objective ", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;

        EditorGUILayout.HelpBox("All markers have been listed below. Red buttons means that property is not selected or configurated. ", MessageType.None);

        if (prop.allMarkers != null && prop.allMarkers.Count > 0) {

            for (int i = 0; i < prop.allMarkers.Count; i++) {

                if (prop.allMarkers[i] == null) {

                    prop.GetAllMarkers();
                    break;

                }

                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.LabelField(prop.allMarkers[i].transform.name, EditorStyles.boldLabel, GUILayout.MinWidth(10f));
                EditorGUILayout.BeginHorizontal();

                string butString;
                butString = "Marker";

                GUI.color = Color.green;

                if (GUILayout.Button(butString, GUILayout.MinWidth(10f))) {

                    Selection.activeGameObject = prop.allMarkers[i].gameObject;
                    SceneView.FrameLastActiveSceneView();

                }

                bool connectedMissionFound = prop.allMarkers[i].connectedMission;

                if (!connectedMissionFound)
                    GUI.color = Color.red;
                else
                    GUI.color = Color.green;

                butString = "Connected Mission";

                if (!connectedMissionFound)
                    butString += " [Add]";

                if (GUILayout.Button(butString, GUILayout.MinWidth(10f))) {

                    if (connectedMissionFound) {

                        Selection.activeGameObject = prop.allMarkers[i].connectedMission.gameObject;

                    } else {

                        processingThisMarker = prop.allMarkers[i];

                        EditorApplication.delayCall += () => {

                            CCDS_FinderWindow.OpenWindow(typeof(ACCDS_Mission), "Finder", "These missions are eligible for assigning for this marker", "Assign This Mission For " + processingThisMarker.transform.name, "Create New Mission For \n" + processingThisMarker.transform.name);
                            CCDS_FinderWindow.typeActionEvent.RemoveListener(CCDS_FinderListener);
                            CCDS_FinderWindow.typeActionEvent.AddListener(CCDS_FinderListener);

                        };

                    }

                }

                //if (!prop.allMarkers[i].connectedMission || (prop.allMarkers[i].connectedMission && !prop.allMarkers[i].connectedMission.mission.missionObjective))
                //    GUI.color = Color.red;
                //else
                //    GUI.color = Color.green;

                //butString = "Connected Mission";

                //if (!prop.allMarkers[i].connectedMission.mission.missionObjective)
                //    butString += " [Add]";

                //if (GUILayout.Button(butString, GUILayout.MinWidth(10f))) {

                //    if (prop.allMarkers[i].connectedMission.mission.missionObjective) {

                //        Selection.activeGameObject = prop.allMarkers[i].connectedMission.mission.missionObjective.gameObject;

                //    } else {

                //        processingThisMarker = prop.allMarkers[i];

                //        EditorApplication.delayCall += () => {

                //            CCDS_FinderWindow.OpenWindow(typeof(ACCDS_Mission), "Finder", "These missions are eligible for assigning for this marker", "Assign Mission For " + processingThisMarker.transform.name, "Create New Mission For \n" + processingThisMarker.transform.name);
                //            CCDS_FinderWindow.typeActionEvent.RemoveListener(CCDS_FinderListener);
                //            CCDS_FinderWindow.typeActionEvent.AddListener(CCDS_FinderListener);

                //        };

                //    }

                //}

                GUI.color = guiColor;

                if (connectedMissionFound) {

                    if (!prop.allMarkers[i].connectedMission.transportToThisLocation)
                        GUI.color = Color.red;
                    else
                        GUI.color = Color.green;

                    butString = "Spawn Position";

                    if (!prop.allMarkers[i].connectedMission.transportToThisLocation)
                        butString += " [Add]";

                    if (GUILayout.Button(butString, GUILayout.MinWidth(10f))) {

                        if (prop.allMarkers[i].connectedMission.transportToThisLocation != null) {

                            Selection.activeGameObject = prop.allMarkers[i].connectedMission.transportToThisLocation.gameObject;
                            SceneView.FrameLastActiveSceneView();

                        } else {

                            processingThisMarker = prop.allMarkers[i];

                            EditorApplication.delayCall += () => {

                                CCDS_FinderWindow.OpenWindow(typeof(CCDS_MissionObjectivePosition), "Finder", "These spawn positions are eligible for assigning for this marker", "Assign This Spawn Position For  \n" + processingThisMarker.transform.name, "Create New Spawn Position For " + processingThisMarker.transform.name);
                                CCDS_FinderWindow.typeActionEvent.RemoveListener(CCDS_FinderListener);
                                CCDS_FinderWindow.typeActionEvent.AddListener(CCDS_FinderListener);

                            };

                        }

                    }

                }

                GUI.color = guiColor;

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();

            }

        }

        EditorGUI.indentLevel--;

        EditorGUILayout.Separator();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Separator();

        GUI.color = Color.green;

        if (GUILayout.Button("Create A New Marker"))
            CreateNewMarker();

        GUI.color = guiColor;

        if (GUILayout.Button("Scene Manager"))
            Selection.activeGameObject = CCDS_SceneManager.Instance.gameObject;

        prop.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        serializedObject.ApplyModifiedProperties();

    }

    public void CreateNewMarker() {

        EditorApplication.delayCall += () => {

            CCDS_InputFieldWindow.OpenWindow("Creating New Marker", "Enter name of the new marker", "Create New Marker", "CCDS_NewMarker");
            CCDS_InputFieldWindow.inputTextActionEvent.RemoveListener(CCDS_InputFieldListener);
            CCDS_InputFieldWindow.inputTextActionEvent.AddListener(CCDS_InputFieldListener);

        };

    }

    public void CCDS_InputFieldListener(string inputText) {

        CCDS_Marker newMarker = Instantiate(CCDS_Settings.Instance.marker.gameObject, Vector3.zero, Quaternion.identity).GetComponent<CCDS_Marker>();

        if (!string.IsNullOrWhiteSpace(inputText))
            newMarker.transform.name = inputText;
        else
            newMarker.transform.name = CCDS_Settings.Instance.marker.transform.name;

        newMarker.transform.SetParent(prop.transform);
        newMarker.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        prop.AddNewMarker(newMarker);
        newMarker.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
        SceneView.lastActiveSceneView.ShowNotification(new GUIContent("New marker has been created. Create and assign a new mission for it..."), 3);
        Selection.activeGameObject = newMarker.gameObject;
        SceneView.FrameLastActiveSceneView();

        EditorUtility.SetDirty(prop);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

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
            processingThisMarker.connectedMission = mission;

        CCDS_MissionObjectivePosition spawnPoint = null;
        CCDS_MissionObjectivePosition[] spawnPoints = FindObjectsByType<CCDS_MissionObjectivePosition>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < spawnPoints.Length; i++) {

            if (spawnPoints[i].gameObject.GetInstanceID() == id) {

                spawnPoint = spawnPoints[i];
                break;

            }

        }

        if (spawnPoint != null)
            processingThisMarker.connectedMission.transportToThisLocation = spawnPoint;

        processingThisMarker = null;

        EditorUtility.SetDirty(prop);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

    }

}
