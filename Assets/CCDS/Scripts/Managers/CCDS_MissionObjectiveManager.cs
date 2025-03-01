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
/// Mission manager. Manages and observes all mission objectives on the scene.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Managers/CCDS Mission Objective Manager")]
public class CCDS_MissionObjectiveManager : ACCDS_Manager {

    private static CCDS_MissionObjectiveManager instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static CCDS_MissionObjectiveManager Instance {

        get {

            if (instance == null)
                instance = FindFirstObjectByType<CCDS_MissionObjectiveManager>();

            return instance;

        }

    }

    /// <summary>
    /// All missions.
    /// </summary>
	public List<ACCDS_Mission> allMissions = new List<ACCDS_Mission>();
    
	private ACCDS_Mission currentMission;
    
	private List<CCDS_MissionObjective_Checkpoint> missionCheckpoints;
	private List<CCDS_MissionObjective_Trailblazer> missionTrailblazers;
	private List<CCDS_MissionObjective_Race> missionRaces;
	
	public int CurrentCheckpointIndex
	{
		get => PlayerPrefs.GetInt(CHECKPOINT_KEY,0);
		
		private set
		{
			if(value < missionCheckpoints.Count)
				PlayerPrefs.SetInt(CHECKPOINT_KEY,value);
		}
	}
	public int CurrentTrailblazerIndex
	{
		get => PlayerPrefs.GetInt(TRAILBLAZER_KEY,0);
		
		private set
		{
			if(value < missionCheckpoints.Count)
				PlayerPrefs.SetInt(TRAILBLAZER_KEY,value);
		}
	}
	public int CurrentRaceIndex 
	{
		get => PlayerPrefs.GetInt(RACE_KEY,0);
		
		private set
		{
			if(value < missionRaces.Count)
				PlayerPrefs.SetInt(RACE_KEY,value);
		}
	}
	
	private const string CHECKPOINT_KEY = "Checkpoint";
	private const string TRAILBLAZER_KEY = "Trailblazer";
	private const string RACE_KEY = "Race";
	
	
    private void Awake() {

        //  Getting all missions.
        GetAllMissions();
        
	    GetMissionsByType();
        //  Deactivating all missions before starting the game.
        DeactivateAllMissions();
        
    } 
    
	private void OnEnable()
	{
		CCDS_Events.OnMissionStarted += OnMissionStarted;
	}
	
	private void OnDisable()
	{
		CCDS_Events.OnMissionStarted -= OnMissionStarted;
	}
	
	private void OnMissionStarted()
	{
		var defaultMission = CCDS_GameplayManager.Instance.currentMission;
		
		if(defaultMission is CCDS_MissionObjective_Checkpoint)
		{
			currentMission = defaultMission as CCDS_MissionObjective_Checkpoint;
		}
		else if(defaultMission is CCDS_MissionObjective_Trailblazer)
		{
			currentMission = defaultMission as CCDS_MissionObjective_Trailblazer;
		}
		else if(defaultMission is CCDS_MissionObjective_Race)
		{
			currentMission = defaultMission as CCDS_MissionObjective_Race;
		}
	
	}

    /// <summary>
    /// Gets all missions.
    /// </summary>
    public void GetAllMissions() {

        //  Checking the list. Creating if it's null.
        if (allMissions == null)
            allMissions = new List<ACCDS_Mission>();

        //  Clearing the list.
        allMissions.Clear();

        //  Getting all missions, even if they are disabled.
        allMissions = GetComponentsInChildren<ACCDS_Mission>(true).ToList();

    }

    /// <summary>
    /// Deactivates all missions.
    /// </summary>
    public void DeactivateAllMissions() {

        //  Checking the list. Creating if it's null.
        if (allMissions == null)
            allMissions = new List<ACCDS_Mission>();

        for (int i = 0; i < allMissions.Count; i++) {

            if (allMissions[i] != null)
                allMissions[i].gameObject.SetActive(false);

        }

    }

    /// <summary>
    /// Creates new mission objective.
    /// </summary>
    /// <param name="gameMode"></param>
    /// <returns></returns>
    public ACCDS_Mission CreateNewMissionObjective(CCDS_GameModes.Mode gameMode) {

        GameObject newMissionObject = new GameObject("CCDS_Mission_" + gameMode.ToString());
        newMissionObject.transform.SetParent(transform);
        newMissionObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        switch (gameMode) {

            case CCDS_GameModes.Mode.Checkpoint:

                newMissionObject.AddComponent<CCDS_MissionObjective_Checkpoint>();

                break;

            case CCDS_GameModes.Mode.Pursuit:

                newMissionObject.AddComponent<CCDS_MissionObjective_Pursuit>();

                break;

            case CCDS_GameModes.Mode.Race:

                newMissionObject.AddComponent<CCDS_MissionObjective_Race>();

                break;

            case CCDS_GameModes.Mode.Trailblazer:

                newMissionObject.AddComponent<CCDS_MissionObjective_Trailblazer>();

	            break;
                
        }

        AddNewMission(newMissionObject.GetComponent<ACCDS_Mission>());
        return newMissionObject.GetComponent<ACCDS_Mission>();

    }

    /// <summary>
    /// Ads a new mission.
    /// </summary>
    /// <param name="newMission"></param>
    public void AddNewMission(ACCDS_Mission newMission) {

        allMissions.Add(newMission);

    }

    private void Reset() {

        if (allMissions == null)
            allMissions = new List<ACCDS_Mission>();

    }
    
	private void GetMissionsByType()
	{
		if(allMissions == null || allMissions.Count == 0) return;
		
		missionCheckpoints = allMissions.OfType<CCDS_MissionObjective_Checkpoint>().ToList();
		missionTrailblazers = allMissions.OfType<CCDS_MissionObjective_Trailblazer>().ToList();
		missionRaces = allMissions.OfType<CCDS_MissionObjective_Race>().ToList();
	}
	
	public void LevelUpMission(ACCDS_Mission mission)
	{
		if(mission is CCDS_MissionObjective_Checkpoint)
		{
			//currentMission = mission as CCDS_MissionObjective_Checkpoint;
			CurrentCheckpointIndex++;
		}
		else if(mission is CCDS_MissionObjective_Trailblazer)
		{
			//currentMission = mission as CCDS_MissionObjective_Trailblazer;
			CurrentTrailblazerIndex++;
		}
		else if(mission is CCDS_MissionObjective_Race)
		{
			//currentMission = mission as CCDS_MissionObjective_Race;
			CurrentRaceIndex++;
		}
	}
	
	
	public bool IsLastMission(ACCDS_Mission currentMission)
	{
		
		if(currentMission is CCDS_MissionObjective_Checkpoint)
		{
			if(CurrentCheckpointIndex == missionCheckpoints.Count -1)
				return true;
		}
		else if(currentMission is CCDS_MissionObjective_Trailblazer)
		{
			if(CurrentTrailblazerIndex == missionTrailblazers.Count -1)
				return  true;
		}
		else if(currentMission is CCDS_MissionObjective_Race)
		{
			if(CurrentRaceIndex == missionRaces.Count -1)
				return true;
		}		
		
		return false;
	}
	
	public ACCDS_Mission GetCurrentMission()
	{
		if(currentMission is CCDS_MissionObjective_Checkpoint)
		{
			return GetCheckpointMission();
		}
		else if(currentMission is CCDS_MissionObjective_Trailblazer)
		{
			return GetTrailblazerMission();
		}
		else if(currentMission is CCDS_MissionObjective_Race)
		{
			return GetRaceMission();
		}
		
		return null;
	}
	
	public string GetMissionTitle(ACCDS_Mission mission)
	{
		if(mission is CCDS_MissionObjective_Checkpoint)
		{
			return $"Checkpoint {CurrentCheckpointIndex + 1} / {missionCheckpoints.Count}";
		}
		else if(mission is CCDS_MissionObjective_Trailblazer)
		{
			return  $"Trailblazer {CurrentTrailblazerIndex +1} / {missionTrailblazers.Count}";
		}
		else if(mission is CCDS_MissionObjective_Race)
		{
			return $"Race {CurrentRaceIndex +1} / {missionRaces.Count}";
		}
		
		return null;
	}
	
	public CCDS_MissionObjective_Checkpoint GetCheckpointMission()
	{
		return missionCheckpoints[CurrentCheckpointIndex];
	}
	
	public CCDS_MissionObjective_Trailblazer GetTrailblazerMission()
	{
		return missionTrailblazers[CurrentTrailblazerIndex];
	}
	
	public CCDS_MissionObjective_Race GetRaceMission()
	{
		return missionRaces[CurrentRaceIndex];
	}

}
