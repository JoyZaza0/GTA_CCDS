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
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// UI drag with vector2 input. Must be attached to the UI image.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/CCDS UI Drag")]
[RequireComponent(typeof(Image))]
public class CCDS_UI_Drag : ACCDS_Component, IDragHandler {

    /// <summary>
    /// Input.
    /// </summary>
    public static Vector2 dragInput = Vector2.zero;

    private void OnEnable() {

        ResetInputs();

    }

    private void OnDisable() {

        ResetInputs();

    }

    /// <summary>
    /// Getting drag data via IDragHandler interface.
    /// </summary>
    /// <param name="pointerData"></param>
    public void OnDrag(PointerEventData pointerData) {

        // Receiving drag input from UI.
        dragInput.x += pointerData.delta.x * .02f;
        dragInput.y -= pointerData.delta.y * .02f;

    }

    /// <summary>
    /// Resets the vector input.
    /// </summary>
    public static void ResetInputs() {

        dragInput = Vector2.zero;

    }

}
