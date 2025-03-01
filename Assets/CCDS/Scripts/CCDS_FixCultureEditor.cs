//----------------------------------------------
//        City Car Driving Simulator
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using System.Globalization;
using System.Threading;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[InitializeOnLoad]
public static class CCDS_FixCultureEditor {

    static CCDS_FixCultureEditor() {

        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

    }

}
#endif

public static class FixCultureRuntime {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void FixCulture() {

        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

    }

}
