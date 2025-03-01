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
/// All markers have configured missions. And these missions have target transforms to transport the player vehicle.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Managers/CCDS MissionObjective Positions Manager")]
public class CCDS_MissionObjectivePositionsManager : ACCDS_Manager {

    private static CCDS_MissionObjectivePositionsManager instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static CCDS_MissionObjectivePositionsManager Instance {

        get {

            if (instance == null)
                instance = FindFirstObjectByType<CCDS_MissionObjectivePositionsManager>();

            return instance;

        }

    }

    /// <summary>
    /// Positions.
    /// </summary>
    public List<CCDS_MissionObjectivePosition> allPositions = new List<CCDS_MissionObjectivePosition>();

    private void Awake() {

        //  Getting all positions, even if they are disabled.
        GetAllPositions();

    }

    /// <summary>
    /// Get all mission transforms.
    /// </summary>
    public void GetAllPositions() {

        //  Checking the list. Creating if it's null.
        if (allPositions == null)
            allPositions = new List<CCDS_MissionObjectivePosition>();

        //  Clearing the list.
        allPositions.Clear();

        //  Getting all mission positions, even if they are disabled.
        allPositions = GetComponentsInChildren<CCDS_MissionObjectivePosition>(true).ToList();

    }

    /// <summary>
    /// Creates a new position and adds it to the list.
    /// </summary>
    public CCDS_MissionObjectivePosition CreateNewPosition() {

        GameObject sp = new GameObject("CCDS_MissionPosition_");
        sp.AddComponent<CCDS_MissionObjectivePosition>();
        sp.transform.SetParent(transform);
        sp.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        AddNewPosition(sp.GetComponent<CCDS_MissionObjectivePosition>());
        return sp.GetComponent<CCDS_MissionObjectivePosition>();

    }

    /// <summary>
    /// Adds a new position.
    /// </summary>
    /// <param name="newPosition)"></param>
    public void AddNewPosition(CCDS_MissionObjectivePosition newPosition) {

        allPositions.Add(newPosition);

    }

    private void Reset() {

        if (allPositions == null)
            allPositions = new List<CCDS_MissionObjectivePosition>();

    }

}

