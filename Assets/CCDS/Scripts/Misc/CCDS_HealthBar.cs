//----------------------------------------------
//        City Car Driving Simulator
//
// Copyright � 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI healthbar with slider. Must be child object of a vehicle.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Misc/CCDS Health Bar")]
public class CCDS_HealthBar : ACCDS_Component {

    private ACCDS_Vehicle vehicle;

    /// <summary>
    /// Vehicle.
    /// </summary>
    private ACCDS_Vehicle Vehicle {

        get {

            if (vehicle == null)
                vehicle = GetComponentInParent<ACCDS_Vehicle>();

            return vehicle;

        }

    }

    /// <summary>
    /// Damage slider.
    /// </summary>
    private Slider damageSlider;

    public Slider DamageSlider {

        get {

            if (damageSlider == null)
                damageSlider = GetComponentInChildren<Slider>();

            return damageSlider;

        }

    }

    /// <summary>
    /// Local position offset.
    /// </summary>
    public Vector3 positionOffset = new Vector3(0f, 1.5f, 0f);

    private void Update() {

        //  Disable the slider if no vehicle found and return.
        if (!Vehicle) {

            if (DamageSlider.gameObject.activeSelf) {

                DamageSlider.SetValueWithoutNotify(0f);
                DamageSlider.gameObject.SetActive(false);

            }

            return;

        }

        //  Enabling the slider.
        if (!DamageSlider.gameObject.activeSelf)
            DamageSlider.gameObject.SetActive(true);

        //  Set value of the slider.
        if (DamageSlider.gameObject.activeSelf)
            DamageSlider.SetValueWithoutNotify(1f - Mathf.InverseLerp(0f, 100f, Vehicle.damage));
        else
            DamageSlider.SetValueWithoutNotify(0f);

        //  Set position and rotation of the slider.
        transform.position = Vehicle.transform.position;
        transform.position += positionOffset;
        transform.rotation = Vehicle.transform.rotation;

        //  Rotate the slider along with main camera if found.
        //if (Camera.main)
        //    transform.rotation = Camera.main.transform.rotation;
	    if (RCCP_SceneManager.Instance.activePlayerCamera)
		    transform.rotation = RCCP_SceneManager.Instance.activePlayerCamera.transform.rotation;

    }

}
