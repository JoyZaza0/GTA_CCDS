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
/// Player component must be attached to the player vehicle.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Player/CCDS Player")]
[RequireComponent(typeof(RCCP_CarController))]
public class CCDS_Player : ACCDS_Component {

    /// <summary>
    /// Save name of the vehicle. Must be unique.
    /// </summary>
    public string vehicleSaveName = "";

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

    /// <summary>
    /// Total score.
    /// </summary>
    public float Score {

        get {

            return (score_Speed + score_Drift + score_Stunt) * scoreMultiplier;

        }

    }

    /// <summary>
    /// Is alive?
    /// </summary>
    public bool Alive {

        get {

            if (damage >= 100f)
                return false;
            else
                return true;

        }

    }

    /// <summary>
    /// Can control of the vehicle.
    /// </summary>
    public bool CanControl {

        get {

            return CarController.canControl;

        }

    }

    /// <summary>
    /// On Mission?
    /// </summary>
    public bool OnMission {

        get {

            if (!SceneManager.GameplayManager)
                return false;

            return SceneManager.GameplayManager.OnMission;

        }

    }

    /// <summary>
    /// Damage.
    /// </summary>
    public float damage = 0f;

    /// <summary>
    /// Damage multiplier factor.
    /// </summary>
    [Range(0f, 10f)] public float damageMP = 1f;

    /// <summary>
    /// Scores.
    /// </summary>
    public float score_Speed, score_Drift, score_Stunt;

    /// <summary>
    /// Score multiplier.
    /// </summary>
    [Range(0f, 10f)] public float scoreMultiplier = 1f;

    /// <summary>
    /// Felony.
    /// </summary>
    public float felony = 0f;

    /// <summary>
    /// Felony multiplier.
    /// </summary>
    [Range(0f, .1f)] public float felonyMultiplier = .02f;

    /// <summary>
    /// Timers for earning score.
    /// </summary>
    public float speedingTime, stuntingTime, driftingTime;

    /// <summary>
    /// Busting right now?
    /// </summary>
    public float busting = 0f;
    [Range(0f, 100f)] public float bustingMP = 20f;

    /// <summary>
    /// Police is nearby, in pursue, or busted the player already?
    /// </summary>
    public bool policeNearby, inPursue, busted;

    /// <summary>
    /// Calculated police fine as money.
    /// </summary>
    public float policeFineMoney = 0f;

    /// <summary>
    /// Time counter for getting back to the garage.
    /// </summary>
    public float timeForGarage = 0f;

    /// <summary>
    /// Increases time for garage on enable. If timer hits 3 seconds, main menu will be loaded.
    /// </summary>
    public bool garaging = false;

    private float resettedTime, resettedTimeForHardReset;

    private void Update() {

        //  Limiting damage and felony values betwen 0f - 100f.
        damage = Mathf.Clamp(damage, 0f, 100f);
        felony = Mathf.Clamp(felony, 0f, 100f);

        //  Return if vehicle is not controllable.
        if (!CanControl) {

            speedingTime = 0f;
            stuntingTime = 0f;
            driftingTime = 0f;
            resettedTime = 0f;
            resettedTimeForHardReset = 0f;
            busting = 0f;
            timeForGarage = 0f;
            return;

        }

        //  Police nearby?
        policeNearby = PoliceNearby();

        //  In pursue right now?
        inPursue = PoliceInPursue();

        //  Increasing time for garage on enable. If timer hits 3 seconds, main menu will be loaded.
        if (garaging)
            timeForGarage += Time.deltaTime;
        else
            timeForGarage = 0f;

        //  Calculating the drift, speeding, and stunt scores.
        Scores();

        //  Checking busted, busting, or not.
        CheckBusted();

        //  Resetting the vehicle on input.
        CheckReset();

    }

    /// <summary>
    /// Calculating the drift, speeding, and stunt scores.
    /// </summary>
    private void Scores() {

        //  If player is alive and not busted, can earn score...
        if (busted) {

            speedingTime = 0f;
            stuntingTime = 0f;
            driftingTime = 0f;
            return;

        }

        //  If player is alive, not busted, inpursue, and police is nearby, increase police fine money.
        if (inPursue && policeNearby)
            policeFineMoney += felony * felonyMultiplier * 30f * Time.deltaTime;

        //  Increase speeding time if player speed is over 80 km/h.
        if (Mathf.Abs(CarController.speed) >= 80f) {

            //  Increase speeding time.
            speedingTime += Time.deltaTime;

            //  If police is nearby, increase felony.
            if (policeNearby)
                felony += speedingTime * felonyMultiplier;

        } else {

            //  If player speed is below 80 km/h, set speeding time to 0.
            speedingTime = 0f;

        }

        //  Increasing speeding score if speeding time is over 2 seconds.
        if (speedingTime >= 2f)
            score_Speed += Mathf.Abs(CarController.speed) * scoreMultiplier * Time.deltaTime * 1f;

        //  If vehicle is not grounded and speed is over 80 km/h, it means it's flying.
        if (!CarController.IsGrounded && CarController.speed > 80f) {

            //  Increasing stunting time.
            stuntingTime += Time.deltaTime;

            //  Increasing felony if police is nearby.
            if (policeNearby)
                felony += stuntingTime * felonyMultiplier;

        } else {

            //  If vehicle is not flying, set stunting time to 0.
            stuntingTime = 0f;

        }

        //  If stunting time over half a second, increase stunt score.
        if (stuntingTime >= .5f)
            score_Stunt += Mathf.Abs(CarController.speed) * scoreMultiplier * 2f * Time.deltaTime * 2.5f;

        //  If vehicle is drifting, increase drifting time.
        if (Mathf.Abs(CarController.RearAxle.leftWheelCollider.wheelSlipAmountSideways) > .25f) {

            driftingTime += Time.deltaTime;

            //  Increase felony if police is nearby.
            if (policeNearby)
                felony += stuntingTime * felonyMultiplier;

        } else {

            //  If vehicle is not drifting, set drifting time to 0.
            driftingTime = 0f;

        }

        //  If drifting time is over half a second, increase drift score.
        if (driftingTime >= .5f)
            score_Drift += Mathf.Abs(CarController.speed) * Mathf.Abs(Mathf.Abs(CarController.RearAxle.leftWheelCollider.wheelSlipAmountSideways * 10f)) * scoreMultiplier * Time.deltaTime * 1f;

    }

    /// <summary>
    /// Checking busted, busting, or not.
    /// </summary>
    private void CheckBusted() {

        //  If busting is over 100f, bust the player.
        if (!busted && busting >= 100f)
            Busted();

        //  If police is in pursue, nearby, and speed of the player vehicle is below 40km/h.
        if (!inPursue || (inPursue && policeNearby && Mathf.Abs(CarController.speed) >= 40f))
            busting -= Time.deltaTime * bustingMP;

        busting = Mathf.Clamp(busting, 0f, 100f);

    }

    /// <summary>
    /// Resetting the vehicle on input.
    /// </summary>
    private void CheckReset() {

        //  Reset time used to prevent repeatedly.
        if (resettedTime > 0)
            resettedTime -= Time.deltaTime;

        if (resettedTime < 0)
            resettedTime = 0f;

        //  If player pushes the R button, try to reset the player vehicle.
        if (Input.GetKeyDown(KeyCode.R)) {

            //  Can't reset the player vehicle if busting right now.
            if (busting > 0) {

                CCDS_UI_Informer.Instance.Info("You can't reset the vehicle in pursue!");
                return;

            }

            //  Return if reset time is not 0.
            if (resettedTime != 0)
                return;

            //  Reset the vehicle.
            ResetVehicle();

        }

        //  If player pushes and holds the R button, try to reset the player vehicle to the spawn point.
        if (Input.GetKey(KeyCode.R)) {

            //  Can't reset the player vehicle if busting right now.
            if (busting > 0) {

                CCDS_UI_Informer.Instance.Info("You can't reset the vehicle in pursue!");
                return;

            }

            //  Increasing the reset timer.
            resettedTimeForHardReset += Time.deltaTime;

            //  If reset timer is above 3 seconds, reset the player.
            if (resettedTimeForHardReset < 3f)
                return;

            //  Reset the player vehicle back to the spawn point.
            HardResetVehicle();

        }

    }

    /// <summary>
    /// Resets the player vehicle.
    /// </summary>
    private void ResetVehicle() {

        resettedTime = 2f;

        CarController.Rigid.interpolation = RigidbodyInterpolation.None;
        CarController.Rigid.isKinematic = true;
        RCCP.Transport(transform.position + Vector3.up * 2f, Quaternion.Euler(0f, transform.eulerAngles.y, 0f));
        StartCoroutine(RestoreResetVehicle());

    }

    /// <summary>
    /// Resets the player vehicle back to the spawn position.
    /// </summary>
    private void HardResetVehicle() {

        resettedTimeForHardReset = 0f;

        CarController.Rigid.interpolation = RigidbodyInterpolation.None;
        CarController.Rigid.isKinematic = true;
        RCCP.Transport(SceneManager.GameplayManager.spawnPoint.transform.position, SceneManager.GameplayManager.spawnPoint.transform.rotation);
        StartCoroutine(RestoreResetVehicle());

    }

    /// <summary>
    /// Restores the resetted vehicle. Needs to be called after resetting the vehicle.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RestoreResetVehicle() {

        yield return new WaitForFixedUpdate();
        CarController.Rigid.interpolation = RigidbodyInterpolation.Interpolate;
        CarController.Rigid.isKinematic = false;

    }

    /// <summary>
    /// Busting the player.
    /// </summary>
    public void Busting() {

        //  Return if not controllable, or not alive, or busted.
        if (!CanControl || !Alive || busted)
            return;

        busting += Time.deltaTime * bustingMP;
        busting = Mathf.Clamp(busting, 0f, 100f);

    }

    /// <summary>
    /// Busted.
    /// </summary>
    private void Busted() {

        //  Return if not controllable, or not alive, or busted.
        if (!CanControl || !Alive || busted)
            return;

        //  Setting busted to true.
        busted = true;

        //  Setting controllable state to false.
        RCCP.SetControl(CarController, false);

        CCDS_Events.Event_OnLocalPlayerBusted(this);

    }

    /// <summary>
    /// Released from busted.
    /// </summary>
    public void ReleaseFromBusted() {

        //  Return if not controllable, or not alive, or busted.
        if (!Alive || !busted)
            return;

        //  Setting busted to true.
        felony = 0f;
        busting = 0f;
        busted = false;

        //  Setting controllable state to false.
        RCCP.SetControl(CarController, true);

        CCDS_Events.Event_OnLocalPlayerReleased(this);

    }

    /// <summary>
    /// Any nearby cops around?
    /// </summary>
    /// <returns></returns>
    private bool PoliceNearby() {

        CCDS_SceneManager sceneManager = SceneManager;

        //  Return ''false'' if CCDS_CopsManager not found.
        if (!sceneManager)
            return false;

        CCDS_CopsManager copsManager = sceneManager.CopsManager;

        if (!copsManager)
            return false;

        List<CCDS_AI_Cop> allCops = copsManager.allCops;

        //  If distance is below 100 meters, return true. Otherwise return false.
        for (int i = 0; i < allCops.Count; i++) {

            if (allCops[i] != null && Vector3.Distance(transform.position, allCops[i].transform.position) < 100f)
                return true;

        }

        return false;

    }

    /// <summary>
    /// Any cop is in pursue right now?
    /// </summary>
    /// <returns></returns>
    private bool PoliceInPursue() {

        //  Return ''false'' if CCDS_CopsManager not found.
        if (!SceneManager)
            return false;

        if (!SceneManager.CopsManager)
            return false;

        CCDS_CopsManager copsManager = SceneManager.CopsManager;

        bool inpursue = false;

        if (copsManager.allCops != null && copsManager.allCops.Count >= 1) {

            for (int i = 0; i < copsManager.allCops.Count; i++) {

                if (copsManager.allCops[i].InPursue && copsManager.allCops[i].targetChase != null && copsManager.allCops[i].targetChase == this)
                    inpursue = true;

            }

        }

        return inpursue;

    }

    /// <summary>
    /// On collision enter.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision) {

        //  Return if not alive.
        if (!Alive)
            return;

        //  Return if velocity of the collision is below 5.
        if (collision.relativeVelocity.magnitude < 5f)
            return;

        //  Impulse.
        float impulse = collision.impulse.magnitude / 3000f;

        //  Increasing the damage.
        damage += impulse * damageMP;

        //  If damage is above 100, wrecked.
        if (damage >= 100f) {

            damage = 100f;
            Wrecked();
            return;

        }

        if (CanControl && !busted) {

            CCDS_AI_Cop cop = collision.collider.GetComponentInParent<CCDS_AI_Cop>();

            if (cop && felony < 25f)
                felony += 25f;
            else if (cop && felony > 25f)
                felony += 10f;

        }

    }

    /// <summary>
    /// Wrecked.
    /// </summary>
    public void Wrecked() {

        RCCP.SetControl(CarController, false);
        RCCP.DeRegisterPlayerVehicle();

        //  Calling an event on wrecked.
        CCDS_Events.Event_OnLocalPlayerWrecked(this);

    }

    private void OnValidate() {

        if (vehicleSaveName == "")
            vehicleSaveName = "Player_" + transform.name;

    }

}
