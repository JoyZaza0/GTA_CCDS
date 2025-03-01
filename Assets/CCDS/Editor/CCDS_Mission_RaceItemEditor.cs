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

[CustomEditor(typeof(CCDS_MissionObjective_Race_Vehicle))]
public class CCDS_Mission_RaceItemEditor : Editor {

    CCDS_MissionObjective_Race_Vehicle prop;
    GUISkin skin;
    Color guiColor;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_MissionObjective_Race_Vehicle)target;
        serializedObject.Update();
        GUI.skin = skin;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("Racer vehicle as a mission item.", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();

        if (GUILayout.Button("Mission Manager"))
            Selection.activeGameObject = CCDS_MissionObjectiveManager.Instance.gameObject;

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        if (!EditorApplication.isPlaying && prop.GetComponentInParent<CCDS_MissionObjective_Race>(true) != null)
            prop.GetComponentInParent<CCDS_MissionObjective_Race>(true).GetAllRacers();

        serializedObject.ApplyModifiedProperties();

    }

}
