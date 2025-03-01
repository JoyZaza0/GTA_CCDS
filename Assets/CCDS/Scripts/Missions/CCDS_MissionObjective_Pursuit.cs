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
/// Pursuit mission.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Missions/CCDS MissionObjective Pursuit")]
public class CCDS_MissionObjective_Pursuit : ACCDS_Mission, ICCDS_CheckEditorError {

    /// <summary>
    /// Pursuit vehicle.
    /// </summary>
    public ACCDS_Vehicle pursuitVehicle;

    /// <summary>
    /// Waypoint path for the pursuit vehicle.
    /// </summary>
    public RCCP_AIWaypointsContainer waypointPath;

    /// <summary>
    /// Default position of the vehicle.
    /// </summary>
    private Vector3 defaultPosition;

    /// <summary>
    /// Default rotation of the vehicle.
    /// </summary>
    private Quaternion defaultRotation;

    private void Awake() {

        Initialize();

    }

    /// <summary>
    /// Initialize the mission.
    /// </summary>
    public void Initialize() {

        //  Getting pursuit vehicle.
        GetVehicle();

        //  Getting default positions and rotation.
        defaultPosition = pursuitVehicle.transform.position;
        defaultRotation = pursuitVehicle.transform.rotation;

        //  Setting defaults to zero.
        pursuitVehicle.CarController.canControl = false;
        pursuitVehicle.waypointPath = waypointPath;
        pursuitVehicle.damage = 0f;
        percentage = -1f;
        percentageOver = 100f;

    }

    private void OnEnable() {

        //  Make sure everything is back to default when re-enabling the checkpoint.
        Restart();

        //  Listening an event when mission starts. It'll be used to set canControl bool of the pursuit vehicle to true.
        CCDS_Events.OnMissionStarted += CCDS_Events_OnMissionStarted;

    }

    /// <summary>
    /// When the mission starts.
    /// </summary>
    private void CCDS_Events_OnMissionStarted() {

        //  Setting canControl bool of the pursuit vehicle to true.
        pursuitVehicle.CarController.canControl = true;

    }

    private void OnDisable() {

        //  Setting defaults to zero.
        pursuitVehicle.CarController.canControl = false;
        pursuitVehicle.damage = 0f;
        percentage = -1f;
        percentageOver = 100f;

        //  Resetting waypoint index back to 0.
        pursuitVehicle.CarController.OtherAddonsManager.AI.currentWaypointIndex = 0;

        //  Not listening the event.
        CCDS_Events.OnMissionStarted -= CCDS_Events_OnMissionStarted;

    }

    /// <summary>
    /// Restarts the mission. Everything goes back to the default settings.
    /// </summary>
    public void Restart() {

        //  Setting defaults to zero.
        pursuitVehicle.CarController.canControl = false;
        pursuitVehicle.damage = 0f;
        percentage = -1f;
        percentageOver = 100f;

        //  Transporting the pursuit vehicle back to the original position and rotation.
        if (defaultPosition != Vector3.zero)
            RCCP.Transport(pursuitVehicle.CarController, defaultPosition, defaultRotation);

        //  Resetting waypoint index back to 0.
        pursuitVehicle.CarController.OtherAddonsManager.AI.currentWaypointIndex = 0;

    }

    private void Update() {

        //  Return if no pursuit vehicle selected.
        if (!pursuitVehicle) {

            Debug.LogError("Pursuit vehicle is not selected for the " + transform.name + "! Therefore, pursuit mission will not work properly. Please select the pursuit vehicle.");
            return;

        }

        //  Mission's current target is pursuit vehicle.
        currentTarget = pursuitVehicle.transform.position;

        //  Calculating the percentage.
        percentage = Mathf.Lerp(0f, 100f, pursuitVehicle.damage);

        //  If pursuit vehicle is wrecked, complete the mission with success.
        if (!pursuitVehicle.IsAlive)
            Completed(true);

    }

    /// <summary>
    /// Completes the mission with stated success.
    /// </summary>
    /// <param name="success"></param>
    public void Completed(bool success) {

        //  Mission completed.
        CCDS_GameplayManager.Instance.MissionCompleted(success);

    }

    /// <summary>
    /// Gets the pursuit vehicle.
    /// </summary>
    public void GetVehicle() {

        pursuitVehicle = GetComponentInChildren<CCDS_MissionObjective_Pursuit_Vehicle>(true);

    }

    /// <summary>
    /// Sets the new vehicle as pursuit vehicle.
    /// </summary>
    /// <param name="newVehicle"></param>
    public void SetVehicle(ACCDS_Vehicle newVehicle) {

        if (transform.GetComponentInChildren<ACCDS_Vehicle>()) {

            if (!Equals(transform.GetComponentInChildren<ACCDS_Vehicle>().gameObject, newVehicle.gameObject))
                DestroyImmediate(transform.GetComponentInChildren<ACCDS_Vehicle>().gameObject);

        }

        pursuitVehicle = newVehicle;

    }

    public string[] CheckErrors() {

        List<string> errorStrings = new List<string>();

        if (pursuitVehicle == null)
            errorStrings.Add("Pursuit vehicle is missing, please select it in the scene or create a new one!");

        if (waypointPath == null)
            errorStrings.Add("Waypoint path is not selected for " + transform.name + ", please select it in the scene or create a new one!");

        return errorStrings.ToArray();

    }

}
