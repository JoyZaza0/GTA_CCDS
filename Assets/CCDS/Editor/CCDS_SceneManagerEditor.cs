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

[CustomEditor(typeof(CCDS_SceneManager))]
public class CCDS_SceneManagerEditor : Editor {

    CCDS_SceneManager prop;
    GUISkin skin;
    Color guiColor;
    static bool readme;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("CCDS_Gui");
        guiColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (CCDS_SceneManager)target;
        serializedObject.Update();
        GUI.skin = skin;

        if (!EditorApplication.isPlaying)
            prop.GetAllComponents();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("CCDS_SceneManager is responsible for checking and observing the main controller components in the scene. All managers must be added for full functional gameplay. Game would still run without them.", MessageType.None);
        EditorGUILayout.HelpBox("Green buttons means the manager has been found in the scene, it can be selected by clicking the button. Red buttons means the manager couldn't found in the scene, it can be created by clicking the button.", MessageType.None);
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("levelType"), new GUIContent("Level Type", "Level type."));

        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        if (EditorApplication.isPlaying)
            EditorGUILayout.HelpBox("Managers can't be created at runtime, this means clicking the red buttons won't do anything during gameplay.", MessageType.Info);

        switch (prop.levelType) {

            case CCDS_SceneManager.LevelType.MainMenu:

                MainMenu();
                break;

            case CCDS_SceneManager.LevelType.Gameplay:

                Gameplay();
                break;

        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        RCCP();

        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.HelpBox("Checks all managers and creates if necessary.", MessageType.None);

        GUI.color = Color.cyan;

        if (EditorApplication.isPlaying)
            GUI.enabled = false;

        if (GUILayout.Button("Check & Create All Managers"))
            CreateAll();

        GUI.enabled = true;

        GUI.color = guiColor;

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        prop.CheckErrors();
        EditorGUILayout.Space();

        readme = EditorGUILayout.ToggleLeft(new GUIContent("Info", "Read me if you want, or don't read me if you don't want. It's up to you, or not?"), readme);

        if (readme)
            EditorGUILayout.HelpBox("This main manager will check all sub managers in the scene and let you know if something found not right. However, it won't guarantee a clean gameplay. I would recommend you to check the documentation before use, and don't miss the common mistakes section. Keep an eye on your console and inspector panel for more detailed infos.", MessageType.None);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();

        prop.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        if (FindFirstObjectByType<RCCP_SceneManager>())
            RCCP_SceneManager.Instance.registerLastVehicleAsPlayer = false;

        serializedObject.ApplyModifiedProperties();

    }

    private void MainMenu() {

        if (prop.MainMenuManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.MainMenuManager == null ? "Add " : "") + "Mainmenu Manager"))) {

            if (prop.MainMenuManager)
                Selection.activeGameObject = prop.MainMenuManager.gameObject;
            else if (!EditorApplication.isPlaying)
                CreateComponent(typeof(CCDS_MainMenuManager));

        }

        if (prop.MainMenuUIManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.MainMenuUIManager == null ? "Add " : "") + "Mainmenu UI Manager"))) {

            if (prop.MainMenuUIManager) {

                Selection.activeGameObject = prop.MainMenuUIManager.gameObject;

            } else if (!EditorApplication.isPlaying) {

                GameObject dp = Instantiate(CCDS_Settings.Instance.mainmenuUIManager.gameObject, Vector3.zero, Quaternion.identity);
                dp.transform.name = CCDS_Settings.Instance.mainmenuUIManager.transform.name;
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            }

        }

        if (prop.CameraOrbit != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.CameraOrbit == null ? "Add " : "") + "Camera Orbit"))) {

            if (prop.CameraOrbit) {

                Selection.activeGameObject = prop.CameraOrbit.gameObject;
                SceneView.FrameLastActiveSceneView();

            } else if (!EditorApplication.isPlaying) {

                CreateComponent(typeof(CCDS_Camera_Orbit));

            }

        }

        GUI.color = guiColor;

        if (prop.SoundtrackManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.SoundtrackManager == null ? "Add " : "") + "Soundtrack Manager"))) {

            if (prop.SoundtrackManager) {

                Selection.activeGameObject = prop.SoundtrackManager.gameObject;

            } else if (!EditorApplication.isPlaying) {

                CreateComponent(typeof(CCDS_SoundtrackManager));

            }

        }

    }

    private void Gameplay() {

        if (prop.GameplayManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.GameplayManager == null ? "Add " : "") + "Gameplay Manager"))) {

            if (prop.GameplayManager)
                Selection.activeGameObject = prop.GameplayManager.gameObject;
            else if (!EditorApplication.isPlaying)
                CreateComponent(typeof(CCDS_GameplayManager));

        }

        if (prop.CCDSUIManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.CCDSUIManager == null ? "Add " : "") + "Gameplay UI Manager"))) {

            if (prop.CCDSUIManager) {

                Selection.activeGameObject = prop.CCDSUIManager.gameObject;

            } else if (!EditorApplication.isPlaying) {

                GameObject dp = Instantiate(CCDS_Settings.Instance.gameplayUIManager.gameObject, Vector3.zero, Quaternion.identity);
                dp.transform.name = CCDS_Settings.Instance.gameplayUIManager.transform.name;
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            }

        }

        if (prop.MarkerManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.MarkerManager == null ? "Add " : "") + "Marker Manager"))) {

            if (prop.MarkerManager)
                Selection.activeGameObject = prop.MarkerManager.gameObject;
            else if (!EditorApplication.isPlaying)
                CreateComponent(typeof(CCDS_MarkerManager));

        }

        if (prop.MissionManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.MissionManager == null ? "Add " : "") + "Mission Manager"))) {

            if (prop.MissionManager)
                Selection.activeGameObject = prop.MissionManager.gameObject;
            else if (!EditorApplication.isPlaying)
                CreateComponent(typeof(CCDS_MissionObjectiveManager));

        }

        if (prop.MissionPositions != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.MissionPositions == null ? "Add " : "") + "Mission Position Manager"))) {

            if (prop.MissionPositions)
                Selection.activeGameObject = prop.MissionPositions.gameObject;
            else if (!EditorApplication.isPlaying)
                CreateComponent(typeof(CCDS_MissionObjectivePositionsManager));

        }

        if (prop.MinimapManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.MinimapManager == null ? "Add " : "") + "Minimap Manager"))) {

            if (prop.MinimapManager)
                Selection.activeGameObject = prop.MinimapManager.gameObject;
            else if (!EditorApplication.isPlaying)
                CreateComponent(typeof(CCDS_MinimapManager));

        }

        if (prop.CopsManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.CopsManager == null ? "Add " : "") + "Cops Manager"))) {

            if (prop.CopsManager)
                Selection.activeGameObject = prop.CopsManager.gameObject;
            else if (!EditorApplication.isPlaying)
                CreateComponent(typeof(CCDS_CopsManager));

        }

        GUI.color = guiColor;

    }

    private void RCCP() {

        if (prop.RCCPSceneManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.RCCPSceneManager == null ? "Add " : "") + "RCCP Scene Manager"))) {

            if (prop.RCCPSceneManager) {

                Selection.activeGameObject = prop.RCCPSceneManager.gameObject;

            } else {

                RCCP_SceneManager sm = RCCP_SceneManager.Instance;
                sm.registerLastVehicleAsPlayer = false;

            }

        }

        if (prop.levelType == CCDS_SceneManager.LevelType.Gameplay) {

            if (prop.RCCPCamera != null)
                GUI.color = Color.green;
            else
                GUI.color = Color.red;

            if (GUILayout.Button(new GUIContent((prop.RCCPCamera == null ? "Add " : "") + "CCDS / RCCP Camera"))) {

                if (prop.RCCPCamera) {

                    Selection.activeGameObject = prop.RCCPCamera.gameObject;
                    SceneView.FrameLastActiveSceneView();

                } else if (!EditorApplication.isPlaying) {

                    GameObject dp = Instantiate(CCDS_Settings.Instance.vehicleCamera.gameObject, Vector3.zero, Quaternion.identity);
                    dp.transform.name = CCDS_Settings.Instance.vehicleCamera.transform.name;
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                }

            }

        }

        GUI.color = guiColor;

    }

    public GameObject CreateComponent(Type monoBehaviour) {

        GameObject newGO = new GameObject(monoBehaviour.FullName);
        newGO.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        newGO.AddComponent(monoBehaviour);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        return newGO;

    }

    private void CreateAll() {

        switch (prop.levelType) {

            case CCDS_SceneManager.LevelType.MainMenu:

                if (!prop.MainMenuManager)
                    CreateComponent(typeof(CCDS_MainMenuManager));

                if (!prop.MainMenuUIManager) {

                    GameObject dp = PrefabUtility.InstantiatePrefab(CCDS_Settings.Instance.mainmenuUIManager.gameObject) as GameObject;
                    dp.transform.position = Vector3.zero;
                    dp.transform.rotation = Quaternion.identity;
                    dp.transform.name = CCDS_Settings.Instance.mainmenuUIManager.transform.name;

                }

                if (!prop.CameraOrbit) {

                    GameObject createdCameraOrbit = CreateComponent(typeof(CCDS_Camera_Orbit));

                    if (createdCameraOrbit.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>() == null)
                        createdCameraOrbit.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();

                }

                if (!prop.RCCPSceneManager) {

                    RCCP_SceneManager sm = RCCP_SceneManager.Instance;
                    sm.registerLastVehicleAsPlayer = false;

                }

                break;

            case CCDS_SceneManager.LevelType.Gameplay:

                if (!prop.GameplayManager)
                    CreateComponent(typeof(CCDS_GameplayManager));

                if (!prop.CCDSUIManager) {

                    GameObject dp = PrefabUtility.InstantiatePrefab(CCDS_Settings.Instance.gameplayUIManager.gameObject) as GameObject;
                    dp.transform.position = Vector3.zero;
                    dp.transform.rotation = Quaternion.identity;
                    dp.transform.name = CCDS_Settings.Instance.gameplayUIManager.transform.name;

                }

                if (!prop.MarkerManager)
                    CreateComponent(typeof(CCDS_MarkerManager));

                if (!prop.MissionManager)
                    CreateComponent(typeof(CCDS_MissionObjectiveManager));

                if (!prop.MissionPositions)
                    CreateComponent(typeof(CCDS_MissionObjectivePositionsManager));

                if (!prop.MinimapManager)
                    CreateComponent(typeof(CCDS_MinimapManager));

                if (!prop.CopsManager)
                    CreateComponent(typeof(CCDS_CopsManager));

                if (!prop.RCCPSceneManager) {

                    RCCP_SceneManager sm = RCCP_SceneManager.Instance;
                    sm.registerLastVehicleAsPlayer = false;

                }

                if (!prop.RCCPCamera) {

                    GameObject dp = Instantiate(CCDS_Settings.Instance.vehicleCamera.gameObject, Vector3.zero, Quaternion.identity);
                    dp.transform.name = CCDS_Settings.Instance.vehicleCamera.transform.name;

                }

                break;

        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

    }

}
