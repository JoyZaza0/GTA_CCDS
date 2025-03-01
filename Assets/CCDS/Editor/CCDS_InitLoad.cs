//----------------------------------------------
//        City Car Driving Simulator
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CCDS_InitLoad {

    [InitializeOnLoadMethod]
    static void InitOnLoad() {

        EditorApplication.delayCall += EditorUpdate;

    }

    public static void EditorUpdate() {

        //CCDS_SetScriptingSymbol.SetEnabled("BCG_CCDS", false);

        bool hasKey = false;

#if BCG_CCDS
        hasKey = true;
#endif

        if (!hasKey) {

            EditorUtility.DisplayDialog("Regards from BoneCracker Games", "Thank you for purchasing and using City Car Driving Simulator. Please read the documentation before use. Also check out the online documentation for updated info. Have fun :)", "Let's get started!");

            CCDS_WelcomeWindow.OpenWindow();

            if (EditorUtility.DisplayDialog("Restart Unity", "Please restart Unity after importing the package. Otherwise inputs may not work for the first time.", "Restart Unity After Compile", "Don't Restart"))
                EditorApplication.OpenProject(Directory.GetCurrentDirectory());

            CCDS_SetScriptingSymbol.SetEnabled("BCG_CCDS", true);

        }

    }

}
