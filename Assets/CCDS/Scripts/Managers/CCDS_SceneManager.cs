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

/// <summary>
/// Manages and observes all major systems such as gameplay, missions, markers, spawn positions, and minimap.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Managers/CCDS Scene Manager")]
public class CCDS_SceneManager : CCDS_Singleton<CCDS_SceneManager>, ICCDS_CheckEditorError {

    public enum LevelType { MainMenu, Gameplay }
    public LevelType levelType = LevelType.Gameplay;

    #region GAMEPLAYCOMPONENTS

    /// <summary>
    /// Gameplay manager.
    /// </summary>
    public CCDS_GameplayManager GameplayManager {

        get {

            if (gameplayManager == null)
                gameplayManager = FindFirstObjectByType<CCDS_GameplayManager>();

            return gameplayManager;

        }

    }

    /// <summary>
    /// Gameplay UI manager.
    /// </summary>
    public CCDS_UI_Manager CCDSUIManager {

        get {

            if (gameplayUIManager == null)
                gameplayUIManager = FindFirstObjectByType<CCDS_UI_Manager>();

            return gameplayUIManager;

        }

    }

    /// <summary>
    /// Marker manager.
    /// </summary>
    public CCDS_MarkerManager MarkerManager {

        get {

            if (markerManager == null)
                markerManager = FindFirstObjectByType<CCDS_MarkerManager>();

            return markerManager;

        }

    }

    /// <summary>
    /// Mission manager.
    /// </summary>
    public CCDS_MissionObjectiveManager MissionManager {

        get {

            if (missionManager == null)
                missionManager = FindFirstObjectByType<CCDS_MissionObjectiveManager>();

            return missionManager;

        }

    }

    /// <summary>
    /// Mission positions manager.
    /// </summary>
    public CCDS_MissionObjectivePositionsManager MissionPositions {

        get {

            if (missionPositions == null)
                missionPositions = FindFirstObjectByType<CCDS_MissionObjectivePositionsManager>();

            return missionPositions;

        }

    }

    /// <summary>
    /// Minimap manager.
    /// </summary>
    public CCDS_MinimapManager MinimapManager {

        get {

            if (minimapManager == null)
                minimapManager = FindFirstObjectByType<CCDS_MinimapManager>();

            return minimapManager;

        }

    }

    /// <summary>
    /// Cops manager.
    /// </summary>
    public CCDS_CopsManager CopsManager {

        get {

            if (copsManager == null)
                copsManager = FindFirstObjectByType<CCDS_CopsManager>();

            return copsManager;

        }

    }

    #endregion

    #region MAINMENUCOMPONENTS

    /// <summary>
    /// Mainmenu manager.
    /// </summary>
    public CCDS_MainMenuManager MainMenuManager {

        get {

            if (mainmenuManager == null)
                mainmenuManager = FindFirstObjectByType<CCDS_MainMenuManager>();

            return mainmenuManager;

        }

    }

    /// <summary>
    /// Mainmenu UI manager.
    /// </summary>
    public CCDS_UI_MainMenuManager MainMenuUIManager {

        get {

            if (mainmenuUIManager == null)
                mainmenuUIManager = FindFirstObjectByType<CCDS_UI_MainMenuManager>();

            return mainmenuUIManager;

        }

    }

    /// <summary>
    /// Orbit camera.
    /// </summary>
    public CCDS_Camera_Orbit CameraOrbit {

        get {

            if (orbitCamera == null)
                orbitCamera = FindFirstObjectByType<CCDS_Camera_Orbit>();

            return orbitCamera;

        }

    }

    /// <summary>
    /// Soundtrack manager.
    /// </summary>
    public CCDS_SoundtrackManager SoundtrackManager {

        get {

            if (soundtrackManager == null)
                soundtrackManager = FindFirstObjectByType<CCDS_SoundtrackManager>();

            return soundtrackManager;

        }

    }

    #endregion

    #region RCCPCOMPONENTS

    /// <summary>
    /// RCCP Scene Manager.
    /// </summary>
    public RCCP_SceneManager RCCPSceneManager {

        get {

            if (rccpSceneManager == null)
                rccpSceneManager = FindFirstObjectByType<RCCP_SceneManager>();

            return rccpSceneManager;

        }

    }

    /// <summary>
    /// RCCP Vehicle Camera.
    /// </summary>
    public RCCP_Camera RCCPCamera {

        get {

            if (rccpCamera == null)
                rccpCamera = FindFirstObjectByType<RCCP_Camera>();

            return rccpCamera;

        }

    }

    #endregion

    #region GET
    private CCDS_GameplayManager gameplayManager;
    private CCDS_UI_Manager gameplayUIManager;
    private CCDS_MarkerManager markerManager;
    private CCDS_MissionObjectiveManager missionManager;
    private CCDS_MissionObjectivePositionsManager missionPositions;
    private CCDS_MinimapManager minimapManager;
    private CCDS_CopsManager copsManager;

    private CCDS_MainMenuManager mainmenuManager;
    private CCDS_UI_MainMenuManager mainmenuUIManager;
    private CCDS_Camera_Orbit orbitCamera;
    private CCDS_SoundtrackManager soundtrackManager;

    private RCCP_SceneManager rccpSceneManager;
    private RCCP_Camera rccpCamera;
    #endregion

    private void Awake() {

        //  Getting and checking all components.
        GetAllComponents();

    }

    /// <summary>
    /// Gets all major manager components.
    /// </summary>
    public void GetAllComponents() {

        switch (levelType) {

            case LevelType.Gameplay:

                CCDS_GameplayManager _gameplayManager = GameplayManager;
                CCDS_UI_Manager _gameplayUIManager = gameplayUIManager;
                CCDS_MarkerManager _markerManager = MarkerManager;
                CCDS_MissionObjectiveManager _missionManager = MissionManager;
                CCDS_MissionObjectivePositionsManager _missionPositions = MissionPositions;
                CCDS_MinimapManager _minimapManager = MinimapManager;
                CCDS_CopsManager _copsManager = CopsManager;
                RCCP_SceneManager _rccpSceneManager = RCCPSceneManager;
                RCCP_Camera _rccpCamera = RCCPCamera;

                break;

            case LevelType.MainMenu:

                CCDS_MainMenuManager _mainmenuManager = MainMenuManager;
                CCDS_UI_MainMenuManager _mainmenuUIManager = MainMenuUIManager;
                CCDS_Camera_Orbit _cameraOrbit = CameraOrbit;
                RCCP_SceneManager _rccpSceneManager2 = RCCPSceneManager;
                CCDS_SoundtrackManager _soundtrackManager = SoundtrackManager;

                break;

        }

    }

    public string[] CheckMisconfigurations() {

        CCDS_SceneManager sm = this;

        List<string> errorMessages = new List<string>();

        switch (sm.levelType) {

            case LevelType.MainMenu:

                if (sm.MainMenuManager) {

                    if (sm.MainMenuManager.spawnPoint == null)
                        errorMessages.Add("Missing 'Spawn Point' on CCDS_MainMenuManager!");

                }

                if (sm.MainMenuUIManager) {

                    bool missingPanel = false;

                    if (sm.MainMenuUIManager.panels == null)
                        sm.MainMenuUIManager.panels = new GameObject[0];

                    for (int i = 0; i < sm.MainMenuUIManager.panels.Length; i++) {

                        if (sm.MainMenuUIManager.panels[i] == null)
                            missingPanel = true;

                    }

                    if (missingPanel)
                        errorMessages.Add("Missing UI Panel on MainMenu Manager!");

                    if (sm.MainMenuUIManager.panelTitleText == null)
                        errorMessages.Add("Missing UI Text 'Panel Title Text' on MainMenu Manager!");

                    if (sm.MainMenuUIManager.moneyText == null)
                        errorMessages.Add("Missing UI Text 'Money Text' on MainMenu Manager!");

                    if (sm.MainMenuUIManager.vehiclePriceText == null)
                        errorMessages.Add("Missing UI Text 'Vehicle Price Text' on MainMenu Manager!");

                    if (sm.MainMenuUIManager.selectVehicleButton == null)
                        errorMessages.Add("Missing UI Button 'Vehicle Select Button' on MainMenu Manager!");

                    if (sm.MainMenuUIManager.purchaseVehicleButton == null)
                        errorMessages.Add("Missing UI Button 'Vehicle Purchase Button' on MainMenu Manager!");

                }

                if (sm.CameraOrbit) {

                    sm.CameraOrbit.gameObject.tag = "MainCamera";

                    if (sm.CameraOrbit.target == null)
                        errorMessages.Add("Missing 'Target' on Camera_Orbit!");

                    if (!sm.CameraOrbit.gameObject.TryGetComponent(out AudioListener listener))
                        sm.CameraOrbit.gameObject.AddComponent<AudioListener>();

                }

                break;

            case LevelType.Gameplay:

                if (sm.GameplayManager) {

                    if (sm.GameplayManager.spawnPoint == null)
                        errorMessages.Add("Missing 'Spawn Point' on Gameplay Manager!");

                    if (sm.GameplayManager.spawnPoint != null && sm.GameplayManager.spawnPoint.transform.position == new Vector3(0f, 1.5f, 15f))
                        errorMessages.Add("Position of the 'Spawn Point' on Gameplay Manager is default!");

                }

                if (sm.CCDSUIManager) {

                    //if (sm.CCDSUIManager.spawnPoint == null)
                    //    errorMessages.Add("Missing 'Spawn Point' on CCDS_GameplayManager!", MessageType.Error);

                }

                if (sm.MarkerManager) {

                    if (sm.MarkerManager.allMarkers == null)
                        sm.MarkerManager.allMarkers = new List<CCDS_Marker>();

                    if (sm.MarkerManager.allMarkers != null && sm.MarkerManager.allMarkers.Count < 1)
                        errorMessages.Add("No 'Markers' on Marker Manager, add one marker at least!");

                    if (sm.MarkerManager.allMarkers != null) {

                        for (int i = 0; i < sm.MarkerManager.allMarkers.Count; i++) {

                            if (sm.MarkerManager.allMarkers[i] == null)
                                errorMessages.Add("Missing a marker on Marker Manager!");

                            if (sm.MarkerManager.allMarkers[i].connectedMission == null)
                                errorMessages.Add("Missing connected mission on a marker on Marker Manager!");

                            if (sm.MarkerManager.allMarkers[i] != null && sm.MarkerManager.allMarkers[i].connectedMission && sm.MarkerManager.allMarkers[i].connectedMission == null)
                                errorMessages.Add("Missing 'Mission Objective' on the marker named " + sm.MarkerManager.allMarkers[i].transform.name + "!");

                            if (sm.MarkerManager.allMarkers[i] != null && sm.MarkerManager.allMarkers[i].connectedMission && sm.MarkerManager.allMarkers[i].connectedMission.transportToThisLocation == null)
                                errorMessages.Add("Missing 'Mission Position' on the marker named " + sm.MarkerManager.allMarkers[i].transform.name + "!");

                        }

                    }

                }

                if (sm.MissionManager) {

                    if (sm.MissionManager.allMissions == null)
                        sm.MissionManager.allMissions = new List<ACCDS_Mission>();

                    if (sm.MissionManager.allMissions != null && sm.MissionManager.allMissions.Count < 1)
                        errorMessages.Add("No 'Missions' on Mission Manager, add one mission at least!");

                    if (sm.MissionManager.allMissions != null) {

                        bool missingMission = false;

                        for (int i = 0; i < sm.MissionManager.allMissions.Count; i++) {

                            if (sm.MissionManager.allMissions[i] == null)
                                missingMission = true;

                        }

                        if (missingMission)
                            errorMessages.Add("Missing 'Missions' on Mission Manager!");

                    }

                }

                if (sm.MissionPositions) {

                    if (sm.MissionPositions.allPositions == null)
                        sm.MissionPositions.allPositions = new List<CCDS_MissionObjectivePosition>();

                    if (sm.MissionPositions.allPositions != null && sm.MissionPositions.allPositions.Count < 1)
                        errorMessages.Add("No 'Positions' on Mission Positions Manager, add one position at least!");

                    if (sm.MissionPositions.allPositions != null) {

                        bool missingPosition = false;

                        for (int i = 0; i < sm.MissionPositions.allPositions.Count; i++) {

                            if (sm.MissionPositions.allPositions[i] == null)
                                missingPosition = true;

                        }

                        if (missingPosition)
                            errorMessages.Add("Missing 'Position' on Mission Positions Manager!");

                    }

                }

                if (sm.MinimapManager) {

                    if (sm.MinimapManager.MinimapCamera == null)
                        errorMessages.Add("Missing 'Minimap Camera' on Minimap Manager!");

                }

                if (sm.CopsManager) {

                    if (sm.CopsManager.allCops == null)
                        sm.CopsManager.allCops = new List<CCDS_AI_Cop>();

                    bool missingCopFound = false;

                    for (int i = 0; i < sm.CopsManager.allCops.Count; i++) {

                        if (sm.CopsManager.allCops[i] == null)
                            missingCopFound = true;

                    }

                    if (missingCopFound)
                        errorMessages.Add("Missing 'Cop' on Cops Manager!");

                }

                break;

        }

        Camera[] cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);

        bool multipleCamerasFound = false;

        if (cameras != null && cameras.Length > 1) {

            for (int i = 0; i < cameras.Length; i++) {

                if (cameras[i].GetComponentInParent<CCDS_Minimap_Camera>() == false && cameras[i].GetComponentInParent<RCCP_Camera>() == false && cameras[i].GetComponentInParent<CCDS_Camera_Orbit>() == false && cameras[i].GetComponentInParent<RCCP_CinematicCamera>() == false && cameras[i].GetComponentInParent<RCCP_FixedCamera>() == false)
                    multipleCamerasFound = true;

            }

        }

        if (multipleCamerasFound)
            errorMessages.Add("Multiple cameras found in the scene, be sure to delete other cameras!");

        AudioListener[] audioListeners = FindObjectsByType<AudioListener>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        bool multipleAudioListenersFound = false;

        if (audioListeners != null && audioListeners.Length > 1)
            multipleAudioListenersFound = true;

        if (multipleAudioListenersFound)
            errorMessages.Add("Multiple audiolisteners found in the scene, be sure to delete other audiolisteners!");

        return errorMessages.ToArray();

    }

    public string[] CheckErrors() {

        if (CheckMisconfigurations().Length > 0)
            return CheckMisconfigurations();
        else
            return null;

    }

}
