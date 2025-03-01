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

public class CCDS_EditorWindows : EditorWindow {

    [MenuItem("Tools/BoneCracker Games/CCDS/Edit Settings", false, -10000)]
    [MenuItem("GameObject/BoneCracker Games/CCDS/Edit Settings", false, -10000)]
    public static void OpenSettings() {

        Selection.activeObject = CCDS_Settings.Instance;

    }

    [MenuItem("Tools/BoneCracker Games/CCDS/Player Vehicles", false, -10000)]
    [MenuItem("GameObject/BoneCracker Games/CCDS/Player Vehicles", false, -10000)]
    public static void OpenPlayerVehicles() {

        Selection.activeObject = CCDS_PlayerVehicles.Instance;

    }

    [MenuItem("Tools/BoneCracker Games/CCDS/Create/Scene Managers/Main Menu/Check & Add Scene Managers", false, -9000)]
    [MenuItem("GameObject/BoneCracker Games/CCDS/Create/Scene Managers/Main Menu/Check & Add Scene Managers", false, -9000)]
    public static void CheckSceneManagers_MainMenu() {

        CheckMainMenu();

    }

    [MenuItem("Tools/BoneCracker Games/CCDS/Create/Scene Managers/Gameplay/Check & Add Scene Managers", false, -9000)]
    [MenuItem("GameObject/BoneCracker Games/CCDS/Create/Scene Managers/Gameplay/Check & Add Scene Managers", false, -9000)]
    public static void CheckSceneManagers_Gameplay() {

        CheckGameplay();

    }

    [MenuItem("Tools/BoneCracker Games/CCDS/Enable SceneView Panel", false, 5000)]
    [MenuItem("GameObject/BoneCracker Games/CCDS/Enable SceneView Panel", false, 5000)]
    public static void EnableView() {

        SessionState.SetBool("CCDS_Editor_ShowSceneViewErrors", true);
        CCDS_SceneViewGUI.CheckAgain();

    }

    [MenuItem("Tools/BoneCracker Games/CCDS/Disable SceneView Panel", false, 5000)]
    [MenuItem("GameObject/BoneCracker Games/CCDS/Disable SceneView Panel", false, 5000)]
    public static void DisableView() {

        SessionState.SetBool("CCDS_Editor_ShowSceneViewErrors", false);
        CCDS_SceneViewGUI.CheckAgain();

    }

    [MenuItem("Tools/BoneCracker Games/CCDS/Welcome Window", false, 7000)]
    [MenuItem("GameObject/BoneCracker Games/CCDS/Welcome Window", false, 7000)]
    public static void OpenWindow() {

        GetWindow<CCDS_WelcomeWindow>(true);

    }

    [MenuItem("Tools/BoneCracker Games/CCDS/Help", false, 8000)]
    [MenuItem("GameObject/BoneCracker Games/CCDS/Help", false, 8000)]
    public static void Help() {

        EditorUtility.DisplayDialog("Contact", "Please include your invoice number while sending a contact form. I usually respond within a business day.", "Close");

        string url = "https://www.bonecrackergames.com/contact/";
        Application.OpenURL(url);

    }

    static void CheckMainMenu() {

        CCDS_SceneManager sceneManager = CCDS_SceneManager.Instance;
        sceneManager.levelType = CCDS_SceneManager.LevelType.MainMenu;
        sceneManager.GetAllComponents();
        Selection.activeGameObject = sceneManager.gameObject;

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

    }

    static void CheckGameplay() {

        CCDS_SceneManager sceneManager = CCDS_SceneManager.Instance;
        sceneManager.levelType = CCDS_SceneManager.LevelType.Gameplay;
        sceneManager.GetAllComponents();
        Selection.activeGameObject = sceneManager.gameObject;

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

    }

}
