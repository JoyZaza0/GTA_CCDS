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
/// Race positioner system used on vehicles. Must be attached to the vehicle.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Missions/CCDS Race Positioner")]
public class CCDS_RacePositioner : ACCDS_Component {

    /// <summary>
    /// Target waypoint path.
    /// </summary>
    public RCCP_AIWaypointsContainer waypointPath;

    /// <summary>
    /// Current waypoint.
    /// </summary>
    public Transform currentWaypoint;

    /// <summary>
    /// Current waypoint index.
    /// </summary>
    public int currentWaypointIndex = 0;

    /// <summary>
    /// Total distance traveled.
    /// </summary>
    public float totalDistance = 0f;

    /// <summary>
    /// Current distance traveled between last waypoint and current waypoint.
    /// </summary>
    private float curDistance = 0f;

    /// <summary>
    /// Values to detect distance traveled changed.
    /// </summary>
    private float distanceTraveled_Old = 0f;
    private float distanceTraveled_Last = 0f;

    /// <summary>
    /// Gets the closest waypoint on the target waypoint path.
    /// </summary>
    public void GetClosestWaypoint() {

        //  Return if waypoint path is not selected.
        if (!waypointPath) {

            Debug.LogError("Waypoint Path is not selected on the " + transform.name + ". Can't get the closest waypoint...");
            currentWaypointIndex = -1;
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

        currentWaypoint = waypointPath.waypoints[closestIndex].transform;
        currentWaypointIndex = closestIndex;

    }

    private void OnEnable() {

        currentWaypointIndex = 0;
        totalDistance = 0;
        curDistance = 0;
        distanceTraveled_Old = 0;
        distanceTraveled_Last = 0;

        //  Get the closest waypoint if waypoint path is selected.
        if (waypointPath)
            GetClosestWaypoint();

    }

    private void Update() {

        //  Return if waypoint path is not selected.
        if (!waypointPath) {

            Debug.LogError("Waypoint Path is not selected for " + transform.name + "!");
            currentWaypointIndex = -1;
            enabled = false;
            return;

        }

        //  Reset waypoint index if above total waypoint count.
        if (currentWaypointIndex >= waypointPath.waypoints.Count)
            currentWaypointIndex = 0;

        // Next waypoint and its position.
        currentWaypoint = waypointPath.waypoints[currentWaypointIndex].transform;
        float distanceBetweenWaypoints = Vector3.Distance(waypointPath.waypoints[(int)Mathf.Clamp(currentWaypointIndex - 1, 0f, Mathf.Infinity)].transform.position, currentWaypoint.position);

        //  Current position.
        curDistance = Vector3.Distance(transform.position, currentWaypoint.position);

        //  Pass the waypoint.
        if (curDistance != 0 && curDistance <= 20f) {

            totalDistance += distanceBetweenWaypoints;
            currentWaypointIndex++;

        } else if (Vector3.Dot((currentWaypoint.position - transform.position).normalized, transform.forward) < 0f) {

            //totalDistance += distanceBetweenWaypoints;
            //currentWaypointIndex++;

        }

        float distanceTraveled_F = totalDistance + (distanceBetweenWaypoints - curDistance);

        if (distanceTraveled_Old > distanceTraveled_F && distanceTraveled_Last == 0)
            distanceTraveled_Last = distanceTraveled_F;
        else if (distanceTraveled_Old < distanceTraveled_F)
            distanceTraveled_Last = 0f;

        distanceTraveled_Old = distanceTraveled_F;

    }

    private void OnDisable() {

        currentWaypointIndex = 0;
        totalDistance = 0;
        curDistance = 0;
        distanceTraveled_Old = 0;
        distanceTraveled_Last = 0;

    }

}
