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
/// Vehicle component used on racers for race mission (CCDS_Mission_Race).
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Missions/CCDS MissionObjective Race Vehicle")]
public class CCDS_MissionObjective_Race_Vehicle : ACCDS_Vehicle {

    private void OnEnable() {

        if (AI == null) {

            Debug.LogError("AI component couldn't found on " + transform.name + ". Be sure this racer has AI controller component.");
            enabled = false;
            return;

        }

        if (waypointPath == null) {

            Debug.LogError("Waypoint path is not selected for " + transform.name + ". Be sure this racer has a valid waypoint path.");
            enabled = false;
            return;

        }

        AI.waypointsContainer = waypointPath;
        AI.currentWaypointIndex = 0;

        GetClosestWaypoint();

        if (CCDS_Settings.Instance.showHealthBar && HealthBar)
            HealthBar.gameObject.SetActive(true);

        if (CCDS_Settings.Instance.showEngineSmoke && EngineSmoke)
            EngineSmoke.gameObject.SetActive(true);

        damage = 0f;
        finished = false;

    }

    private void Update() {

        //  Clamping the damage between 0f - 100f.
        damage = Mathf.Clamp(damage, 0f, 100f);

    }

    /// <summary>
    /// On collision.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision) {

        //  Return if not alive.
        if (!IsAlive)
            return;

        //  Return if velocity of the collision is below 5.
        if (collision.relativeVelocity.magnitude < 5)
            return;

        //  Impulse of the collision.
        float impulse = collision.impulse.magnitude / 10000f;

        //  Increasing the damage depending on the impulse with damage multiplier.
        damage += impulse * damageMP;

        //  If damage is above 100f, wrecked.
        if (damage > 100f) {

            damage = 100f;
            Wrecked();

        }

    }

    private void OnDisable() {

        if (CCDS_Settings.Instance.showHealthBar && HealthBar)
            HealthBar.gameObject.SetActive(false);

        if (CCDS_Settings.Instance.showEngineSmoke && EngineSmoke)
            EngineSmoke.gameObject.SetActive(false);

    }

}
