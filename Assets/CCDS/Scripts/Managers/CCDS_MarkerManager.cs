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
/// Marker manager and observer in the scene.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Managers/CCDS Marker Manager")]
public class CCDS_MarkerManager : ACCDS_Manager {

    private static CCDS_MarkerManager instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static CCDS_MarkerManager Instance {

        get {

            if (instance == null)
                instance = FindFirstObjectByType<CCDS_MarkerManager>();

            return instance;

        }

    }

    /// <summary>
    /// Markers.
    /// </summary>
    public List<CCDS_Marker> allMarkers = new List<CCDS_Marker>();

    private void Awake() {

        //  Getting all markers, even if they are disabled.
	    GetAllMarkers();

    }
    
	private void Start()
	{
		SetAllMarkers();
	}

    private void OnEnable() {

        //  Listening events when mission changes or mission complete on enable.
        CCDS_Events.OnMissionChanged += CCDS_Events_OnMissionChanged;
	    CCDS_Events.OnMissionCompleted += CCDS_Events_OnMissionCompleted;

    }

    private void OnDisable() {

        //  Unregistering the listeners on disable.
        CCDS_Events.OnMissionChanged -= CCDS_Events_OnMissionChanged;
	    CCDS_Events.OnMissionCompleted -= CCDS_Events_OnMissionCompleted;

    }
    

    /// <summary>
    /// When mission changed.
    /// </summary>
    /// <param name="gameMode"></param>
    private void CCDS_Events_OnMissionChanged() {

        CheckMarkers();

    }

    /// <summary>
    /// When mission completed.
    /// </summary>
    private void CCDS_Events_OnMissionCompleted() {

	    CheckMarkers();
	    SetAllMarkers();
    }

    /// <summary>
    /// Gets all markers.
    /// </summary>
    public void GetAllMarkers() {

        //  Checking the list. Creating if it's null.
        if (allMarkers == null)
            allMarkers = new List<CCDS_Marker>();

        //  Clearing the list.
        allMarkers.Clear();

        //  Getting all markers, even if they are disabled.
        allMarkers = GetComponentsInChildren<CCDS_Marker>(true).ToList();

    }
    

    /// <summary>
    /// Checking the markers. Enabling them if player is not on any mission, disabling them if player is on a mission.
    /// </summary>
    public void CheckMarkers() {

        //  Return if gameplay manager couldn't found.
        if (!CCDS_GameplayManager.Instance)
            return;

        //  Disable all markers on mission. Otherwise, enable all.
        if (!CCDS_GameplayManager.Instance.OnMission)
            ActivateAllMarkers();
        else
            DeactivateAllMarkers();

    }

    /// <summary>
    /// Deactivates all markers.
    /// </summary>
    public void DeactivateAllMarkers() {

        for (int i = 0; i < allMarkers.Count; i++) {

            if (allMarkers[i] != null) {

                if (allMarkers[i].gameObject.activeSelf)
                    allMarkers[i].gameObject.SetActive(false);

            }

        }

    }

    /// <summary>
    /// Enables all markers.
    /// </summary>
    public void ActivateAllMarkers() {

        for (int i = 0; i < allMarkers.Count; i++) {

            if (allMarkers[i] != null) {

                if (!allMarkers[i].gameObject.activeSelf) {

	                if (allMarkers[i].connectedMission.reenableMarkerInSeconds > 0)
	                {
		                StartCoroutine(ActivateMarkerDelayed(allMarkers[i], allMarkers[i].connectedMission.reenableMarkerInSeconds));
	                	
	                }
                    else if (allMarkers[i].connectedMission.reenableMarkerAfterMission)
                    {
                    	allMarkers[i].gameObject.SetActive(true);                  	
                    }
                }

            }

        }

    }

    /// <summary>
    /// Enables all markers with delay.
    /// </summary>
    public IEnumerator ActivateMarkerDelayed(CCDS_Marker marker, float delay) {

        yield return new WaitForSeconds(delay);
	    marker.gameObject.SetActive(true);
    }

    /// <summary>
    /// Adds a new marker to the list.
    /// </summary>
    public void AddNewMarker(CCDS_Marker newMarker) {

        allMarkers.Add(newMarker);

    }

    private void Reset() {

        GetAllMarkers();

    }
    
	private void SetAllMarkers()
	{
		var checkpointMarkers = allMarkers.Where(marker => marker.missionMode == CCDS_GameModes.Mode.Checkpoint).ToArray();
		var trailblazerMarkers = allMarkers.Where(marker => marker.missionMode == CCDS_GameModes.Mode.Trailblazer).ToArray();
		var raceMarkers = allMarkers.Where(marker => marker.missionMode == CCDS_GameModes.Mode.Race).ToArray();
		
		for (int i = 0; i < checkpointMarkers.Length; i++)
		{
			checkpointMarkers[i].connectedMission = CCDS_MissionObjectiveManager.Instance.GetCheckpointMission();
			
			string title = CCDS_MissionObjectiveManager.Instance.GetMissionTitle(checkpointMarkers[i].connectedMission);
			
			if(title != null)
				checkpointMarkers[i].lable.text = title;
		}
		
		for (int i = 0; i < trailblazerMarkers.Length; i++) 
		{
			trailblazerMarkers[i].connectedMission = CCDS_MissionObjectiveManager.Instance.GetTrailblazerMission();
			
			string title = CCDS_MissionObjectiveManager.Instance.GetMissionTitle(trailblazerMarkers[i].connectedMission);
			
			if(title != null)
				trailblazerMarkers[i].lable.text = title;
		}
		
		for (int i = 0; i < raceMarkers.Length; i++) 
		{
			raceMarkers[i].connectedMission = CCDS_MissionObjectiveManager.Instance.GetRaceMission();
			
			string title = CCDS_MissionObjectiveManager.Instance.GetMissionTitle(raceMarkers[i].connectedMission);
			
			if(title != null)
				raceMarkers[i].lable.text = title;
		}		
		
	}

}
