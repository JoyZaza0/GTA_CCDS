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
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages and observes all cop vehicles in the scene. All cop vehicles must be child of this transform.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Managers/CCDS Cop Manager")]
public class CCDS_CopsManager : ACCDS_Manager {

    private static CCDS_CopsManager instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static CCDS_CopsManager Instance {

        get {

            if (instance == null)
                instance = FindFirstObjectByType<CCDS_CopsManager>();

            return instance;

        }

    }

    /// <summary>
    /// All cop vehicles.
    /// </summary>
    public List<CCDS_AI_Cop> allCops = new List<CCDS_AI_Cop>();

    private void Awake() {

        //  Getting all cop vehicles.
        GetAllCops();

    }

    private void Update() {

        //  Checking all cops to restore if they have been killed.
        CheckCopsToRestore();

    }

    /// <summary>
    /// Checking all cops to restore if they have been killed.
    /// </summary>
    private void CheckCopsToRestore() {

        //  Creating a new list to store all vehicles.
        List<RCCP_CarController> allVehicles = new List<RCCP_CarController>();

        //  Getting all vehicles except cop vehicles.
        for (int i = 0; i < RCCP_SceneManager.Instance.allVehicles.Count; i++) {

            bool isCop = false;

            for (int k = 0; k < allCops.Count; k++) {

                if (allCops[k] != null) {

                    if (Equals(RCCP_SceneManager.Instance.allVehicles[i].gameObject, allCops[k].gameObject))
                        isCop = true;

                }

            }

            if (!isCop)
                allVehicles.Add(RCCP_SceneManager.Instance.allVehicles[i]);

        }

        //  If distance to other vehicles is over 200 meters and cop vehicles is wrecked, restore it.
        for (int i = 0; i < allCops.Count; i++) {

            bool canRestore = true;

            if (allCops[i] != null) {

                for (int k = 0; k < allVehicles.Count; k++) {

                    if (allVehicles[k] != null && !Equals(allCops[i].gameObject, allVehicles[k].gameObject)) {

                        float distance = Vector3.Distance(allVehicles[k].transform.position, allCops[i].transform.position);

                        if (distance < 200f)
                            canRestore = false;

                    }

                }

            }

            if (allCops[i].damage < 100)
                canRestore = false;

            if (canRestore)
                RestoreCop(allCops[i]);

        }

    }

    /// <summary>
    /// Restores the target cop vehicle.
    /// </summary>
    /// <param name="restoreCop"></param>
    private void RestoreCop(CCDS_AI_Cop restoreCop) {

        restoreCop.damage = 0f;
        restoreCop.CarController.canControl = true;
        restoreCop.CarController.Damage.repairNow = true;

        StartCoroutine(RestoreResetVehicle(restoreCop));

    }

    /// <summary>
    /// Reset position and rotation of the target cop vehicle.
    /// </summary>
    /// <param name="restoreCop"></param>
    /// <returns></returns>
    private IEnumerator RestoreResetVehicle(CCDS_AI_Cop restoreCop) {

        restoreCop.CarController.Rigid.interpolation = RigidbodyInterpolation.None;
        restoreCop.CarController.Rigid.isKinematic = true;
        RCCP.Transport(restoreCop.CarController, restoreCop.transform.position + Vector3.up * 2f, Quaternion.Euler(0f, restoreCop.transform.eulerAngles.y, 0f));

        yield return new WaitForFixedUpdate();

        restoreCop.CarController.Rigid.interpolation = RigidbodyInterpolation.Interpolate;
        restoreCop.CarController.Rigid.isKinematic = false;

    }

    /// <summary>
    /// Adding a new cop vehicle to the list.
    /// </summary>
    /// <param name="newCop"></param>
    public void AddNewCop(CCDS_AI_Cop newCop) {

        allCops.Add(newCop);

    }

    /// <summary>
    /// Getting all cop vehicles even if they are disabled.
    /// </summary>
    public void GetAllCops() {

        //  Checking the list. Creating if it's null.
        if (allCops == null)
            allCops = new List<CCDS_AI_Cop>();

        //  Clearing the list.
        allCops.Clear();

        //  Getting all cop vehicles.
        allCops = GetComponentsInChildren<CCDS_AI_Cop>(true).ToList();

    }

    private void Reset() {

        GetAllCops();

    }

}
