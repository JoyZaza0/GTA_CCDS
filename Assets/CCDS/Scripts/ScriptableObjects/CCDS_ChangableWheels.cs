//----------------------------------------------
//        City Car Driving Simulator
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Changes wheels at runtime. It holds changable wheels as prefab in an array.
/// </summary>
[System.Serializable]
public class CCDS_ChangableWheels : ScriptableObject {

    #region singleton
    private static CCDS_ChangableWheels instance;
    public static CCDS_ChangableWheels Instance { get { if (instance == null) instance = Resources.Load("CCDS_ChangableWheels") as CCDS_ChangableWheels; return instance; } }
    #endregion

    [System.Serializable]
    public class ChangableWheels {

        public GameObject wheel;

    }

    /// <summary>
    /// All changable wheels.
    /// </summary>
    public ChangableWheels[] wheels;

}


