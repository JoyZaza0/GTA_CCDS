//----------------------------------------------
//        City Car Driving Simulator
//
// Copyright � 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[DefaultExecutionOrder(10)]
[AddComponentMenu("BoneCracker Games/CCDS/Missions/Mission Items/CCDS AI Cop")]
public class CCDS_AI_Cop : ACCDS_Vehicle {

    /// <summary>
    /// Unity's Navigator.
    /// </summary>
    private NavMeshAgent navigator;

    /// <summary>
    /// Detector with Sphere Collider. Used for finding target Gameobjects in chasing mode.
    /// </summary>
    public List<CCDS_Player> targetsInZone = new List<CCDS_Player>();

    /// <summary>
    /// Detector radius.
    /// </summary>
    public float detectorRadius = 100f;

    /// <summary>
    /// Target Gameobject for chasing.
    /// </summary>
    public CCDS_Player targetChase;

    /// <summary>
    /// Steer, Motor, And Brake inputs. Will feed RCC_CarController with these inputs.
    /// </summary>
    public float steerInput = 0f;
    public float throttleInput = 0f;
    public float brakeInput = 0f;
    public float handbrakeInput = 0f;

    /// <summary>
    /// This timer was used for deciding go back or not, after crashing.
    /// </summary>
    private float resetTime = 0f;
    private bool reversingNow = false;

    /// <summary>
    /// Has target to purchase now?
    /// </summary>
    public bool InPursue {

        get {

            bool state = false;

            if (targetChase != null)
                state = true;

            return state;

        }

    }

    /// <summary>
    /// Police siren component.
    /// </summary>
    public RCCP_PoliceLights policeSiren;

    /// <summary>
    /// Police siren audioclip.
    /// </summary>
    public AudioSource sirenAudio;

    private void Awake() {

        // Creating our Navigator and setting properties for pathfinding. Unity's NavMesh will be used.
        GameObject navigatorObject = new GameObject("Navigator");
        navigatorObject.transform.SetParent(transform, false);
        navigator = navigatorObject.AddComponent<NavMeshAgent>();
        navigator.radius = 1;
        navigator.speed = 1;
        navigator.angularSpeed = 100000f;
        navigator.acceleration = 100000f;
        navigator.height = 1;
        navigator.avoidancePriority = 0;

    }

    private void OnEnable() {

        //  Initializing the healthbar if enabled.
        if (CCDS_Settings.Instance.showHealthBar && HealthBar)
            HealthBar.gameObject.SetActive(true);

        //  Initializing the engine smoke if enabled.
        if (CCDS_Settings.Instance.showEngineSmoke && EngineSmoke)
            EngineSmoke.gameObject.SetActive(true);

    }

    private void Update() {

        // Assigning navigator's position to front wheels of the vehicle
        navigator.transform.localPosition = Vector3.zero;
        navigator.transform.localPosition += Vector3.forward * CarController.FrontAxle.leftWheelCollider.transform.localPosition.z;

        //  Enabling / disabling the siren depending on the target chase.
        sirenAudio.enabled = (targetChase);
        policeSiren.SetSiren((targetChase));

        //  If vehicle is controllable, check navigation and reset.
        if (CarController.canControl) {

            CheckReset();
            Navigation();

        } else {

            targetChase = null;

        }

        //  Feed RCCP with inputs.
        FeedRCC();

        //  If target chase is not null, close enough, and speed is below target point, busting.
        if (targetChase != null && Vector3.Distance(transform.position, targetChase.transform.position) < 15f && Mathf.Abs(targetChase.CarController.speed) <= 20f && targetChase.CarController.canControl && targetChase.Alive && !targetChase.busted)
            targetChase.Busting();

    }

    private void FixedUpdate() {

        //  Check possible targets if alive and controllable.
        if (IsAlive && CarController.canControl)
            CheckTargets();

    }

    /// <summary>
    /// Navigation based on Unity's NavMesh.
    /// </summary>
    private void Navigation() {

        // Navigator Input is multiplied by 3.5f for fast reactions.
        float navigatorInput = Mathf.Clamp(transform.InverseTransformDirection(navigator.desiredVelocity).x * 3.5f, -1f, 1f);

        // If our scene doesn't have a target to chase, stop and return.
        if (!targetChase) {

            Stop();
            return;

        }

        // If the vehicle is close enough to the target, and speed is below 10, stop.
        if (Vector3.Distance(transform.position, targetChase.transform.position) < 15f && Mathf.Abs(targetChase.CarController.speed) <= 20f) {

            Stop();
            return;

        }

        Vector3 correctedTargetPosition = targetChase.transform.position;
        correctedTargetPosition += targetChase.transform.forward * (targetChase.CarController.speed / 10f);

        // Setting destination of the Navigator. 
        if (navigator.isOnNavMesh)
            navigator.SetDestination(correctedTargetPosition);

        //  If vehicle goes forward, calculate throttle and brake inputs.
        if (!reversingNow) {

            throttleInput = 1f;
            throttleInput *= Mathf.Clamp01(Mathf.Lerp(10f, 0f, (Mathf.Abs(CarController.speed)) / CarController.maximumSpeed));
            brakeInput = 0f;
            handbrakeInput = 0f;

            //  If vehicle speed is high enough, calculate them related to navigator input. This will reduce throttle input, and increase brake input on sharp turns.
            if (CarController.speed > 30f) {

                throttleInput -= Mathf.Abs(navigatorInput) / 3f;
                brakeInput += Mathf.Abs(navigatorInput) / 3f;

            }

        }

        //  Brake input can't be higher than 0.25.
        if (brakeInput > .25f)
            throttleInput = 0f;

        // Steer input.
        steerInput = navigatorInput;
        steerInput = Mathf.Clamp(steerInput, -1f, 1f) * CarController.direction;

        //  Clamping inputs.
        throttleInput = Mathf.Clamp01(throttleInput);
        brakeInput = Mathf.Clamp01(brakeInput);
        handbrakeInput = Mathf.Clamp01(handbrakeInput);

        //  If vehicle goes backwards, set brake input to 1 for reversing.
        if (reversingNow) {

            throttleInput = 0f;
            brakeInput = 1f;
            handbrakeInput = 0f;

        } else {

            if (Mathf.Abs(CarController.speed) < 5f && brakeInput >= .5f) {

                brakeInput = 0f;
                handbrakeInput = 1f;

            }

        }

    }

    /// <summary>
    /// Checks the near targets if navigation mode is set to follow or chase mode.
    /// </summary>
    private void CheckTargets() {

        if (targetsInZone == null)
            targetsInZone = new List<CCDS_Player>();

        if (!CCDS_GameplayManager.Instance) {

            targetsInZone.Clear();
            return;

        }

        List<CCDS_Player> dD_Players = new List<CCDS_Player>();

        if (CCDS_GameplayManager.Instance.player != null) {

            if (Vector3.Distance(transform.position, CCDS_GameplayManager.Instance.player.transform.position) <= detectorRadius) {

                if (!targetsInZone.Contains(CCDS_GameplayManager.Instance.player))
                    targetsInZone.Add(CCDS_GameplayManager.Instance.player);

            }

        }

        // Removing unnecessary targets in list first. If target is null or not active, remove it from the list.
        for (int i = 0; i < targetsInZone.Count; i++) {

            if (targetsInZone[i] == null)
                targetsInZone.RemoveAt(i);

            if (targetsInZone[i] != null)
                dD_Players.Add(targetsInZone[i]);

        }

        targetsInZone.Clear();
        targetsInZone = new List<CCDS_Player>();

        // Removing unnecessary targets in list first. If target is null or not active, remove it from the list.
        for (int i = 0; i < dD_Players.Count; i++) {

            if (dD_Players[i] != null)
                targetsInZone.Add(dD_Players[i]);

        }

        // Removing unnecessary targets in list first. If target is null or not active, remove it from the list.
        for (int i = 0; i < targetsInZone.Count; i++) {

            if (targetsInZone[i] != null && !targetsInZone[i].Alive && !targetsInZone[i].gameObject.activeInHierarchy)
                targetsInZone.RemoveAt(i);

            else {

                //  If distance to the target is far away, remove it from the list.
                if (targetsInZone[i] != null && Vector3.Distance(transform.position, targetsInZone[i].transform.position) > (detectorRadius))
                    targetsInZone.RemoveAt(i);

            }

        }

        // If there is a target in the zone, get closest enemy.
        if (targetsInZone.Count > 0)
            targetChase = GetClosestEnemy(targetsInZone.ToArray());
        else
            targetChase = null;

    }

    /// <summary>
    /// Gets the closest enemy.
    /// </summary>
    /// <param name="enemies"></param>
    /// <returns></returns>
    private CCDS_Player GetClosestEnemy(CCDS_Player[] enemies) {

        CCDS_Player bestTarget = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (CCDS_Player potentialTarget in enemies) {

            if (potentialTarget.felony >= 25f) {

                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;

                if (dSqrToTarget < closestDistanceSqr) {

                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;

                }

            }

        }

        return bestTarget;

    }

    /// <summary>
    /// Feeding the RCC with throttle, brake, steer, and handbrake inputs.
    /// </summary>
    private void FeedRCC() {

        RCCP_Inputs inputs = new RCCP_Inputs();

        inputs.throttleInput = throttleInput;
        inputs.brakeInput = brakeInput;
        inputs.steerInput = steerInput;
        inputs.handbrakeInput = handbrakeInput;

        CarController.Inputs.OverrideInputs(inputs);

    }

    /// <summary>
    /// Stops the vehicle immediately.
    /// </summary>
    private void Stop() {

        throttleInput = 0f;
        brakeInput = 0f;
        steerInput = 0f;
        handbrakeInput = 1f;

    }

    /// <summary>
    /// Vehicle will try to go backwards if crashed or stucked.
    /// </summary>
    private void CheckReset() {

        // If unable to move forward, puts the gear to R.
        if (Mathf.Abs(CarController.speed) <= 5 && transform.InverseTransformDirection(CarController.Rigid.linearVelocity).z <= 1f)
            resetTime += Time.deltaTime;

        //  If car is stucked for 2 seconds, reverse now.
        if (resetTime >= 2)
            reversingNow = true;

        //  If car is stucked for 4 seconds, or speed exceeds 25, go forward.
        if (resetTime >= 4 || Mathf.Abs(CarController.speed) >= 25) {

            reversingNow = false;
            resetTime = 0;

        }

    }

    /// <summary>
    /// On collision enter.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision) {

        //  If it's not alive, return.
        if (!IsAlive)
            return;

        if (collision.relativeVelocity.magnitude < 5)
            return;

        damage += (collision.impulse.magnitude / 1200f) * damageMP;

        if (damage > 100f) {

            damage = 100f;
            Wrecked();

        }

        CCDS_Events.Event_OnCollision(collision.impulse.magnitude);

    }

    private void OnDisable() {

        //  Disabling the healthbar if active.
        if (CCDS_Settings.Instance.showHealthBar && HealthBar)
            HealthBar.gameObject.SetActive(false);

        //  Disabling the engine smoke if active.
        if (CCDS_Settings.Instance.showEngineSmoke && EngineSmoke)
            EngineSmoke.gameObject.SetActive(false);

    }

    private void Reset() {

        if (sirenAudio == null)
            sirenAudio = RCCP_AudioSource.NewAudioSource(gameObject, "AudioSource_Siren", 10f, 100f, .65f, CCDS_Settings.Instance.policeSirenAudio, true, false, false);

        policeSiren = GetComponentInChildren<RCCP_PoliceLights>();

        if (policeSiren == null && CCDS_Settings.Instance.policeSiren != null) {

            policeSiren = Instantiate(CCDS_Settings.Instance.policeSiren.gameObject, transform).GetComponent<RCCP_PoliceLights>();
            policeSiren.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            policeSiren.transform.localPosition += Vector3.up * 1.5f;

        }

    }

}
