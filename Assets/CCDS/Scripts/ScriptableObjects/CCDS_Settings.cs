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

/// <summary>
/// Shared settings of the CCDS.
/// </summary>
public class CCDS_Settings : ScriptableObject {

    private static CCDS_Settings instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static CCDS_Settings Instance { get { if (instance == null) instance = Resources.Load("CCDS_Settings") as CCDS_Settings; return instance; } }

    /// <summary>
    /// Main menu scene index in the build settings.
    /// </summary>
    [Min(0)] public int mainMenuSceneIndex = 0;

    /// <summary>
    /// Default money.
    /// </summary>
    [Header("Default Settings")] [Min(0)] public int defaultMoney = 10000;

    /// <summary>
    /// Default player name.
    /// </summary>
    public string defaultPlayerName = "New Player";

    /// <summary>
    /// Default selected vehicle index.
    /// </summary>
    [Min(0)] public int defaultSelectedVehicleIndex = 0;

    /// <summary>
    /// Default audio volume.
    /// </summary>
    [Range(0f, 1f)] public float defaultAudioVolume = 1f;

    /// <summary>
    /// Default music volume.
    /// </summary>
    [Range(0f, 1f)] public float defaultMusicVolume = .65f;

    /// <summary>
    /// Starts the engine when the game starts.
    /// </summary>
    [Header("Settings")] [Space()] public bool startEngineAtStart = true;

    /// <summary>
    /// Shows an arrow indicator at top of the player vehicle during on mission.
    /// </summary>
    public bool showArrowIndicator = true;

    /// <summary>
    /// Shows minimap icons for player and opponent vehicles.
    /// </summary>
    public bool showMinimapIcons = true;

    /// <summary>
    /// Shows engine smoke for player and opponent vehicles on higher damage.
    /// </summary>
    public bool showEngineSmoke = true;

    /// <summary>
    /// Shows healthbar on opponent vehicles.
    /// </summary>
    public bool showHealthBar = true;

    /// <summary>
    /// RCCP camera.
    /// </summary>
    [Header("Resources")] public RCCP_Camera vehicleCamera;

    /// <summary>
    /// Arrow item used on the player vehicle indicating the following mission objective.
    /// </summary>
    public CCDS_ArrowIndicator arrowForPlayer;

    /// <summary>
    /// Minimap camera.
    /// </summary>
    public CCDS_Minimap_Camera minimapCamera;

    /// <summary>
    /// Minimap icon used on player vehicles.
    /// </summary>
    public CCDS_MinimapItem minimapIconForPlayerVehicle;

    /// <summary>
    /// Minimap icon used on opponent vehicles.
    /// </summary>
    public CCDS_MinimapItem minimapIconForOpponentVehicle;

    /// <summary>
    /// Minimap icon used on police vehicles.
    /// </summary>
    public CCDS_MinimapItem minimapIconForPoliceVehicle;

    /// <summary>
    /// Health bar used on opponent vehicles.
    /// </summary>
    public CCDS_HealthBar healthBar;

    /// <summary>
    /// Engine smoke used on vehicles.
    /// </summary>
    public CCDS_EngineSmoke engineSmoke;

    /// <summary>
    /// Main menu UI manager.
    /// </summary>
    public CCDS_UI_MainMenuManager mainmenuUIManager;

    /// <summary>
    /// Gameplay UI manager.
    /// </summary>
    public CCDS_UI_Manager gameplayUIManager;

    /// <summary>
    /// Marker for the marker manager.
    /// </summary>
    public CCDS_Marker marker;

    /// <summary>
    /// Siren.
    /// </summary>
    public RCCP_PoliceLights policeSiren;

    /// <summary>
    /// Siren audioclip.
    /// </summary>
    public AudioClip policeSirenAudio;

    /// <summary>
    /// Trailblazer obstacle.
    /// </summary>
    public CCDS_MissionObjective_TrailblazerItem trailBlazerObstacle;

    /// <summary>
    /// Checkpoint.
    /// </summary>
    public CCDS_MissionObjective_CheckpointItem checkpoint;

    /// <summary>
    /// Cop vehicle.
    /// </summary>
    public ACCDS_Vehicle copVehicle;

    /// <summary>
    /// Pursuit vehicle.
    /// </summary>
    public ACCDS_Vehicle pursuitVehicle;

    /// <summary>
    /// Racer vehicle.
    /// </summary>
    public ACCDS_Vehicle racerVehicle;

    /// <summary>
    /// Race finisher.
    /// </summary>
	public CCDS_MissionObjective_Race_Finisher raceFinisher;
    
    /// <summary>
    /// Default mission will be used at very begining of the gameplay.
    /// </summary>
    [Header("Default Mission")] public ACCDS_Mission defaultMission;

}
