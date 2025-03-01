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
/// Collider with trigger enabled used to finish the race mission (CCDS_Mission_Race).
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Missions/CCDS MissionObjective Race Finisher")]
public class CCDS_MissionObjective_Race_Finisher : ACCDS_Component {

    /// <summary>
    /// Race mission as CCDS_Mission_Race.
    /// </summary>
    private CCDS_MissionObjective_Race raceMission;

    private void Awake() {

        //  Getting race mission in parent.
        raceMission = GetComponentInParent<CCDS_MissionObjective_Race>();

    }

    /// <summary>
    /// On trigger enter.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {

        //  Getting the trigger player.
        CCDS_Player player = other.GetComponentInParent<CCDS_Player>();

        //  If trigger is player...
        if (player) {

            //  If it's player, complete the mission with success if not any opponent passed the finish line before.
            CCDS_GameplayManager.Instance.MissionCompleted(CheckIfPlayerCanWinTheRace());

        }

        //  Getting the opponent player.
        CCDS_MissionObjective_Race_Vehicle opponent = other.GetComponentInParent<CCDS_MissionObjective_Race_Vehicle>();

        //  If it's opponent, set ''finished'' bool of the opponent to true. Race won't be over if opponent passes the finish line. It will be used to complete the mission with success or failure.
        if (opponent) {

            //  If it's opponent, set ''finished'' bool of the opponent to true.
            opponent.Finished();

        }

    }

    /// <summary>
    /// Check if player can win the race after passing the finish line. 
    /// </summary>
    /// <returns></returns>
    public bool CheckIfPlayerCanWinTheRace() {

        //  Return if no race mission found.
        if (!raceMission) {

            Debug.LogError("raceMission variable of the " + transform.name + " is not selected. Therefore, race finisher won't work. Please select the raceMission...");
            return false;

        }

        //  Looping all racers...
        for (int i = 0; i < raceMission.racers.Count; i++) {

            if (raceMission.racers[i] != null) {

                //  If one of them passed the finish line, return with false.
                if (raceMission.racers[i].finished)
                    return false;

            }

        }

        //  None of any racers passed the finish line yet. Return with true.
        return true;

    }

    private void OnDrawGizmos() {

        Gizmos.matrix = transform.localToWorldMatrix;

        Color defColor = Gizmos.color;
        Gizmos.color = new Color(0f, 1f, 0f, .5f);
        Gizmos.DrawCube(GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size);
        Gizmos.color = defColor;

    }

}
