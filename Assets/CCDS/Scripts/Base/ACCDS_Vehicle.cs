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
/// Base class for the vehicle.
/// </summary>
[SelectionBase]
[DefaultExecutionOrder(10)]
public abstract class ACCDS_Vehicle : MonoBehaviour {

    private RCCP_CarController carController;

    /// <summary>
    /// Car controller.
    /// </summary>
    public RCCP_CarController CarController {

        get {

            if (carController == null)
                carController = GetComponent<RCCP_CarController>();

            return carController;

        }

    }

    private RCCP_AI ai;

    /// <summary>
    /// AI controller of the car controller.
    /// </summary>
    public RCCP_AI AI {

        get {

            if (ai == null)
                ai = CarController.OtherAddonsManager.AI;

            return ai;

        }

    }

    /// <summary>
    /// Is this vehicle alive?
    /// </summary>
    public bool IsAlive {

        get {

            if (damage < 100)
                return true;
            else
                return false;

        }

    }

    /// <summary>
    /// Health bar used on opponent vehicles.
    /// </summary>
    public CCDS_HealthBar HealthBar {

        get {

            if (healthBar == null && GetComponent<CCDS_HealthBar>())
                healthBar = GetComponent<CCDS_HealthBar>();

            if (healthBar == null) {

                healthBar = Instantiate(CCDS_Settings.Instance.healthBar);
                healthBar.transform.SetParent(transform);
                healthBar.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                healthBar.transform.localPosition += healthBar.positionOffset;

            }

            return healthBar;

        }

    }

    private CCDS_HealthBar healthBar;

    /// <summary>
    /// Engine smoke used on higher damage.
    /// </summary>
    public CCDS_EngineSmoke EngineSmoke {

        get {

            if (engineSmoke == null && GetComponent<CCDS_EngineSmoke>())
                engineSmoke = GetComponent<CCDS_EngineSmoke>();

            if (engineSmoke == null) {

                engineSmoke = Instantiate(CCDS_Settings.Instance.engineSmoke);
                engineSmoke.transform.SetParent(transform);
                engineSmoke.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                engineSmoke.transform.localPosition += engineSmoke.positionOffset;

            }

            return engineSmoke;

        }

    }

    private CCDS_EngineSmoke engineSmoke;

    /// <summary>
    /// Waypoint path.
    /// </summary>
    [HideInInspector] public RCCP_AIWaypointsContainer waypointPath;

    /// <summary>
    /// Damage.
    /// </summary>
    public float damage = 0f;

    /// <summary>
    /// Damage multiplier.
    /// </summary>
    public float damageMP = 1f;

    /// <summary>
    /// Finished the race?
    /// </summary>
    public bool finished = false;

    /// <summary>
    /// Gets the closest waypoint on the target waypoint path.
    /// </summary>
    public void GetClosestWaypoint() {

        //  Return if waypoint path is not selected.
        if (!waypointPath) {

            Debug.LogError("Waypoint Path is not selected on the " + transform.name + ". Can't get the closest waypoint...");
            CarController.OtherAddonsManager.AI.currentWaypointIndex = 0;
            return;

        }

        //  Closest distance and index temp variables.
        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;

        //  Getting the closest waypoint.
        for (int i = 0; i < waypointPath.waypoints.Count; i++) {

            if (waypointPath.waypoints[i] != null) {

                if (Vector3.Distance(transform.position, waypointPath.waypoints[i].transform.position) < closestDistance) {

                    closestDistance = Vector3.Distance(transform.position, waypointPath.waypoints[i].transform.position);
                    closestIndex = i;

                }

            }

        }

        CarController.OtherAddonsManager.AI.currentWaypointIndex = closestIndex;

    }


    /// <summary>
    /// Finished.
    /// </summary>
    public void Finished() {

        //  Setting finished to true and setting canControl bool of the vehicle to false.
        finished = true;
        CarController.canControl = false;

    }

    /// <summary>
    /// Wrecked.
    /// </summary>
    public void Wrecked() {

        //  Setting canControl bool of the vehicle to false.
        CarController.canControl = false;

    }

}
