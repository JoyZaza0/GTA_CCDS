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

[CustomEditor(typeof(CCDS_CopsManager))]
public class CCDS_CopsManagerEditor : Editor {

    CCDS_CopsManager prop;
    ACCDS_Vehicle referenceCopVehiclePrefab;
    GUISkin skin;
    Color guiColor;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

        if (CCDS_Settings.Instance.copVehicle != null)
            referenceCopVehiclePrefab = CCDS_Settings.Instance.copVehicle;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_CopsManager)target;
        serializedObject.Update();
        GUI.skin = skin;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("CCDS_CopsManager is responsible for storing, managing and observing the cops. ", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        if (prop.allCops == null)
            prop.allCops = new List<CCDS_AI_Cop>();

        referenceCopVehiclePrefab = (ACCDS_Vehicle)EditorGUILayout.ObjectField("Cop Prefab To Crate", referenceCopVehiclePrefab, typeof(ACCDS_Vehicle), false);

        EditorGUILayout.Space();

        if (referenceCopVehiclePrefab == null)
            GUI.enabled = false;

        GUI.color = Color.green;

        if (GUILayout.Button("Create New Cop")) {

            CreateNewCop();

            EditorUtility.SetDirty(prop);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        }

        GUI.color = guiColor;

        GUI.enabled = true;

        EditorGUILayout.Separator();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Separator();

        if (!EditorApplication.isPlaying)
            prop.GetAllCops();

        prop.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        serializedObject.ApplyModifiedProperties();

    }

    public void CreateNewCop() {

        CCDS_AI_Cop newCop = Instantiate(referenceCopVehiclePrefab, Vector3.zero, Quaternion.identity).GetComponent<CCDS_AI_Cop>();
        newCop.transform.name = CCDS_Settings.Instance.copVehicle.name;
        newCop.transform.SetParent(prop.transform);
        newCop.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        newCop.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
        prop.AddNewCop(newCop);
        Selection.activeGameObject = newCop.gameObject;
        SceneView.FrameLastActiveSceneView();

    }

}
