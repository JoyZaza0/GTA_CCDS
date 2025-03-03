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
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages and observes all managers on the scene. Green buttons are meaning that manager is active in the scene and you can select it. Red buttons are meaning that manager doesn't exist in the scene, you can create it by simply clicking on it. New managers can't be created at runtime.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Managers/CCDS Gameplay Manager")]
public class CCDS_GameplayManager : ACCDS_Manager {

    private static CCDS_GameplayManager instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static CCDS_GameplayManager Instance {

        get {

            if (instance == null)
                instance = FindFirstObjectByType<CCDS_GameplayManager>();

            return instance;

        }

    }

    /// <summary>
    /// Current game state.
    /// </summary>
    [Tooltip("Sets controllable state of the player vehicle depending on the game state.")] public CCDS_GameStates.GameState gameState = CCDS_GameStates.GameState.Stopped;

    /// <summary>
    /// Old game state used to detect state changes at runtime.
    /// </summary>
    private CCDS_GameStates.GameState gameState_Old = CCDS_GameStates.GameState.Stopped;

    /// <summary>
    /// Currently on a mission right now?
    /// </summary>
    public bool OnMission {

        get {

            if (currentMission != null && defaultMission != null) {

                if (currentMission == defaultMission)
                    return false;
                else
                    return true;

            } else {

                return false;

            }

        }

    }

    /// <summary>
    /// Player.
    /// </summary>
    [Tooltip("Actual player.")] public CCDS_Player player;

    /// <summary>
    /// Spawn point.
    /// </summary>
    [Tooltip("Spawn point of the player vehicle.")] public CCDS_SpawnPoint spawnPoint;

    /// <summary>
    /// Latest selected vehicle index.
    /// </summary>
    [Tooltip("Last selected player vehicle index.")] public int lastSelectedVehicleIndex;

    /// <summary>
    /// Time limitation used on missions.
    /// </summary>
    [Tooltip("Time limitation used on missions.")] public float timeLimit = -1f;

    /// <summary>
    /// Countdown to start the game.
    /// </summary>
    [Tooltip("Countdown to start the game.")] [Range(0, 10)] public int countdownToStart = 3;

    /// <summary>
    /// Time of the day.
    /// </summary>
    [Tooltip("Time of the day.")] public bool isDay = true;

    /// <summary>
    /// Time since game start.
    /// </summary>
    [Tooltip("Time since game start.")] public float timeSinceGameStart = 0f;

    /// <summary>
    /// Current mission.
    /// </summary>
    [Tooltip("Current mission.")] public ACCDS_Mission currentMission;

    /// <summary>
    /// Default mission.
    /// </summary>
	private ACCDS_Mission defaultMission;
	public BCG_EnterExitPlayer playerCharacer;

	private float oldScore = 0f;
	//private bool usePlayerCharacer;
	//public bool UsePlayerCharacer;

    private void Awake() {

        //  Make sure game is running now.
        CCDS.ResumeGame();

        //  Getting latest selected vehicle index.
        lastSelectedVehicleIndex = CCDS.GetVehicle();

        //  Setting default mission.
        defaultMission = CCDS_Settings.Instance.defaultMission;
        currentMission = defaultMission;

        //  Getting last state of the gamestate to detect when it changes.
        gameState_Old = gameState;

    }

	private void Start() {
		
		SpawnPlayerCharacer();
	    
        //  Spawning the player vehicle.
		SpawnPlayer();
		
        //  Starting the game with countdown.
        StartMission(countdownToStart);

        //  Set headlights of all vehicles depending on the isDay.
        Invoke(nameof(CheckIsDay), 1.5f);

        InvokeRepeating(nameof(SaveMoneyInterval), 1f, 1f);
		
    }

    private void OnEnable() {

        CCDS_Events.OnLocalPlayerBusted += CCDS_Events_OnLocalPlayerBusted;
        CCDS_Events.OnLocalPlayerReleased += CCDS_Events_OnLocalPlayerReleased;
        CCDS_Events.OnLocalPlayerWrecked += CCDS_Events_OnLocalPlayerWrecked;

    }

    private void CCDS_Events_OnLocalPlayerWrecked(CCDS_Player player) {

        CCDS_UI_Manager.Instance.PlayerWrecked(player);

    }

    private void CCDS_Events_OnLocalPlayerReleased(CCDS_Player player) {

        CCDS_UI_Manager.Instance.PlayerReleased(player);

    }

    private void CCDS_Events_OnLocalPlayerBusted(CCDS_Player player) {

        CCDS_UI_Manager.Instance.PlayerBusted(player);

    }

    /// <summary>
    /// Checks isDay and enables / disables headlights of all vehicles.
    /// </summary>
    private void CheckIsDay() {

        //  Getting all RCCP vehicles and enabling / disabling the low beam headlights if they have.
        RCCP_CarController[] allVehicles = RCCP_SceneManager.Instance.allVehicles.ToArray();

        if (allVehicles != null && allVehicles.Length > 0) {

            for (int i = 0; i < allVehicles.Length; i++) {

                if (allVehicles[i] != null && allVehicles[i].Lights != null)
                    allVehicles[i].Lights.lowBeamHeadlights = !isDay;

            }

        }

        //  Getting all RTC vehicles and enabling / disabling the low beam headlights if they have.
        //RTC_CarController[] allTraffic = RTC_SceneManager.Instance.allVehicles;

        //if (allTraffic != null && allTraffic.Length > 0) {

        //    for (int i = 0; i < allTraffic.Length; i++) {

        //        if (allTraffic[i] != null)
        //            allTraffic[i].isNight = !isDay;

        //    }

        //}

    }

    private void SaveMoneyInterval() {

        if (!player)
            return;

        if (oldScore != player.Score)
            CCDS.ChangeMoney((int)(player.Score - oldScore));

        oldScore = player.Score;

    }

    private void Update() {

        //  Calling an event when the game state has been changed.
        if (gameState_Old != gameState) {

            //  Setting canControl bool of the vehicle with the game state changes.
            switch (gameState) {

                case CCDS_GameStates.GameState.Stopped:

                    if (player)
                        player.CarController.canControl = false;

                    break;

                case CCDS_GameStates.GameState.Countdown:

                    if (player)
                        player.CarController.canControl = false;

                    break;

                case CCDS_GameStates.GameState.Paused:

                    if (player)
                        player.CarController.canControl = false;

                    break;

                case CCDS_GameStates.GameState.Started:

                    if (player)
                        player.CarController.canControl = true;

                    break;

            }

            CCDS_Events.Event_GameStateChanged(gameState);

        }

        gameState_Old = gameState;

        //  Consuming the time if on mission. If timer hits to 0, mission completed.
        if (gameState == CCDS_GameStates.GameState.Started) {

            if (timeLimit != -1 && timeLimit > 0)
                timeLimit -= Time.deltaTime;

            if (timeLimit != -1 && timeLimit < 0) {

                timeLimit = -1f;

                if (OnMission)
                    MissionCompleted(false);

            }

            //  Time since game start. Only if game state is started.
            timeSinceGameStart += Time.deltaTime;

        }

        //  Time since game start can't be a minus value.
        if (timeSinceGameStart < 0)
            timeSinceGameStart = 0f;

    }
    
	public void SpawnPlayerCharacer()
	{
		if (playerCharacer) 
		{
			Debug.LogError("Scene already includes player characer named  Destroying it!");
			Destroy(playerCharacer.gameObject);
		}
		
		Vector3 playerPosition = CCDS.GetPlayerCharacterPosition();
		
		playerCharacer = Instantiate(Resources.Load<BCG_EnterExitPlayer>("Player"));
		
		if(playerPosition != Vector3.zero)
		{
			playerCharacer.transform.position = playerPosition;
		}
		else
		{
			var defaultPosition = GameObject.FindGameObjectWithTag("DefaultPlayerPosition").transform;
			
			if(defaultPosition)
			{
				playerCharacer.transform.SetPositionAndRotation(defaultPosition.position,defaultPosition.rotation);
			}
		}
	}
	
    /// <summary>
    /// Spawns the player vehicle with latest selected vehicle index.
    /// </summary>
    public void SpawnPlayer() {

        //  If there is a player already spawned before, return.
        if (player) {

            Debug.LogError("Scene already includes player vehicle named " + player.name + ", Destroying it!");
            Destroy(player.gameObject);

        }

        //  If spawn point couldn't found, inform and create.
	    //if (spawnPoint == null) {

	        // Debug.LogError("Spawn point couldn't found, creating it at vector3 zero. Be sure to create a spawn point and assign it in the CCDS_GameplayManager!");

	        //spawnPoint = new GameObject("CCDS_SpawnPoint").AddComponent<CCDS_SpawnPoint>();
            //spawnPoint.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            //spawnPoint.transform.position += Vector3.up * 1.5f;
	        //spawnPoint.transform.position += Vector3.forward * 15f;
			
	        
	    // }
	   
	    Vector3 playerPosition = CCDS.GetPlayerCharacterPosition();
	    
	    if(playerPosition != Vector3.zero)
	    {
		    SpawnPointManager spawnPointManager = FindAnyObjectByType<SpawnPointManager>();
	        
		    if(spawnPointManager)
		    {
			    spawnPoint = spawnPointManager.GetClosestPoint(playerCharacer.transform);
		    }
	    }
	
	    
        //  Spawning RCCP vehicle with controllable state.
	    // player = RCCP.SpawnRCC(CCDS_PlayerVehicles.Instance.playerVehicles[lastSelectedVehicleIndex].vehicle, spawnPoint.transform.position, spawnPoint.transform.rotation, true, true, CCDS_Settings.Instance.startEngineAtStart).GetComponent<CCDS_Player>();
	    player = Instantiate(CCDS_PlayerVehicles.Instance.playerVehicles[lastSelectedVehicleIndex].vehicle, spawnPoint.transform.position, spawnPoint.transform.rotation).GetComponent<CCDS_Player>();
	    player.CarController.SetCanControl(false);
	    player.CarController.KillEngine();
	    
        //  Setting headlight of the vehicle.
        if (player.CarController.Lights != null)
            player.CarController.Lights.lowBeamHeadlights = !isDay;
        else
            Debug.LogWarning("Lights couldn't found on this player vehicle named " + player.transform.name + ", please add lights component through the RCCP_CarController!");

        if (player.CarController.Customizer != null) {

            player.CarController.Customizer.Load();
            player.CarController.Customizer.Initialize();

        } else {

            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + player.transform.name + ", please add customizer component through the RCCP_CarController!");

        }

        //  Calling an event when player spawned.
        CCDS_Events.Event_OnLocalPlayerSpawned(player);

    }

    /// <summary>
    /// When player enters the mission marker. Triggered by the marker.
    /// </summary>
    /// <param name="marker"></param>
    public void EnteredMarker(CCDS_Marker marker) {

        if (marker.connectedMission == null) {

            Debug.LogError("Mission objective is missing on the marker named : " + marker.transform.name + ", please create a new mission objective or assign a mission objective for this marker!");
            return;

        }

        //  Setting current mission.
        currentMission = marker.connectedMission;

        //  If mission has target location to transport the player, transport the player vehicle to that location.
        if (currentMission.transportToThisLocation)
            RCCP.Transport(player.CarController, currentMission.transportToThisLocation.transform.position, currentMission.transportToThisLocation.transform.rotation);

        //  Enabling the mission objective.
        currentMission.gameObject.SetActive(true);

        //  If mission has target gameobjects to enable, enable them.
        if (currentMission.gameobjectsToEnable != null && currentMission.gameobjectsToEnable.Length > 0) {

            for (int i = 0; i < currentMission.gameobjectsToEnable.Length; i++) {

                if (currentMission.gameobjectsToEnable[i] != null)
                    currentMission.gameobjectsToEnable[i].SetActive(true);

            }

        }

        //  If mission has target gameobjects to disable, disable them.
        if (currentMission.gameobjectsToDisable != null && currentMission.gameobjectsToDisable.Length > 0) {

            for (int i = 0; i < currentMission.gameobjectsToDisable.Length; i++) {

                if (currentMission.gameobjectsToDisable[i] != null)
                    currentMission.gameobjectsToDisable[i].SetActive(false);

            }

        }

        //  If mission has start mission instant option, start the mission instantly. Otherwise, start with countdown.
        if (currentMission.startMissionInstantly)
            StartMission(0f);
        else
            StartMission(currentMission.countDown);

        //  If mission has start info, display it.
        if (currentMission.misssionStartInfo != "")
            CCDS_UI_Informer.Instance.Info(currentMission.misssionStartInfo);

        //  Disabling the traffic for a while to prevent clipping with other traffic vehicles at spawned position.
	    //CCDS.DisableTrafficForAWhile();

        //  Calling an event when mission changes.
        CCDS_Events.Event_OnMissionChanged();

    }
    
    
	public void EnteredMarker(ACCDS_Mission mission) {

		if (mission == null) {

			Debug.LogError("Mission objective is missing on the marker named : " + mission.transform.name + ", please create a new mission objective or assign a mission objective for this marker!");
            return;

        }

        //  Setting current mission.
		currentMission = mission;

        //  If mission has target location to transport the player, transport the player vehicle to that location.
        if (currentMission.transportToThisLocation)
            RCCP.Transport(player.CarController, currentMission.transportToThisLocation.transform.position, currentMission.transportToThisLocation.transform.rotation);

        //  Enabling the mission objective.
        currentMission.gameObject.SetActive(true);

        //  If mission has target gameobjects to enable, enable them.
        if (currentMission.gameobjectsToEnable != null && currentMission.gameobjectsToEnable.Length > 0) {

            for (int i = 0; i < currentMission.gameobjectsToEnable.Length; i++) {

                if (currentMission.gameobjectsToEnable[i] != null)
                    currentMission.gameobjectsToEnable[i].SetActive(true);

            }

        }

        //  If mission has target gameobjects to disable, disable them.
        if (currentMission.gameobjectsToDisable != null && currentMission.gameobjectsToDisable.Length > 0) {

            for (int i = 0; i < currentMission.gameobjectsToDisable.Length; i++) {

                if (currentMission.gameobjectsToDisable[i] != null)
                    currentMission.gameobjectsToDisable[i].SetActive(false);

            }

        }

        //  If mission has start mission instant option, start the mission instantly. Otherwise, start with countdown.
        if (currentMission.startMissionInstantly)
            StartMission(0f);
        else
            StartMission(currentMission.countDown);

        //  If mission has start info, display it.
        if (currentMission.misssionStartInfo != "")
            CCDS_UI_Informer.Instance.Info(currentMission.misssionStartInfo);

        //  Disabling the traffic for a while to prevent clipping with other traffic vehicles at spawned position.
		// CCDS.DisableTrafficForAWhile();

        //  Calling an event when mission changes.
        CCDS_Events.Event_OnMissionChanged();

    }

    /// <summary>
    /// Starts the current mission.
    /// </summary>
    /// <param name="countDown"></param>
    public void StartMission(float countDown) {

        StartCoroutine(StartModeWithCountDown(countDown));

    }

    /// <summary>
    /// Starts the current mission with countdown in seconds.
    /// </summary>
    /// <param name="countDown"></param>
    /// <returns></returns>
    private IEnumerator StartModeWithCountDown(float countDown) {

        if (currentMission.timeLimited)
            timeLimit = currentMission.time;
        else
            timeLimit = -1f;

        gameState = CCDS_GameStates.GameState.Countdown;
        CCDS_Events.Event_OnMissionCountdownStarted();

        yield return new WaitForSeconds(countDown);

        gameState = CCDS_GameStates.GameState.Started;
        CCDS_Events.Event_OnMissionStarted();

    }

    /// <summary>
    /// When mission is over. If success, reward the player.
    /// </summary>
    /// <param name="success"></param>
    public void MissionCompleted(bool success) {

        //  If success...
	    if (success) { 
        	
            //  If mission has completed info, display it with rewarded amount.
            if (currentMission.missionCompletedInfo != "") {
				
	            if(currentMission.uiMode == MissionUIMode.None)
	            {
		            if (currentMission.rewardPlayer)
			            CCDS_UI_Informer.Instance.Info(currentMission.missionCompletedInfo + "\nRewarded: $ " + currentMission.reward.ToString() + "!");
		            else
			            CCDS_UI_Informer.Instance.Info(currentMission.missionCompletedInfo);
	            }
	            else if(currentMission.uiMode == MissionUIMode.WithPopup)
	            {
	            	if(currentMission.rewardPlayer)
	            	{
	            		string info = currentMission.missionCompletedInfo + "\nRewarded: $ " + currentMission.reward.ToString() + "!";
	                 
		            	
	            		if(!CCDS_MissionObjectiveManager.Instance.IsLastMission(currentMission))
	            		{
		            		CCDS_MissionObjectiveManager.Instance.LevelUpMission(currentMission);

		            		CCDS_UI_Informer.Instance.OpenVictoryPopup(info,true,()=> 
		            		{		            		
			            		EnteredMarker(CCDS_MissionObjectiveManager.Instance.GetCurrentMission());
			            		CCDS_UI_Informer.Instance.CloseVictoryPopup();
		            		});
	            		}
	            		else
	            		{	            			
		            		CCDS_UI_Informer.Instance.OpenVictoryPopup(info,false);
	            		}
	            	}
	            }
            }
            
		

            //  If mission has rewardPlayer option enabled, reward the player with money.
            if (currentMission.rewardPlayer)
	            CCDS.ChangeMoney(currentMission.reward);
                
	        CCDS_Events.Event_OnMissionVictory(currentMission);

        } else {

            //  If mission has failed info, display it.
	        if (currentMission.missionFailedInfo != "")
	        {
	        	if(currentMission.uiMode == MissionUIMode.None)
	        	{
		        	CCDS_UI_Informer.Instance.Info(currentMission.missionFailedInfo);	        		
	        	}
	        	else if(currentMission.uiMode == MissionUIMode.WithPopup)
	        	{
	        		CCDS_UI_Informer.Instance.OpenFailedPopup(currentMission.missionFailedInfo,() => 
	        		{
	        			EnteredMarker(CCDS_MissionObjectiveManager.Instance.GetCurrentMission());
	        			CCDS_UI_Informer.Instance.CloseFailedPopup();
	        		});
	        	}
	        }

        }

        //  Disabling the mission objective.
        currentMission.gameObject.SetActive(false);

        //  If mission has target gameobjects to disable, disable them.
        if (currentMission.gameobjectsToEnable != null && currentMission.gameobjectsToEnable.Length > 0) {

            for (int i = 0; i < currentMission.gameobjectsToEnable.Length; i++) {

                if (currentMission.gameobjectsToEnable[i] != null)
                    currentMission.gameobjectsToEnable[i].SetActive(false);

            }

        }

        //  If mission has target gameobjects to enable, enable them.
        if (currentMission.gameobjectsToDisable != null && currentMission.gameobjectsToDisable.Length > 0) {

            for (int i = 0; i < currentMission.gameobjectsToDisable.Length; i++) {

                if (currentMission.gameobjectsToDisable[i] != null)
                    currentMission.gameobjectsToDisable[i].SetActive(true);

            }

        }

        //  Setting current mission back to the default mission.
        currentMission = defaultMission;

        //  Setting lime limit to -1. It means we don't have any time limit.
        timeLimit = -1f;

        //  Calling an event when mission completed.
        CCDS_Events.Event_OnMissionCompleted();

    }

    /// <summary>
    /// Adds additional time to the time limit.
    /// </summary>
    /// <param name="seconds"></param>
    public void AddTime(int seconds) {

        timeLimit += seconds;

        if (seconds < 2f)
            CCDS_UI_Informer.Instance.Info("Added " + seconds.ToString() + " second!");
        else
            CCDS_UI_Informer.Instance.Info("Added " + seconds.ToString() + " seconds!");

    }

    /// <summary>
    /// Pays the fine to be free.
    /// </summary>
    public void PayFineToBeFree() {

        CCDS.ChangeMoney(-(int)player.policeFineMoney);
        player.ReleaseFromBusted();

    }

    private void OnDisable() {

        CCDS_Events.OnLocalPlayerBusted -= CCDS_Events_OnLocalPlayerBusted;
        CCDS_Events.OnLocalPlayerReleased -= CCDS_Events_OnLocalPlayerReleased;
        CCDS_Events.OnLocalPlayerWrecked -= CCDS_Events_OnLocalPlayerWrecked;

    }

    private void Reset() {

        if (spawnPoint == null) {

            spawnPoint = new GameObject("CCDS_SpawnPoint").AddComponent<CCDS_SpawnPoint>();
            spawnPoint.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            spawnPoint.transform.position += Vector3.up * 1.5f;
            spawnPoint.transform.position += Vector3.forward * 15f;

        }

    }

}
