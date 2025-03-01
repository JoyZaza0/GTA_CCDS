//----------------------------------------------
//        City Car Driving Simulator
//
// Copyright � 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;

public class CCDS_SceneChecker : Editor {

    static IEnumerable<ICCDS_CheckEditorError> checkEditorScripts;
    static string[] errors;

    [InitializeOnLoadMethod]
    public static void InitOnLoad() {

        errors = new string[0];

        EditorApplication.playModeStateChanged += (obj) => {

            if (obj == PlayModeStateChange.EnteredEditMode) {

                EditorApplication.delayCall += () => {

                    CheckAllComponents();
                    UpdateErrorComponents();

                };

            }

        };

        EditorApplication.hierarchyChanged += EditorApplication_hierarchyChanged;

    }

    public static void CheckAllComponents() {

        CCDS_MissionObjectiveManager sceneManager = FindFirstObjectByType<CCDS_MissionObjectiveManager>(FindObjectsInactive.Include);

        if (sceneManager != null)
            sceneManager.GetAllMissions();

        CCDS_MarkerManager markerManager = FindFirstObjectByType<CCDS_MarkerManager>(FindObjectsInactive.Include);

        if (markerManager != null)
            markerManager.GetAllMarkers();

        CCDS_MissionObjectiveManager missionManager = FindFirstObjectByType<CCDS_MissionObjectiveManager>(FindObjectsInactive.Include);

        if (missionManager != null)
            missionManager.GetAllMissions();

        CCDS_MissionObjectivePositionsManager positionsManager = FindFirstObjectByType<CCDS_MissionObjectivePositionsManager>(FindObjectsInactive.Include);

        if (positionsManager != null)
            positionsManager.GetAllPositions();

        CCDS_CopsManager copsManager = FindFirstObjectByType<CCDS_CopsManager>(FindObjectsInactive.Include);

        if (copsManager != null)
            copsManager.GetAllCops();

        if (!EditorApplication.isPlaying)
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

    }

    public static void UpdateErrorComponents() {

        checkEditorScripts = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ICCDS_CheckEditorError>();

    }

    public static string[] GetErrors() {

        List<string> allErrors = new List<string>();

        if (checkEditorScripts == null)
            return allErrors.ToArray();

        foreach (ICCDS_CheckEditorError editorScript in checkEditorScripts) {

            if (editorScript != null && editorScript.CheckErrors() != null && editorScript.CheckErrors().Length > 0) {

                for (int i = 0; i < editorScript.CheckErrors().Length; i++)
                    allErrors.Add(editorScript.CheckErrors()[i]);

            }

        }

        return allErrors.ToArray();

    }

    private static void EditorApplication_hierarchyChanged() {

        if (EditorApplication.isPlaying)
            return;

        UpdateErrorComponents();

    }

    public static void CheckCameras() {

        Camera[] allCameras = FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < allCameras.Length; i++) {

            if (allCameras[i].transform.GetComponentInParent<RCCP_Camera>(true) == null) {

                DestroyImmediate(allCameras[i].gameObject);

                if (!EditorApplication.isPlaying)
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            }

        }

    }

    public static void CheckAudioListeners() {

        AudioListener[] allListeners = FindObjectsByType<AudioListener>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < allListeners.Length; i++) {

            if (allListeners[i].transform.GetComponentInParent<RCCP_Camera>(true) == null) {

                DestroyImmediate(allListeners[i].gameObject);

                if (!EditorApplication.isPlaying)
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            }

        }

    }

}
