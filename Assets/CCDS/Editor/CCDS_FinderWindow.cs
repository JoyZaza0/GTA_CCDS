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
using UnityEditor.SceneManagement;

public class CCDS_FinderWindow : EditorWindow {

    Vector2 scrollPos;
    public static System.Type type;
    public static string description;
    public static string buttonSelectText;
    public static string buttonCreateText;
    public static CCDS_ActionEvent_Find typeActionEvent;

    CCDS_GameModes.Mode gameMode;

    GUISkin skin;
    Color guiColor;

    public static void OpenWindow(System.Type _type, string title, string desc, string buttonSelect, string buttonCreate) {

        type = _type;
        description = desc;
        buttonSelectText = buttonSelect;
        buttonCreateText = buttonCreate;
        GetWindow<CCDS_FinderWindow>(true, title);

    }

    private void OnEnable() {

        titleContent = new GUIContent("Finder");
        minSize = new Vector2(500f, 180f);

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

        if (typeActionEvent == null)
            typeActionEvent = new CCDS_ActionEvent_Find();

    }

    public void OnGUI() {

        GUI.skin = skin;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField(description, EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.Space();

        switch (type.Name) {

            case "ACCDS_Mission":

                CCDS_MissionObjectiveManager missionObjectiveManager = CCDS_MissionObjectiveManager.Instance;
                missionObjectiveManager.GetAllMissions();

                if (missionObjectiveManager.allMissions.Count > 0) {

                    for (int i = 0; i < missionObjectiveManager.allMissions.Count; i++) {

                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        EditorGUILayout.Space();

                        if (missionObjectiveManager.allMissions[i] != null) {

                            EditorGUILayout.ObjectField("", missionObjectiveManager.allMissions[i], typeof(ACCDS_Mission), true);

                            GUI.color = Color.cyan;

                            if (GUILayout.Button(buttonSelectText)) {

                                if (typeActionEvent == null)
                                    typeActionEvent = new CCDS_ActionEvent_Find();

                                typeActionEvent.Invoke(missionObjectiveManager.allMissions[i].gameObject.GetInstanceID());
                                Close();

                            }

                            GUI.color = guiColor;

                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space();

                    }

                }

                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                GUI.color = Color.green;

                gameMode = (CCDS_GameModes.Mode)EditorGUILayout.EnumPopup("Mission Type", gameMode);

                if (GUILayout.Button(buttonCreateText)) {

                    ACCDS_Mission newMission = CCDS_MissionObjectiveManager.Instance.CreateNewMissionObjective(gameMode);
                    Selection.activeGameObject = newMission.gameObject;

                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                    if (typeActionEvent == null)
                        typeActionEvent = new CCDS_ActionEvent_Find();

                    typeActionEvent.Invoke(newMission.gameObject.GetInstanceID());
                    Close();

                }

                GUI.color = guiColor;
                EditorGUILayout.EndHorizontal();



                break;

            case "CCDS_MissionObjectivePosition":

                CCDS_MissionObjectivePositionsManager missionObjectivePositionManager = CCDS_MissionObjectivePositionsManager.Instance;
                missionObjectivePositionManager.GetAllPositions();

                if (missionObjectivePositionManager.allPositions.Count > 0) {

                    for (int i = 0; i < missionObjectivePositionManager.allPositions.Count; i++) {

                        if (missionObjectivePositionManager.allPositions[i] != null) {

                            EditorGUILayout.BeginVertical(GUI.skin.box);
                            EditorGUILayout.Space();

                            EditorGUILayout.ObjectField("", missionObjectivePositionManager.allPositions[i], typeof(CCDS_MissionObjectivePosition), true);

                            GUI.color = Color.cyan;

                            if (GUILayout.Button(buttonSelectText)) {

                                if (typeActionEvent == null)
                                    typeActionEvent = new CCDS_ActionEvent_Find();

                                typeActionEvent.Invoke(missionObjectivePositionManager.allPositions[i].gameObject.GetInstanceID());
                                Close();

                            }

                            GUI.color = guiColor;

                            EditorGUILayout.Space();
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.Space();

                        }

                    }

                }

                EditorGUILayout.Space();
                GUI.color = Color.green;
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal(GUI.skin.box);

                if (GUILayout.Button(buttonCreateText)) {

                    CCDS_MissionObjectivePosition newPosition = CCDS_MissionObjectivePositionsManager.Instance.CreateNewPosition();
                    Selection.activeGameObject = newPosition.gameObject;

                    newPosition.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
                    SceneView.FrameLastActiveSceneView();

                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                    if (typeActionEvent == null)
                        typeActionEvent = new CCDS_ActionEvent_Find();

                    typeActionEvent.Invoke(newPosition.gameObject.GetInstanceID());
                    Close();

                }

                EditorGUILayout.EndHorizontal();
                GUI.color = guiColor;

                break;

        }

        EditorGUILayout.EndScrollView();

    }

}
