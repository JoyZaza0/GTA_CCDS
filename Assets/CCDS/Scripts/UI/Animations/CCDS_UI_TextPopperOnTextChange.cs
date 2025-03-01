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
using TMPro;

/// <summary>
/// Changes the scale of the text on text changes.
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
[AddComponentMenu("BoneCracker Games/CCDS/UI/Animations/CCDS UI Text Popper On Text Change")]
public class CCDS_UI_TextPopperOnTextChange : ACCDS_Component {

    /// <summary>
    /// Text.
    /// </summary>
    private TextMeshProUGUI text;

    /// <summary>
    /// Old text used to detect text changes.
    /// </summary>
    public string oldText = "";

    /// <summary>
    /// Timer.
    /// </summary>
    private float timer = 0f;

    /// <summary>
    /// Default scale of the text.
    /// </summary>
    private Vector3 defaultScale = Vector3.zero;

    /// <summary>
    /// Popping now?
    /// </summary>
    public bool interacting = false;

    private void OnEnable() {

        //  Setting timer and interacting to false on enable.
        timer = 0f;
        interacting = false;

    }

    private void OnDisable() {

        //  Setting timer and interacting to false on disable.
        timer = 0f;
        interacting = false;

    }

    private void LateUpdate() {

        //  Getting text component if not found yet.
        if (!text)
            text = GetComponent<TextMeshProUGUI>();

        //  Return if no text found.
        if (!text)
            return;

        //  Getting default scale of the text.
        if (defaultScale == Vector3.zero)
            defaultScale = text.transform.localScale;

        //  If text changed, set timer to 1 for interation.
        if (text.text != oldText)
            timer = 1f;

        //  Setting old text string.
        oldText = text.text;

        //  If timer is above 0 second, interaction.
        if (timer > 0) {

            //  Consuming time.
            timer -= Time.unscaledDeltaTime * 3f;

            //  Interaction.
            if (!interacting)
                StartCoroutine(Pop());

        } else {

            timer = 0f;
            interacting = false;
            text.transform.localScale = Vector3.Lerp(text.transform.localScale, defaultScale, Time.unscaledDeltaTime * 5f);

        }

    }

    /// <summary>
    /// Pops the text by changing scale of the text.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pop() {

        interacting = true;

        text.transform.localScale *= 1.2f;
        float time = 1f;

        while (time > 0f) {

            time -= Time.deltaTime;
            text.transform.localScale = Vector3.Lerp(text.transform.localScale, defaultScale, Time.unscaledDeltaTime * 5f);

            yield return null;

        }

        text.transform.localScale = defaultScale;

    }

}
