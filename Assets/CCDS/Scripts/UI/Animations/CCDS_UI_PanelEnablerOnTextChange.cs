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
/// Enables the target UI panel on text changes.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/Animations/CCDS UI Panel Enabler On Text Change")]
[RequireComponent(typeof(TextMeshProUGUI))]
public class CCDS_UI_PanelEnablerOnTextChange : ACCDS_Component {

    /// <summary>
    /// Target gameplay panel.
    /// </summary>
    public CanvasGroup targetPanel;

    /// <summary>
    /// Text.
    /// </summary>
    public TextMeshProUGUI[] text;

    /// <summary>
    /// Old text used to detect text changes.
    /// </summary>
    public string[] oldText;

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
        if (text.Length < 1)
            text = GetComponentsInChildren<TextMeshProUGUI>();

        //  Return if no text found.
        if (text.Length < 1)
            return;

        // Creating old text array.
        if (oldText.Length < 1)
            oldText = new string[text.Length];

        //  Getting default scale of the text.
        if (defaultScale == Vector3.zero)
            defaultScale = targetPanel.transform.localScale;

        for (int i = 0; i < text.Length; i++) {

            //  If text changed, set timer to 1 for interation.
            if (text[i].text != oldText[i])
                timer = 1f;

            //  Setting old text string.
            oldText[i] = text[i].text;

        }

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
            targetPanel.alpha = Mathf.Lerp(targetPanel.alpha, 0f, Time.unscaledDeltaTime * 5f);

        }

    }

    /// <summary>
    /// Pops the text by changing scale of the text.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pop() {

        interacting = true;

        targetPanel.transform.localScale *= 1.2f;
        targetPanel.alpha = 0f;

        float time = 1f;

        while (time > 0f) {

            time -= Time.deltaTime;

            targetPanel.transform.localScale = Vector3.Lerp(targetPanel.transform.localScale, defaultScale, Time.unscaledDeltaTime * 5f);
            targetPanel.alpha = Mathf.Lerp(targetPanel.alpha, 1f, Time.unscaledDeltaTime * 5f);

            yield return null;

        }

        targetPanel.transform.localScale = defaultScale;
        targetPanel.alpha = 1f;

    }

}
