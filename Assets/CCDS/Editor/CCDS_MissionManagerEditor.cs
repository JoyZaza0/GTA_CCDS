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

[CustomEditor(typeof(CCDS_MissionObjectiveManager))]
public class CCDS_MissionManagerEditor : Editor {

    CCDS_MissionObjectiveManager prop;
    GUISkin skin;
    Color guiColor;
    CCDS_GameModes.Mode gameMode;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_MissionObjectiveManager)target;
        serializedObject.Update();
        GUI.skin = skin;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("Missions triggered by the markers. Each marker has a specific mission objective. Markers are connected with these mission objectives. Be sure your markers have corresponding mission objectives.", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;

        if (prop.allMissions != null && prop.allMissions.Count > 0) {

            for (int i = 0; i < prop.allMissions.Count; i++) {

                if (prop.allMissions[i] == null) {

                    if (!EditorApplication.isPlaying)
                        prop.GetAllMissions();

                    break;

                }

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(prop.allMissions[i].transform.name, EditorStyles.boldLabel, GUILayout.MinWidth(10f));

                GUI.color = guiColor;

                ICCDS_CheckEditorError check = prop.allMissions[i] as ICCDS_CheckEditorError;

                if (check != null) {

                    if (check.CheckErrors() != null && check.CheckErrors().Length > 0)
                        GUI.color = Color.red;

                }

                if (GUILayout.Button("Select Mission Objective", GUILayout.MinWidth(10f))) {

                    Selection.activeGameObject = prop.allMissions[i].gameObject;

                }

                GUI.color = guiColor;

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                if (check != null && check.CheckErrors() != null && check.CheckErrors().Length > 0) {

                    for (int k = 0; k < check.CheckErrors().Length; k++) {

                        EditorGUILayout.HelpBox(check.CheckErrors()[k], MessageType.Error);

                    }

                }

            }

        }

        EditorGUI.indentLevel--;

        EditorGUILayout.Separator();

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.BeginHorizontal(GUI.skin.box);

        float defaultLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 100f;

        gameMode = (CCDS_GameModes.Mode)EditorGUILayout.EnumPopup("Mission Type", gameMode);

        EditorGUIUtility.labelWidth = defaultLabelWidth;
        GUI.color = Color.cyan;

        string butString = "Create New Mission";

        butString += " [" + gameMode.ToString() + "]";

        EditorGUILayout.Space();

        if (GUILayout.Button(butString)) {

            ACCDS_Mission newMission = CCDS_MissionObjectiveManager.Instance.CreateNewMissionObjective(gameMode);
            Selection.activeGameObject = newMission.gameObject;

            EditorUtility.SetDirty(prop.gameObject);

        }

        GUI.color = guiColor;

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();

        EditorGUILayout.HelpBox("If there are active mission objects in the scene, all mission objects will be deactivated in the scene when the game starts. Mission objects will be enabled by the markers only.", MessageType.None);

        EditorGUILayout.EndVertical();
        EditorGUILayout.Separator();

        EditorGUILayout.HelpBox("Check all mission objectives one by one, and be sure you don't have any errors or warnings. You'll see warnings and errors on your console and inspector panel in this case...", MessageType.None);

        if (!EditorApplication.isPlaying)
            prop.GetAllMissions();

        prop.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        serializedObject.ApplyModifiedProperties();

    }

}
