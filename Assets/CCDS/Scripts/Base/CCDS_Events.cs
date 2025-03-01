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
/// CCDS Events.
/// </summary>
public class CCDS_Events {

    /// <summary>
    /// When player money changed.
    /// </summary>
    public delegate void onMoneyChanged();
    public static event onMoneyChanged OnMoneyChanged;

    /// <summary>
    /// When player vehicle changed in the main menu.
    /// </summary>
    public delegate void onVehicleChangedMM();
    public static event onVehicleChangedMM OnVehicleChangedMM;

    /// <summary>
    /// When gamestate changed.
    /// </summary>
    public static event onGameStateChanged OnGameStateChanged;
    public delegate void onGameStateChanged(CCDS_GameStates.GameState gameState);

    /// <summary>
    /// When mission started.
    /// </summary>
    public static event onMissionStarted OnMissionStarted;
    public delegate void onMissionStarted();

    /// <summary>
    /// When mission countdown started.
    /// </summary>
    public static event onMissionCountdownStarted OnMissionCountdownStarted;
    public delegate void onMissionCountdownStarted();

    /// <summary>
    /// When mission changed.
    /// </summary>
    public static event onMissionChanged OnMissionChanged;
    public delegate void onMissionChanged();

    /// <summary>
    /// When mission completed.
    /// </summary>
    public static event onMissionCompleted OnMissionCompleted;
    public delegate void onMissionCompleted();

    /// <summary>
    /// When player spawned.
    /// </summary>
    public static event onPlayerSpawned OnLocalPlayerSpawned;
    public delegate void onPlayerSpawned(CCDS_Player player);

    /// <summary>
    /// When player dies.
    /// </summary>
    public static event onPlayerDied OnLocalPlayerWrecked;
    public delegate void onPlayerDied(CCDS_Player player);

    /// <summary>
    /// When player busted.
    /// </summary>
    public static event onPlayerBusted OnLocalPlayerBusted;
    public delegate void onPlayerBusted(CCDS_Player player);

    /// <summary>
    /// When player released.
    /// </summary>
    public static event onPlayerReleased OnLocalPlayerReleased;
    public delegate void onPlayerReleased(CCDS_Player player);

    /// <summary>
    /// When player on collision.
    /// </summary>
    public static event onCollision OnCollision;
    public delegate void onCollision(float impulse);

    /// <summary>
    /// When quality changed.
    /// </summary>
    public static event onQualityChanged OnQualityChanged;
    public delegate void onQualityChanged();

    /// <summary>
    /// When audio changed.
    /// </summary>
    public static event onAudioChanged OnAudioChanged;
    public delegate void onAudioChanged();

    /// <summary>
    /// When game paused.
    /// </summary>
    public static event onPaused OnPaused;
    public delegate void onPaused();

    /// <summary>
    /// When game resumed.
    /// </summary>
    public static event onResumed OnResumed;
    public delegate void onResumed();

    /// <summary>
    /// When getting back to the main menu.
    /// </summary>
    public static event onMainMenu OnMainMenu;
	public delegate void onMainMenu();
    
	public static event onMissionVictory OnMissionVitory;
	public delegate void onMissionVictory(ACCDS_Mission mission);


    public static void Event_GameStateChanged(CCDS_GameStates.GameState gameState) {

        if (OnGameStateChanged != null)
            OnGameStateChanged(gameState);

        Debug.Log("Game state has been changed to: " + gameState.ToString() + ".");

    }

    public static void Event_OnMissionStarted() {

        if (OnMissionStarted != null)
            OnMissionStarted();

        Debug.Log("Mission has been started.");

    }

    public static void Event_OnMissionCountdownStarted() {

        if (OnMissionCountdownStarted != null)
            OnMissionCountdownStarted();

        Debug.Log("Mission has been countdowned.");

    }

    public static void Event_OnMissionChanged() {

        if (OnMissionChanged != null)
            OnMissionChanged();

        Debug.Log("Mission has been changed.");

    }

    public static void Event_OnMissionCompleted() {

        if (OnMissionCompleted != null)
            OnMissionCompleted();

        Debug.Log("Mission has been completed.");

    }

    public static void Event_OnLocalPlayerSpawned(CCDS_Player player) {

        if (OnLocalPlayerSpawned != null)
            OnLocalPlayerSpawned(player);

        Debug.Log("Player named " + player.transform.name + " spawned.");

    }

    public static void Event_OnLocalPlayerWrecked(CCDS_Player player) {

        if (OnLocalPlayerWrecked != null)
            OnLocalPlayerWrecked(player);

        Debug.Log("Player named " + player.transform.name + " wrecked.");

    }

    public static void Event_OnLocalPlayerBusted(CCDS_Player player) {

        if (OnLocalPlayerBusted != null)
            OnLocalPlayerBusted(player);

        Debug.Log("Player named " + player.transform.name + " busted.");

    }

    public static void Event_OnLocalPlayerReleased(CCDS_Player player) {

        if (OnLocalPlayerReleased != null)
            OnLocalPlayerReleased(player);

        Debug.Log("Player named " + player.transform.name + " released.");

    }

    public static void Event_OnCollision(float impulse) {

        if (OnCollision != null)
            OnCollision(impulse);

        Debug.Log("Player collided.");

    }

    public static void Event_OnQualityChanged() {

        if (OnQualityChanged != null)
            OnQualityChanged();

        Debug.Log("Quality has been changed.");

    }

    public static void Event_OnAudioChanged() {

        if (OnAudioChanged != null)
            OnAudioChanged();

        Debug.Log("Audio has been changed.");

    }

    public static void Event_OnPaused() {

        if (OnPaused != null)
            OnPaused();

        Debug.Log("Paused the game.");

    }

    public static void Event_OnResumed() {

        if (OnResumed != null)
            OnResumed();

        Debug.Log("Resumed the game.");

    }

    public static void Event_OnMainMenu() {

        if (OnMainMenu != null)
            OnMainMenu();

        Debug.Log("Returned to the main menu.");

    }

    public static void Event_OnMoneyChanged() {

        if (OnMoneyChanged != null)
            OnMoneyChanged();

        Debug.Log("Player money changed.");

    }

    public static void Event_OnVehicleChangedMM() {

        if (OnVehicleChangedMM != null)
            OnVehicleChangedMM();

        Debug.Log("Player vehicle changed in the main menu.");

    }
    
	public static void Event_OnMissionVictory(ACCDS_Mission mission)
	{
		if(OnMissionVitory != null && mission != null)
			OnMissionVitory(mission);
			
		Debug.Log($"Mission {mission.gameObject.name} is Victory");
	}

}
