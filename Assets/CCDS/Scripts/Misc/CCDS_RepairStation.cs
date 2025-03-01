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
/// Repair station to repair the player vehicle. Works with trigger collider.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Misc/CCDS Repair Station")]
public class CCDS_RepairStation : ACCDS_Component {

    /// <summary>
    /// UI canvas for displaying the repair station text.
    /// </summary>
    public Transform UI;

    private void Update() {

        //  Return if UI is not selected.
        if (!UI)
            return;

        //  Return if main camera couldn't found yet.
        if (!Camera.main)
            return;

        //  Setting rotation of the UI.
        UI.transform.rotation = Camera.main.transform.rotation;

    }

    /// <summary>
    /// On trigger enter.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other) {

        //  Getting the player.
        CCDS_Player player = other.GetComponentInParent<CCDS_Player>();

        //  If trigger is not player, return.
        if (!player)
            return;

        //  Return if player is not damaged.
        if (player.damage <= 0)
            return;

        //  If player doesn't have damage component, return.
        if (!player.CarController.Damage)
            return;

        //  Repairing the player vehicle.
        player.CarController.Damage.repaired = false;
        player.CarController.Damage.repairNow = true;
        player.damage = 0f;

        //  Displaying info.
        CCDS_UI_Informer.Instance.Info("Repaired!");

    }

    /// <summary>
    /// On trigger exit.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other) {

        //  Getting the player.
        CCDS_Player player = other.GetComponentInParent<CCDS_Player>();

        //  If trigger is not player, return.
        if (!player)
            return;

        //  If player doesn't have damage component, return.
        if (!player.CarController.Damage)
            return;

        //  Exisitng the trigger zone, stop repairing the vehicle.
        player.CarController.Damage.repaired = true;
        player.CarController.Damage.repairNow = false;

    }

}
