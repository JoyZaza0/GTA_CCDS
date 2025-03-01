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
/// Get back to the garage marker. It's basically a trigger collider. Once player triggers it, main menu will be loaded.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Missions/CCDS Marker Garage")]
public class CCDS_Marker_Garage : ACCDS_Component {

    private CCDS_Player targetPlayer;

    /// <summary>
    /// UI canvas for camera rotation.
    /// </summary>
    [Space()] public Transform UI;

    private void Update() {

        if (targetPlayer != null) {

            if (targetPlayer.timeForGarage >= 3) {

                CCDS.MainMenu();
                Destroy(gameObject);
                return;

            }

        }

        //  Set rotation of the UI canvas.
        if (UI && Camera.main)
            UI.rotation = Camera.main.transform.rotation;

    }

    /// <summary>
    /// On trigger.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {

        //  Return if gameplay manager not found.
        if (!CCDS_GameplayManager.Instance)
            Debug.LogError("CCDS_GameplayManager couldn't found, can't start the mission! Create CCDS_SceneManager and check the scene setup. Tools --> BCG --> CCDS --> Create --> Scene Managers --> Gameplay --> CCDS Scene Manager.");

        //  Finding the player vehicle.
        CCDS_Player player = CCDS_GameplayManager.Instance.player;

        //  Return if player not found.
        if (!player)
            return;

        targetPlayer = other.GetComponentInParent<CCDS_Player>();

        //  Return if player not found.
        if (!targetPlayer)
            return;

        //  If triggered vehicle and local player vehicle is the same, load the main menu.
        if (!Equals(targetPlayer.gameObject, CCDS_GameplayManager.Instance.player.gameObject))
            return;

        targetPlayer.garaging = true;

    }

    private void OnTriggerExit(Collider other) {

        //  Finding the player vehicle.
        CCDS_Player player = other.GetComponentInParent<CCDS_Player>();

        //  Return if player not found.
        if (!player)
            return;

        targetPlayer.garaging = false;

    }

}
