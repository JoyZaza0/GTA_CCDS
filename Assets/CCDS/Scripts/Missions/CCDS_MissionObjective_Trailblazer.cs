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
/// A mission manager used on trailblazer.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Missions/CCDS MissionObjective Trailblazer")]
public class CCDS_MissionObjective_Trailblazer : ACCDS_Mission, ICCDS_CheckEditorError {

    /// <summary>
    /// All trailblazer obstacles.
    /// </summary>
    public List<CCDS_MissionObjective_TrailblazerItem> obstacles = new List<CCDS_MissionObjective_TrailblazerItem>();

    /// <summary>
    /// Default positions of obstacles.
    /// </summary>
    private Vector3[] defaultPositions;

    /// <summary>
    /// Default rotations of obstacles.
    /// </summary>
    private Quaternion[] defaultRotations;

    /// <summary>
    /// Remaining obstacles.
    /// </summary>
    public int remainingObstacles = 0;

    /// <summary>
    /// Total obstacles.
    /// </summary>
    public int totalObstacles = 0;

    private void Awake() {

        Initialize();

    }

    /// <summary>
    /// Initialize the mission.
    /// </summary>
    public void Initialize() {

        //  Getting all trailblazer obstacles.
        GetAllTrailblazerObstacles();

        //  Setting default positions and rotations.
        defaultPositions = new Vector3[obstacles.Count];
        defaultRotations = new Quaternion[obstacles.Count];

        //  Setting defaults to zero.
        remainingObstacles = 0;
        totalObstacles = 0;
        percentage = -1f;
        percentageOver = 100f;
        currentTarget = Vector3.zero;

        //  Assigning default positions and rotations.
        if (obstacles != null && obstacles.Count > 0) {

            for (int i = 0; i < obstacles.Count; i++) {

                if (obstacles[i] != null) {

                    defaultPositions[i] = obstacles[i].transform.position;
                    defaultRotations[i] = obstacles[i].transform.rotation;

                }

            }

        }

    }

    private void OnEnable() {

        //  Make sure everything is back to default when re-enabling the checkpoint.
        Restart();

    }

    private void OnDisable() {

        //  Setting defaults to zero.
        remainingObstacles = 0;
        totalObstacles = 0;
        percentage = -1f;
        percentageOver = 100f;
        currentTarget = Vector3.zero;

    }

    /// <summary>
    /// Restarts the mission. Everything goes back to the default settings.
    /// </summary>
    public void Restart() {

        //  Setting defaults to zero.
        remainingObstacles = 0;
        totalObstacles = 0;
        percentage = -1f;
        percentageOver = 100f;
        currentTarget = Vector3.zero;

        //  Setting positions and rotations of the checkpoints back to the default ones.
        if (obstacles != null && obstacles.Count > 0) {

            for (int i = 0; i < obstacles.Count; i++) {

                if (obstacles[i] != null) {

                    obstacles[i].Restart();

                    if (defaultPositions[i] != Vector3.zero) {

                        obstacles[i].transform.position = defaultPositions[i];
                        obstacles[i].transform.rotation = defaultRotations[i];

                    }

                }

            }

        }

    }

    private void Update() {

        //  If trailblazers found, get remaining and total trailblazer values. Otherwise set them to 0.
        if (obstacles != null && obstacles.Count > 0) {

            remainingObstacles = 0;
            totalObstacles = obstacles.Count;

            for (int i = 0; i < obstacles.Count; i++) {

                if (obstacles[i] != null && !obstacles[i].collided)
                    remainingObstacles++;

            }

        } else {

            remainingObstacles = 0;
            totalObstacles = 0;

        }

        //  Setting percentage depending on the remaining and total obstacles.
        if (remainingObstacles > 0 && totalObstacles > 0)
            percentage = Mathf.Lerp(100f, 0f, (float)remainingObstacles / (float)totalObstacles);
        else
            percentage = -1f;

        //  If there is a remaining obstacle, set current target to it.
        if (remainingObstacles > 0 && totalObstacles > 0)
            currentTarget = obstacles[totalObstacles - remainingObstacles].transform.position;
        else
            currentTarget = Vector3.zero;

        //  If remaining trailblazers is 0 and total trailblazers over 0, this means all remaining trailblazers are hit to 0. Complete the mission with success.
        if (remainingObstacles == 0 && totalObstacles > 0)
            Completed(true);

    }

    /// <summary>
    /// Completes the mission with stated success.
    /// </summary>
    /// <param name="state"></param>
    public void Completed(bool state) {

        //  Setting remaining and total checkpoints to 0.
        Restart();

        //  Mission completed with state.
        CCDS_GameplayManager.Instance.MissionCompleted(state);

    }

    /// <summary>
    /// Adds a new trailblazer obstacle.
    /// </summary>
    public void AddTrailblazerObstacle(CCDS_MissionObjective_TrailblazerItem newTrailblazerObstacle) {

        if (obstacles.Contains(newTrailblazerObstacle))
            return;

        obstacles.Add(newTrailblazerObstacle);

    }

    /// <summary>
    /// Gets all trailblazer obstacles.
    /// </summary>
    public void GetAllTrailblazerObstacles() {

        //  Checking the list. Creating if it's null.
        if (obstacles == null)
            obstacles = new List<CCDS_MissionObjective_TrailblazerItem>();

        //  Clearing the list.
        obstacles.Clear();

        //  Getting all obstacles.
        obstacles = GetComponentsInChildren<CCDS_MissionObjective_TrailblazerItem>(true).ToList();

    }

    public string[] CheckErrors() {

        List<string> errorStrings = new List<string>();

        if (obstacles == null)
            errorStrings.Add("Missing trailblazer obstacles!");

        if (obstacles != null && obstacles.Count == 0)
            errorStrings.Add("Missing trailblazer obstacles, assign or create new trailblazer obstacles in the scene!");

        return errorStrings.ToArray();

    }

}
