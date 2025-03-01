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
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// UI text highlighter on mouse cursor enters / exits.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/Animations/CCDS UI Text Highlighter")]
public class CCDS_UI_TextHighlighter : ACCDS_Component, IPointerEnterHandler, IPointerExitHandler {

    /// <summary>
    /// Text component.
    /// </summary>
    public TextMeshProUGUI text;

    /// <summary>
    /// Default color of the text.
    /// </summary>
    public Color defaultColor = Color.white;

    /// <summary>
    /// Highlighted color of the text.
    /// </summary>
    public Color highlightColor = Color.black;

    /// <summary>
    /// Highlighting now?
    /// </summary>
    public bool highlighting = false;

    private void Awake() {

        //  Getting default color.
        defaultColor = text.color;

    }

    private void OnEnable() {

        //  Not highlighting on enable or disable.
        highlighting = false;

        //  Setting the color of the text to default color.
        text.color = defaultColor;

    }

    private void OnDisable() {

        //  Not highlighting on enable or disable.
        highlighting = false;

        //  Setting the color of the text to default color.
        text.color = defaultColor;

    }

    /// <summary>
    /// When mouse cursor enters.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData) {

        //  Enabling the highlighting.
        highlighting = true;

    }

    /// <summary>
    /// When mouse cursor exits.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData) {

        //  Disabling the highlighting.
        highlighting = false;

    }

    private void Update() {

        //  Set color of the text on highlight or not.
        if (highlighting)
            text.color = Color.Lerp(text.color, highlightColor, Time.unscaledDeltaTime * 25f);
        else
            text.color = Color.Lerp(text.color, defaultColor, Time.unscaledDeltaTime * 25f);

    }

    private void Reset() {

        //  Getting text component in children if exits.
        text = GetComponentInChildren<TextMeshProUGUI>(true);

        if (!text)
            Debug.LogError("Text component couldn't found for this " + transform.name + "! CCDS_UI_TextHighlighter needs a target text to work with. Please select the text component...");

        defaultColor = text.color;
        highlightColor = defaultColor;

    }

}
