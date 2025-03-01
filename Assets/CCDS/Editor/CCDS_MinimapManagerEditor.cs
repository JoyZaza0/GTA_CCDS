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

[CustomEditor(typeof(CCDS_MinimapManager))]
public class CCDS_MinimapManagerEditor : Editor {

    CCDS_MinimapManager prop;
    GUISkin skin;
    Color guiColor;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_MinimapManager)target;
        serializedObject.Update();
        GUI.skin = skin;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("Minimap camera is designed to render the 2d map along with the icons. Minimap camera shouldn't render other renderers. Normally, minimap camera is rendering these layers named 'CCDS_MinimapItem' and 'CCDS_Map'. Icons have this layer already. Be sure your 2d map gameobject in the scene has proper layer, otherwise it will not be rendered by the minimap camera.\n\n2D map is actually a simple plane or quad with proper map texture on it.", MessageType.None);
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        EditorGUILayout.EndVertical();

        prop.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        serializedObject.ApplyModifiedProperties();

    }

}
