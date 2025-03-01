//----------------------------------------------
//        City Car Driving Simulator
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CCDS_InputFieldWindow : EditorWindow {

    public static string inputText;
    public static string description;
    public static string buttonText;
    public static CCDS_ActionEvent inputTextActionEvent;

    GUISkin skin;

    public static void OpenWindow(string title, string desc, string button, string defaultText) {

        description = desc;
        buttonText = button;
        inputText = defaultText;
        GetWindow<CCDS_InputFieldWindow>(true, title);

    }

    private void OnEnable() {

        titleContent = new GUIContent("Enter name of the new marker");
        maxSize = new Vector2(400f, 120f);
        minSize = maxSize;

        skin = Resources.Load<GUISkin>("CCDS_Gui");

        if (inputTextActionEvent == null)
            inputTextActionEvent = new CCDS_ActionEvent();

    }

    public void OnGUI() {

        GUI.skin = skin;
        EditorGUILayout.LabelField(description, EditorStyles.centeredGreyMiniLabel);
        inputText = EditorGUILayout.TextField(inputText);

        if (GUILayout.Button(buttonText)) {

            if (inputTextActionEvent == null)
                inputTextActionEvent = new CCDS_ActionEvent();

            inputTextActionEvent.Invoke(inputText);
            Close();

        }

    }

}
