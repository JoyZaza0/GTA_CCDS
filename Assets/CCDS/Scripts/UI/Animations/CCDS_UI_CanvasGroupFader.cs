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
/// UI canvas fader on enable.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/Animations/CCDS UI Canvas Group Fader")]
public class CCDS_UI_CanvasGroupFader : ACCDS_Component {

    /// <summary>
    /// CanvasGroup component.
    /// </summary>
    private CanvasGroup canvasGroup;

    private void OnEnable() {

        //  Getting canvasgroup component.
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();

        //  Return if not found.
        if (!canvasGroup)
            return;

        //  Setting alpha of the canvasgroup to 0.
        canvasGroup.alpha = 0f;

        //  Fade the alpha value of the canvasgroup.
        StartCoroutine(Fade());

    }

    /// <summary>
    /// Fade the alpha value of the canvasgroup.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Fade() {

        // Time 1 second.
        float time = 1f;

        //  Fading the alpha value to 1.
        while (time > 0f) {

            time -= Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, Time.unscaledDeltaTime * 5f);
            yield return null;

        }

        //  Make sure the alpha value is set to 1 after fading.
        canvasGroup.alpha = 1f;

    }

    private void OnDisable() {

        //  Getting canvasgroup component.
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();

        //  Return if not found.
        if (!canvasGroup)
            return;

        //  Setting alpha of the canvasgroup to 0.
        canvasGroup.alpha = 0f;

    }

}
