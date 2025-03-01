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
/// Options panel used in UI.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/CCDS UI Options")]
public class CCDS_UI_Options : ACCDS_Component {

    /// <summary>
    /// Settings scheme.
    /// </summary>
    public GameObject settings;

    /// <summary>
    /// Controls scheme.
    /// </summary>
    public GameObject controls;

    /// <summary>
    /// Quality buttons in the options / settings menu.
    /// </summary>
    public Button lowQualityButton, medQualityButton, highQualityButton, ultraQualityButton;

    /// <summary>
    /// Controls button for disabling on mobile platform.
    /// </summary>
    public GameObject controlsButton;

    /// <summary>
    /// Audio and music sliders.
    /// </summary>
    public Slider audioSlider, musicSlider;

    private void Awake() {

        if (controlsButton)
            controlsButton.SetActive(!Application.isMobilePlatform);

    }

    private void OnEnable() {

        //  Enabling the settings panel on enable.
        if (settings)
            settings.SetActive(true);

        //  Disabling the controls panel on enable.
        if (controls)
            controls.SetActive(false);

        //  Checking quality buttons on enable.
        CheckQualityButtons();

        //  Checking audio sliders on enable.
        CheckAudioSliders();

        //  Listening quality changed event on enable.
        CCDS_Events.OnQualityChanged += CheckQualityButtons;

    }

    private void OnDisable() {

        //  Enabling the settings panel on disable.
        if (settings)
            settings.SetActive(true);

        //  Disabling the controls panel on disable.
        if (controls)
            controls.SetActive(false);

        //  Not listening quality changed event on disable.
        CCDS_Events.OnQualityChanged -= CheckQualityButtons;

    }

    /// <summary>
    /// Opens the target panel.
    /// </summary>
    /// <param name="activePanel"></param>
    public void OpenPanel(GameObject activePanel) {

        //  Disabling the settings panel.
        if (settings)
            settings.SetActive(false);

        //  Disabling the controls panel.
        if (controls)
            controls.SetActive(false);

        activePanel.SetActive(true);

    }

    /// <summary>
    /// Checks the audio and music sliders. Sets their values without notfying them.
    /// </summary>
    public void CheckAudioSliders() {

        if (audioSlider)
            audioSlider.SetValueWithoutNotify(CCDS.GetAudioVolume());

        if (musicSlider)
            musicSlider.SetValueWithoutNotify(CCDS.GetMusicVolume());

    }

    /// <summary>
    /// Checks the quality buttons. Sets their states depending on the quality settings.
    /// </summary>
    public void CheckQualityButtons() {

        //  Changing the color of the target graphic.
        switch (QualitySettings.GetQualityLevel()) {

            case 0:

                if (lowQualityButton)
                    lowQualityButton.targetGraphic.color = lowQualityButton.colors.normalColor;

                if (medQualityButton)
                    medQualityButton.targetGraphic.color = medQualityButton.colors.disabledColor;

                if (highQualityButton)
                    highQualityButton.targetGraphic.color = highQualityButton.colors.disabledColor;

                if (ultraQualityButton)
                    ultraQualityButton.targetGraphic.color = ultraQualityButton.colors.disabledColor;

                break;

            case 1:

                if (lowQualityButton)
                    lowQualityButton.targetGraphic.color = lowQualityButton.colors.disabledColor;

                if (medQualityButton)
                    medQualityButton.targetGraphic.color = medQualityButton.colors.normalColor;

                if (highQualityButton)
                    highQualityButton.targetGraphic.color = highQualityButton.colors.disabledColor;

                if (ultraQualityButton)
                    ultraQualityButton.targetGraphic.color = ultraQualityButton.colors.disabledColor;

                break;

            case 2:

                if (lowQualityButton)
                    lowQualityButton.targetGraphic.color = lowQualityButton.colors.disabledColor;

                if (medQualityButton)
                    medQualityButton.targetGraphic.color = medQualityButton.colors.disabledColor;

                if (highQualityButton)
                    highQualityButton.targetGraphic.color = highQualityButton.colors.normalColor;

                if (ultraQualityButton)
                    ultraQualityButton.targetGraphic.color = ultraQualityButton.colors.disabledColor;

                break;

            case 3:

                if (lowQualityButton)
                    lowQualityButton.targetGraphic.color = lowQualityButton.colors.disabledColor;

                if (medQualityButton)
                    medQualityButton.targetGraphic.color = medQualityButton.colors.disabledColor;

                if (highQualityButton)
                    highQualityButton.targetGraphic.color = highQualityButton.colors.disabledColor;

                if (ultraQualityButton)
                    ultraQualityButton.targetGraphic.color = ultraQualityButton.colors.normalColor;

                break;

        }

    }

    /// <summary>
    /// Sets the volume of the audio.
    /// </summary>
    /// <param name="slider"></param>
    public void SetAudioVolume(Slider slider) {

        CCDS.SetAudioVolume(slider.value);

        //  Calling an event on audio changed.
        CCDS_Events.Event_OnAudioChanged();

    }

    /// <summary>
    /// Sets the volume of the music.
    /// </summary>
    /// <param name="slider"></param>
    public void SetMusicVolume(Slider slider) {

        CCDS.SetMusicVolume(slider.value);

        //  Calling an event on audio changed.
        CCDS_Events.Event_OnAudioChanged();

    }

    /// <summary>
    /// Sets the level of the quality.
    /// </summary>
    /// <param name="level"></param>
    public void SetQualityLevel(int level) {

        //  Sets the level of the quality.
        QualitySettings.SetQualityLevel(level);

        //  Calling an event on quality changed.
        CCDS_Events.Event_OnQualityChanged();

    }

}
