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
/// Minimap camera used with CCDS_MinimapManager.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Cameras/CCDS Minimap Camera")]
[RequireComponent(typeof(Camera))]
public class CCDS_Minimap_Camera : ACCDS_Component {

    private Camera _cam;

    /// <summary>
    /// Minimap camera.
    /// </summary>
    public Camera Cam {

        get {

            if (_cam == null)
                _cam = GetComponent<Camera>();

            return _cam;

        }

    }

}
