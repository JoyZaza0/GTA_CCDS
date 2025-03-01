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
/// Mission component used on checkpoints.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Missions/CCDS MissionObjective Checkpoint Item")]
public class CCDS_MissionObjective_CheckpointItem : ACCDS_Component {

    /// <summary>
    /// Pass audioclip when player passes this checkpoint.
    /// </summary>
    public AudioClip passClip;

    /// <summary>
    /// Passed this checkpoint before?
    /// </summary>
    public bool passed = false;

    /// <summary>
    /// Add seconds when player passes this checkpoint?
    /// </summary>
    public bool addSecondsOnPass = true;
    public int addSeconds = 1;

    private void OnEnable() {

        passed = false;

    }

    /// <summary>
    /// Restarts the checkpoint and restores back to default.
    /// </summary>
    public void Restart() {

        passed = false;

    }

    private void OnTriggerEnter(Collider other) {

        //  If passed before, return.
        if (passed)
            return;

        //  Getting player.
        CCDS_Player player = other.GetComponentInParent<CCDS_Player>();

        //  If trigger is not player, return.
        if (!player)
            return;

        //  If triggered vehicle is same  with actual player vehicle, pass.
        if (CCDS_GameplayManager.Instance && Equals(CCDS_GameplayManager.Instance.player.gameObject, player.gameObject))
            Passed();

    }

    /// <summary>
    /// Passed checkpoint.
    /// </summary>
    private void Passed() {

        //  Passed.
        passed = true;

        //  If audioclip is selected, play it.
        if (passClip)
            RCCP_AudioSource.NewAudioSource(gameObject, passClip.name, 0f, 0f, 1f, passClip, false, true, true);

        //  Add additional seconds if selected.
        if (addSecondsOnPass)
            CCDS_GameplayManager.Instance.AddTime(addSeconds);

    }

}
