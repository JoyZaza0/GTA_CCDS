using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CCDS_URPLensFlare : MonoBehaviour {

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
    /// Max flare brigthness of the light.
    /// </summary>
    [Range(0f, 10f)] public float flareBrightness = 1.5f;

    /// <summary>
    /// Calculated final flare brightness of the light.
    /// </summary>
    private float finalFlareBrightness = 0f;

    private void Awake() {

        lensFlare_SRP = GetComponent<LensFlareComponentSRP>();

    }

    private void Update() {

        if (!lensFlare_SRP)
            return;

        //  Finding the main camera.
        Camera mainCam = Camera.main;

        //  If no main camera found, return.
        if (!mainCam)
            return;

        //  Lensflares are not affected by collider of the vehicle. They will ignore it. Below code will calculate the angle of the light-camera, and sets intensity of the lensflare.
        float distanceTocam = Vector3.Distance(transform.position, mainCam.transform.position);
        float angle = Vector3.Angle(transform.forward, mainCam.transform.position - transform.position);

        if (!Mathf.Approximately(angle, 0f))
            finalFlareBrightness = flareBrightness * (1f - Mathf.InverseLerp(0f, 600f, distanceTocam)) * ((300f - (3f * angle)) / 300f) / 3f;
        else
            finalFlareBrightness = flareBrightness;

        if (finalFlareBrightness < 0)
            finalFlareBrightness = 0f;

        lensFlare_SRP.attenuationByLightShape = false;
        lensFlare_SRP.intensity = finalFlareBrightness * LightSource.intensity;

    }
}
