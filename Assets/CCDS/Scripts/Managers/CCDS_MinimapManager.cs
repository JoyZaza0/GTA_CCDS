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
/// Minimap manager.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Managers/CCDS Minimap Manager")]
public class CCDS_MinimapManager : ACCDS_Manager {

    private static CCDS_MinimapManager instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static CCDS_MinimapManager Instance {

        get {

            if (instance == null)
                instance = FindFirstObjectByType<CCDS_MinimapManager>();

            return instance;

        }

    }

    private CCDS_Minimap_Camera minimapCamera;

    /// <summary>
    /// Actual minimap camera.
    /// </summary>
    public CCDS_Minimap_Camera MinimapCamera {

        get {

            if (minimapCamera == null)
                minimapCamera = GetComponentInChildren<CCDS_Minimap_Camera>();

            if (minimapCamera == null) {

                minimapCamera = Instantiate(CCDS_Settings.Instance.minimapCamera.gameObject, transform).GetComponent<CCDS_Minimap_Camera>();
                minimapCamera.transform.name = CCDS_Settings.Instance.minimapCamera.transform.name;
                minimapCamera.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                minimapCamera.transform.position += Vector3.up * height;
                minimapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            }

            return minimapCamera;

        }

    }


    /// <summary>
    /// Height of the camera.
    /// </summary>
    public float height = 100f;

    /// <summary>
    /// Position offset.
    /// </summary>
    public Vector3 positionOffset = Vector3.zero;

    /// <summary>
    /// Registered vehicles with minimap icons. Used to add / remove minimap icons on all vehicles in the scene.
    /// </summary>
    public List<RCCP_CarController> registeredVehicles = new List<RCCP_CarController>();

    private void Update() {

        if (!CCDS_Settings.Instance.showMinimapIcons)
            return;

        //  Getting legendary RCCP Scene Manager, welcome sir!
        RCCP_SceneManager sceneManager = RCCP_SceneManager.Instance;

        //  If legendary RCCP Scene Manager is not here, no need to go further.
        if (!sceneManager)
            return;

        //  Getting gameplay manager to detect the player vehicle.
        CCDS_GameplayManager gameplayManager = CCDS_GameplayManager.Instance;

        //  Creating the list if it's null.
        if (registeredVehicles == null)
            registeredVehicles = new List<RCCP_CarController>();

        //  Looping all the vehicles in the scene.
        if (sceneManager.allVehicles != null && sceneManager.allVehicles.Count > 0) {

            for (int i = 0; i < sceneManager.allVehicles.Count; i++) {

                if (sceneManager.allVehicles[i] != null) {

                    //  If vehicle is not registered before, create and add the minimap icon.
                    if (!registeredVehicles.Contains(sceneManager.allVehicles[i])) {

                        //  If vehicle is player vehicle, add player icon. Otherwise add opponent icon. And register the vehicle. We don't wanna have multiple icons per vehicle, that's why we're registering them. 
                        if (sceneManager.allVehicles[i].gameObject.activeInHierarchy) {

                            bool isPlayerVehicle = false;
                            bool isCopVehicle = false;

                            if (gameplayManager != null && gameplayManager.player != null && Equals(sceneManager.allVehicles[i].gameObject, gameplayManager.player.gameObject))
                                isPlayerVehicle = true;

                            if (sceneManager.allVehicles[i].TryGetComponent(out CCDS_AI_Cop cop))
                                isCopVehicle = true;

                            CCDS_MinimapItem icon = Instantiate(isPlayerVehicle ? CCDS_Settings.Instance.minimapIconForPlayerVehicle : (!isCopVehicle ? CCDS_Settings.Instance.minimapIconForOpponentVehicle : CCDS_Settings.Instance.minimapIconForPoliceVehicle), sceneManager.allVehicles[i].transform);
                            icon.SetRoot(sceneManager.allVehicles[i].transform);
                            icon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

                            registeredVehicles.Add(sceneManager.allVehicles[i]);

                        }

                    }

                }

            }

        }

    }

    private void LateUpdate() {

        if (!CCDS_Settings.Instance.showMinimapIcons)
            return;

        //  If no minimap camera selected, return.
        if (!MinimapCamera) {

            Debug.LogError("Minimap camera is not selected on the " + transform.name + "! Please select the minimap camera if you're going to use it...");
            return;

        }

        //  Return if gameplay manager not found.
        if (!CCDS_GameplayManager.Instance)
            return;

        //  Getting the player.
        CCDS_Player player = CCDS_GameplayManager.Instance.player;

        //  If no player found, return.
        if (!player)
            return;

        //  Setting position and rotation of the minimap camera depending on the player vehicle position and rotation.
        MinimapCamera.transform.position = player.transform.position;
        MinimapCamera.transform.position += Vector3.up * height;
        MinimapCamera.transform.rotation = Quaternion.Euler(90f, player.transform.rotation.eulerAngles.y, 0f);
        MinimapCamera.transform.localPosition += MinimapCamera.transform.rotation * positionOffset;

    }

    private void Reset() {

        CCDS_Minimap_Camera mCamera = MinimapCamera;

    }

}
