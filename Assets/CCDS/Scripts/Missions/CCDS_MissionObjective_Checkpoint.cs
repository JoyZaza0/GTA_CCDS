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
using System.Linq;
using UnityEngine;

/// <summary>
/// A mission manager used for checkpoints.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Missions/CCDS MissionObjective Checkpoint")]
public class CCDS_MissionObjective_Checkpoint : ACCDS_Mission, ICCDS_CheckEditorError {

    /// <summary>
    /// All checkpoints.
    /// </summary>
    public List<CCDS_MissionObjective_CheckpointItem> checkpoints = new List<CCDS_MissionObjective_CheckpointItem>();

    /// <summary>
    /// Default positions of the checkpoints.
    /// </summary>
    private Vector3[] defaultPositions;

    /// <summary>
    /// Default rotations of the checkpoints.
    /// </summary>
    private Quaternion[] defaultRotations;

    /// <summary>
    /// Remaining checkpoints.
    /// </summary>
    public int remainingCheckpoints = 0;

    /// <summary>
    /// Total checkpoints.
    /// </summary>
    public int totalCheckpoints = 0;

    private void Awake() {

        Initialize();

    }

    /// <summary>
    /// Initialize the mission.
    /// </summary>
    public void Initialize() {

        //  Getting all checkpoints.
        GetAllCheckpoints();

        //  Getting default positions and rotations.
        defaultPositions = new Vector3[checkpoints.Count];
        defaultRotations = new Quaternion[checkpoints.Count];

        //  Assigning default positions and rotations.
        if (checkpoints != null && checkpoints.Count > 0) {

            for (int i = 0; i < checkpoints.Count; i++) {

                if (checkpoints[i] != null) {

                    defaultPositions[i] = checkpoints[i].transform.position;
                    defaultRotations[i] = checkpoints[i].transform.rotation;

                }

            }

        }

        //  Setting defaults to zero.
        remainingCheckpoints = 0;
        totalCheckpoints = 0;
        percentage = -1f;
        percentageOver = 100f;
        currentTarget = Vector3.zero;

    }

    private void OnEnable() {

        //  Make sure everything is back to default when re-enabling the checkpoint.
        Restart();

    }

    private void OnDisable() {

        //  Setting defaults to zero.
        remainingCheckpoints = 0;
        totalCheckpoints = 0;
        percentage = -1f;
        percentageOver = 100f;
        currentTarget = Vector3.zero;

    }

    /// <summary>
    /// Restarts the mission. Everything goes back to the default settings.
    /// </summary>
    public void Restart() {

        //  Setting defaults to zero.
        remainingCheckpoints = 0;
        totalCheckpoints = 0;
        percentage = -1f;
        percentageOver = 100f;
        currentTarget = Vector3.zero;

        //  Setting positions and rotations of the checkpoints back to the default ones.
        if (checkpoints != null && checkpoints.Count > 0) {

            for (int i = 0; i < checkpoints.Count; i++) {

                if (checkpoints[i] != null) {

                    checkpoints[i].Restart();

                    checkpoints[i].transform.position = defaultPositions[i];
                    checkpoints[i].transform.rotation = defaultRotations[i];

                }

            }

        }

    }

    private void Update() {

        //  If checkpoints found, get remaining and total checkpoint values. Otherwise set them to 0.
        if (checkpoints != null && checkpoints.Count > 0) {

            remainingCheckpoints = 0;
            totalCheckpoints = checkpoints.Count;

            for (int i = 0; i < checkpoints.Count; i++) {

                if (checkpoints[i] != null && !checkpoints[i].passed)
                    remainingCheckpoints++;

            }

        } else {

            remainingCheckpoints = 0;
            totalCheckpoints = 0;

        }

        if (remainingCheckpoints > 0 && totalCheckpoints > 0)
            percentage = Mathf.Lerp(100f, 0f, (float)remainingCheckpoints / (float)totalCheckpoints);
        else
            percentage = -1f;

        if (remainingCheckpoints > 0 && totalCheckpoints > 0)
            currentTarget = checkpoints[totalCheckpoints - remainingCheckpoints].transform.position;
        else
            currentTarget = Vector3.zero;

        //  If remaining checkpoints is 0 and total checkpoints over 0, complete the mission with success.
        if (remainingCheckpoints == 0 && totalCheckpoints > 0)
            Completed(true);

    }

    /// <summary>
    /// Gets all checkpoints.
    /// </summary>
    public void GetAllCheckpoints() {

        //  Checking the list. Creating if it's null.
        if (checkpoints == null)
            checkpoints = new List<CCDS_MissionObjective_CheckpointItem>();

        //  Clearing the list.
        checkpoints.Clear();

        //  Getting all checkpoints.
        checkpoints = GetComponentsInChildren<CCDS_MissionObjective_CheckpointItem>(true).ToList();

    }

    /// <summary>
    /// Adds a new checkpoint.
    /// </summary>
    public void AddCheckpoint(CCDS_MissionObjective_CheckpointItem newCheckpoint) {

        if (checkpoints.Contains(newCheckpoint))
            return;

        checkpoints.Add(newCheckpoint);

    }

    /// <summary>
    /// Completes the mission with stated success.
    /// </summary>
    /// <param name="success"></param>
    public void Completed(bool success) {

        //  Setting remaining and total checkpoints to 0.
        remainingCheckpoints = 0;
        totalCheckpoints = 0;

        //  Mission completed.
        CCDS_GameplayManager.Instance.MissionCompleted(success);

    }

    public string[] CheckErrors() {

        List<string> errorStrings = new List<string>();

        if (checkpoints == null)
            errorStrings.Add("Missing checkpoints!");

        if (checkpoints != null && checkpoints.Count == 0)
            errorStrings.Add("Missing checkpoints gates, assign or create new checkpoints gates in the scene!");

        return errorStrings.ToArray();

    }

}
