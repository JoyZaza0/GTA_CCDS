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
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI key invoker with keyboard buttons.
/// </summary>
[RequireComponent(typeof(Button))]
[AddComponentMenu("BoneCracker Games/CCDS/UI/CCDS UI Key Invoker")]
public class CCDS_UI_KeyInvoker : ACCDS_Component {

    /// <summary>
    /// UI button.
    /// </summary>
    private Button uibutton;

    /// <summary>
    /// UI button.
    /// </summary>
    private Button UIButton {

        get {

            if (uibutton == null)
                uibutton = GetComponent<Button>();

            return uibutton;

        }

    }

    /// <summary>
    /// Button SFX.
    /// </summary>
    public AudioClip buttonSFX;

    /// <summary>
    /// Button SFX volume.
    /// </summary>
    [Range(0f, 1f)] public float buttonSFXVolume = .35f;

    /// <summary>
    /// Main invoker key.
    /// </summary>
    public KeyCode invokerKey = KeyCode.None;

    /// <summary>
    /// Secondary invoker key.
    /// </summary>
    public KeyCode invokerKeyAlternative = KeyCode.None;

    private void Awake() {

        //  Adding listener to the button click.
        UIButton.onClick.AddListener(() => OnClicked());

    }

    private void Update() {

        //  If main invoker key is not null and player pushes the button.
        if (invokerKey != KeyCode.None && Input.GetKeyDown(invokerKey))
            UIButton.onClick.Invoke();

        //  If secondary invoker key is not null and player pushes the button.
        if (invokerKeyAlternative != KeyCode.None && Input.GetKeyDown(invokerKeyAlternative))
            UIButton.onClick.Invoke();

    }

    /// <summary>
    /// On click.
    /// </summary>
    private void OnClicked() {

        //  Creating SFX, but it's ignoring audiolistener pause.
        if (buttonSFX != null) {

            AudioSource aS = RCCP_AudioSource.NewAudioSource(transform.root.gameObject, buttonSFX.name, 0f, 0f, buttonSFXVolume, buttonSFX, false, true, true);
            aS.ignoreListenerPause = true;
            aS.ignoreListenerVolume = false;

        }

    }

    private void OnDestroy() {

        //  Not listening the button click.
        UIButton.onClick.RemoveListener(() => OnClicked());

    }

}
