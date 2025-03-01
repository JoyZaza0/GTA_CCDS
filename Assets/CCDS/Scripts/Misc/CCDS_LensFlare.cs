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
using UnityEngine.Rendering;

/// <summary>
/// SRP Lens Flare adjuster. Must be attached to the light.
/// </summary>
[RequireComponent(typeof(Light))]
[RequireComponent(typeof(LensFlareComponentSRP))]
[AddComponentMenu("BoneCracker Games/CCDS/Misc/CCDS Lensflare")]
public class CCDS_LensFlare : ACCDS_Component {

    private Light _lightSource;

    /// <summary>
    /// Light source.
    /// </summary>
    private Light LightSource {

        get {

            if (_lightSource == null)
                _lightSource = GetComponent<Light>();

            return _lightSource;

        }

    }

    /// <summary>
    /// SRP Lens flare for URP / HDRP.
    /// </summary>
    private LensFlareComponentSRP lensFlare_SRP;

    /// <summary>
    /// Calculated final flare brightness of the light.
    /// </summary>
    private float finalFlareBrightness = 0f;

    /// <summary>
    /// Max flare brigthness of the light.
    /// </summary>
    [Range(0f, 10f)] public float flareBrightness = 1.5f;

    /// <summary>
    /// Use camera angle to set intensity of the flare?
    /// </summary>
    public bool useCameraAngle = true;

    private void Awake() {

        //  Getting lensflare component.
        lensFlare_SRP = GetComponent<LensFlareComponentSRP>();

    }

    private void Update() {

        if (lensFlare_SRP)
            LensFlare_SRP();

    }

    /// <summary>
    /// Operating SRP lensflare related to camera angle.
    /// </summary>
    private void LensFlare_SRP() {

        //  If no main camera found, return.
        if (!Camera.main)
            return;

        //  Lensflares are not affected by collider of the vehicle. They will ignore it. Below code will calculate the angle of the light-camera, and sets intensity of the lensflare.
        float distanceTocam = Vector3.Distance(transform.position, Camera.main.transform.position);
        float angle = Vector3.Angle(transform.forward, Camera.main.transform.position - transform.position);

        if (!Mathf.Approximately(angle, 0f))
            finalFlareBrightness = flareBrightness * (1f - Mathf.InverseLerp(0f, 600f, distanceTocam)) * ((300f - (3f * angle)) / 300f) / 3f;
        else
            finalFlareBrightness = flareBrightness;

        if (finalFlareBrightness < 0)
            finalFlareBrightness = 0f;

        if (!useCameraAngle)
            finalFlareBrightness = 1f;

        lensFlare_SRP.attenuationByLightShape = false;
        lensFlare_SRP.intensity = finalFlareBrightness * LightSource.intensity;

    }

}
