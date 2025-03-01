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

/// <summary>
/// UI layout group element popper. Adjusts the element's scale for popup effect. Must be attached to the layout group.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/Animations/CCDS UI Layout Group Element Popper")]
public class CCDS_UI_LayoutGroupElementPopper : ACCDS_Component {

    /// <summary>
    /// Initialized before? Make sure to initialize it once...
    /// </summary>
    private bool initialized = false;

    /// <summary>
    /// All elements.
    /// </summary>
    private Transform[] elements;

    /// <summary>
    /// Their default scales.
    /// </summary>
    private Vector3[] defaultScale;

    private void OnEnable() {

        if (initialized) {

            //  If elements found...
            if (elements.Length > 0) {

                //  Looping all elements.
                for (int i = 0; i < elements.Length; i++) {

                    //  If element is not nul...
                    if (elements[i] != null) {

                        //  Get default scale if there is not.
                        elements[i].localScale = defaultScale[i];

                        //  Popup effect.
                        StartCoroutine(Pop(elements[i], i, defaultScale[i]));

                    }

                }

            }

            return;

        }

        initialized = true;

        // Getting all children elements.
        elements = transform.GetComponentsInChildren<Transform>(false);

        //  Getting their scales.
        defaultScale = new Vector3[elements.Length];

        //  If elements found...
        if (elements.Length < 1)
            return;

        //  Looping all elements.
        for (int i = 0; i < elements.Length; i++) {

            //  If element is not nul...
            if (elements[i] != null) {

                //  Get default scale if there is not.
                if (defaultScale[i] == Vector3.zero)
                    defaultScale[i] = elements[i].localScale;

                //  Popup effect.
                StartCoroutine(Pop(elements[i], i, defaultScale[i]));

            }

        }

    }

    /// <summary>
    /// Popup effect.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="waitForSeconds"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public IEnumerator Pop(Transform element, float waitForSeconds, Vector3 scale) {

        yield return new WaitForSeconds((float)waitForSeconds / 25f);

        element.localScale *= 1.1f;
        float time = 1.5f;

        while (time > 0f) {

            time -= Time.deltaTime;
            element.localScale = Vector3.Lerp(element.localScale, scale, Time.unscaledDeltaTime * 5f);

            yield return null;

        }

        element.localScale = scale;

    }

    private void OnDisable() {

        if (initialized) {

            //  If elements found...
            if (elements.Length > 0) {

                //  Looping all elements.
                for (int i = 0; i < elements.Length; i++) {

                    //  If element is not nul...
                    if (elements[i] != null)
                        elements[i].localScale = defaultScale[i];

                }

            }

            return;

        }

    }

}
