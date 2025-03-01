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

public enum MissionUIMode {None, WithPopup}

/// <summary>
/// Abstract class for all mission classes.
/// </summary>
[SelectionBase]
public abstract class ACCDS_Mission : MonoBehaviour {

	public MissionUIMode uiMode;
    /// <summary>
    /// Starts the mission instantly.
    /// </summary>
    [Tooltip("Starts the mission immediately, without the countdown. Disable it to start the mission with delay.")] [Space()] public bool startMissionInstantly = true;

    /// <summary>
    /// Transports player vehicle to this location.
    /// </summary>
    [Tooltip("Transports the player vehicle to this location.")] public CCDS_MissionObjectivePosition transportToThisLocation;

    /// <summary>
    /// Mission start info text.
    /// </summary>
    [Tooltip("Mission start text info will be displayed.")] [Space()] public string misssionStartInfo = "Get Ready!";

    /// <summary>
    /// Mission success info text.
    /// </summary>
    [Tooltip("Mission success text info will be displayed.")] public string missionCompletedInfo = "Success!";

    /// <summary>
    /// Mission fail info text.
    /// </summary>
    [Tooltip("Mission fail text info will be displayed.")] public string missionFailedInfo = "Failed!";

    /// <summary>
    /// Gameobjects to enable when the mission starts.
    /// </summary>
    [Tooltip("Gameobjects to enable when the mission starts.")] [Space()] public GameObject[] gameobjectsToEnable;

    /// <summary>
    /// Gameobjects to disable when the mission ends.
    /// </summary>
    [Tooltip("Gameobjects to disable when the mission starts.")] public GameObject[] gameobjectsToDisable;

    /// <summary>
    /// Re-enables this marker after mission ends.
    /// </summary>
    [Tooltip("Re-enables this marker after mission ends.")] [Space()] public bool reenableMarkerAfterMission = true;

    /// <summary>
    /// Re-enables this marker after mission ends (after delay).
    /// </summary>
    [Tooltip("Re-enables this marker after mission ends (after delay).")] [Range(0, 60)] public float reenableMarkerInSeconds = 0;

    /// <summary>
    /// Countdown timer before the mission starts.
    /// </summary>
    [Tooltip("Countdown timer before the mission starts.")] [Space()] [Range(0, 10)] public int countDown = 3;

    /// <summary>
    /// Time limited mission.
    /// </summary>
    [Tooltip("Time limited mission.")] [Space()] public bool timeLimited = false;

    /// <summary>
    /// Limited time.
    /// </summary>
    [Tooltip("Limited time.")] public float time = 60f;

    /// <summary>
    /// Reward player if the mission succeeded.
    /// </summary>
    [Tooltip("Reward player if the mission succeeded.")] [Space()] public bool rewardPlayer = true;
    [Tooltip("Reward player if the mission succeeded.")] public int reward = 10000;

    /// <summary>
    /// Percentage of completing the mission.
    /// </summary>
    internal float percentage = -1f;

    /// <summary>
    /// Percentage over this value.
    /// </summary>
    internal float percentageOver = 100f;

    /// <summary>
    /// Position of the target for the mission.
    /// </summary>
    internal Vector3 currentTarget = Vector3.zero;

    private void Reset() {

        switch (this.GetType().Name) {

            case "CCDS_MissionObjective_Trailblazer":

                startMissionInstantly = false;
                misssionStartInfo = "Hit All Cones Before The Time Runs Out!";

                timeLimited = true;
                time = 10f;

                break;

            case "CCDS_MissionObjective_Checkpoint":

                startMissionInstantly = false;
                misssionStartInfo = "Pass All Checkpoints Before The Time Runs Out!";

                timeLimited = true;
                time = 15f;

                break;

            case "CCDS_MissionObjective_Race":

                startMissionInstantly = false;
                misssionStartInfo = "Win The Race!";

                timeLimited = true;
                time = 135f;

                break;

            case "CCDS_MissionObjective_Pursuit":

                startMissionInstantly = false;
                misssionStartInfo = "Take Him Out!";

                timeLimited = true;
                time = 135f;

                break;

        }

    }

}
