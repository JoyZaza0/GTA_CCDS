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
using UnityEngine.UI;

/// <summary>
/// UI image highlighter on mouse cursor enters / exits.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/Animations/CCDS UI Slider Highlighter")]
public class CCDS_UI_SliderHighlighter : ACCDS_Component, IPointerEnterHandler, IPointerExitHandler {

    /// <summary>
    /// Image component.
    /// </summary>
    public Image slider;

    /// <summary>
    /// Default position of the image.
    /// </summary>
    public Vector3 defaultPosition = Vector3.zero;

    /// <summary>
    /// Highlighting now?
    /// </summary>
    public bool highlighting = false;

    /// <summary>
    /// Animating now?
    /// </summary>
    public bool animating = false;

    /// <summary>
    /// Animated once?
    /// </summary>
    public bool animatedOnce = false;

    private void Awake() {

        if (!slider) {

            Debug.LogError("Image is not selected on this component named " + transform.name + "!");
            enabled = false;
            return;

        }

        //  Getting default local position.
        defaultPosition = slider.rectTransform.localPosition;

    }

    private void OnEnable() {

        //  Not highlighting on enable or disable.
        highlighting = false;

        // Disabling the animating.
        animating = false;

        //  Disabling animating once.
        animatedOnce = false;

        //  Disabling the image.
        slider.gameObject.SetActive(false);

    }

    private void OnDisable() {

        //  Not highlighting on enable or disable.
        highlighting = false;

        // Disabling the animating.
        animating = false;

        //  Disabling animating once.
        animatedOnce = false;

        //  Disabling the image.
        slider.gameObject.SetActive(false);

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

        //  Disabling animating once.
        animatedOnce = false;

    }

    private void Update() {

        //  Animate the image from left to right.
        if (!animating && highlighting && !animatedOnce)
            StartCoroutine(Highlight());

    }

    /// <summary>
    /// Animate the image from left to right.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Highlight() {

        animating = true;
        animatedOnce = true;

        slider.gameObject.SetActive(true);
        slider.rectTransform.localPosition = defaultPosition;
        slider.rectTransform.localPosition += Vector3.right * -300f;

        float time = 1f;

        while (time > 0) {

            float prevTime = time;
            time -= Time.deltaTime;
            slider.rectTransform.Translate(Vector3.right * (Time.unscaledDeltaTime) * 4500f);
            yield return null;

        }

        slider.gameObject.SetActive(false);

        animating = false;

        yield return null;

    }

}
