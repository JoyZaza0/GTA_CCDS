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

[CustomEditor(typeof(CCDS_Player))]
public class CCDS_PlayerEditor : Editor {

    CCDS_Player prop;
    GUISkin skin;
    Color guiColor;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_Player)target;
        serializedObject.Update();
        GUI.skin = skin;

        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        EditorGUILayout.Separator();
        EditorGUILayout.EndVertical();

        bool isPersistent = EditorUtility.IsPersistent(prop.gameObject);

        if (isPersistent)
            GUI.enabled = false;

        if (!EditorApplication.isPlaying) {

            if (PrefabUtility.GetCorrespondingObjectFromSource(prop.gameObject) == null) {

                EditorGUILayout.HelpBox("You'll need to create a new prefab for the vehicle first.", MessageType.Info);
                Color defColor = GUI.color;
                GUI.color = Color.red;

                if (GUILayout.Button("Create Prefab"))
                    CreatePrefab();

                GUI.color = defColor;

            } else {

                EditorGUILayout.HelpBox("Don't forget to save changes.", MessageType.Info);
                Color defColor = GUI.color;
                GUI.color = Color.green;

                if (GUILayout.Button("Save Prefab"))
                    SavePrefab();

                GUI.color = defColor;

            }

            GUI.enabled = true;

            bool foundPrefab = false;

            for (int i = 0; i < CCDS_PlayerVehicles.Instance.playerVehicles.Length; i++) {

                if (CCDS_PlayerVehicles.Instance.playerVehicles[i].vehicle != null) {

                    if (prop.transform.name == CCDS_PlayerVehicles.Instance.playerVehicles[i].vehicle.transform.name) {

                        foundPrefab = true;
                        break;

                    }

                }

            }

            if (!foundPrefab) {

                EditorGUILayout.HelpBox("Player vehicles list doesn't include this vehicle yet!", MessageType.Info);
                Color defColor = GUI.color;
                GUI.color = Color.green;

                if (GUILayout.Button("Add Prefab To Player Vehicles List")) {

                    if (PrefabUtility.GetCorrespondingObjectFromSource(prop.gameObject) == null)
                        CreatePrefab();
                    else
                        SavePrefab();

                    AddToList();

                }

                GUI.color = defColor;

            }

        }

        RCCP_CarController carController = prop.GetComponent<RCCP_CarController>();

        if (carController.GetComponentInChildren<RCCP_Customizer>(true) != null) {

            carController.GetComponentInChildren<RCCP_Customizer>(true).autoSave = false;
            carController.GetComponentInChildren<RCCP_Customizer>(true).autoLoadLoadout = false;

        }

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        serializedObject.ApplyModifiedProperties();

    }

    private void CreatePrefab() {

        PrefabUtility.SaveAsPrefabAssetAndConnect(prop.gameObject, "Assets/CCDS/Prefabs/Player Vehicles/" + prop.gameObject.name + ".prefab", InteractionMode.UserAction);
        Debug.Log("Created Prefab");

        serializedObject.ApplyModifiedProperties();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.SetDirty(prop);

    }

    private void SavePrefab() {

        PrefabUtility.SaveAsPrefabAssetAndConnect(prop.gameObject, "Assets/CCDS/Prefabs/Player Vehicles/" + prop.gameObject.name + ".prefab", InteractionMode.UserAction);
        Debug.Log("Saved Prefab");

        serializedObject.ApplyModifiedProperties();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.SetDirty(prop);

    }

    private void AddToList() {

        CCDS_PlayerVehicles.PlayerVehicle newVehicle = new CCDS_PlayerVehicles.PlayerVehicle();
	    newVehicle.vehicle = PrefabUtility.GetCorrespondingObjectFromSource(prop.gameObject).GetComponent<BCG_EnterExitVehicle>();

        CCDS_PlayerVehicles.Instance.AddNewVehicle(newVehicle);

        CCDS.SetVehicle(0);
        Debug.Log("Added Prefab To The Player Vehicles List");

        serializedObject.ApplyModifiedProperties();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject = CCDS_PlayerVehicles.Instance;

    }

}
