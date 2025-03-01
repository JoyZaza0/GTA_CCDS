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
public abstract class ACCDS_Component : MonoBehaviour {

    public CCDS_SceneManager SceneManager {

        get {

            if (sceneManager == null)
                sceneManager = CCDS_SceneManager.Instance;

            return sceneManager;

        }

    }
    private CCDS_SceneManager sceneManager;

}
