//----------------------------------------------
//        City Car Driving Simulator
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Camera orbit used in the main menu.
/// </summary>
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AudioListener))]
[AddComponentMenu("BoneCracker Games/CCDS/Cameras/CCDS Camera Orbit")]
public class CCDS_Camera_Orbit : ACCDS_Component {

    /// <summary>
    /// Orbiting now?
    /// </summary>
    private bool orbiting = true;

    /// <summary>
    /// Orbiting timer used to reenable orbiting bool.
    /// </summary>
    private float orbitingTimer = 0f;

    /// <summary>
    /// Target gameobject to track.
    /// </summary>
    public GameObject target;

    /// <summary>
    /// Distance to the target.
    /// </summary>
    public float distance = 7.5f;

    /// <summary>
    /// X speed.
    /// </summary>
    public float xSpeed = 150f;

    /// <summary>
    /// Y speed.
    /// </summary>
    public float ySpeed = 100f;

    /// <summary>
    /// Orbit speed.
    /// </summary>
    [Range(0f, 1f)] public float orbitSpeed = .05f;

    /// <summary>
    /// Minimum Y angle limit.
    /// </summary>
    public float yMinLimit = 10f;

    /// <summary>
    /// Maximum Y angle limit.
    /// </summary>
    public float yMaxLimit = 45f;

    /// <summary>
    /// Position offset.
    /// </summary>
    public Vector3 offset = new Vector3(0f, -.45f, 0f);

    /// <summary>
    /// X input.
    /// </summary>
    private float x = 0f;

    /// <summary>
    /// Y input.
    /// </summary>
    private float y = 0f;

    /// <summary>
    /// X smoothed input.
    /// </summary>
    private float x_Smoothed = 0f;

    /// <summary>
    /// Y smoothed input.
    /// </summary>
    private float y_Smoothed = 0f;

    private void OnEnable() {

        //  Getting initial values.
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

    }

    private void LateUpdate() {

        //  If drag input is not zero, continue...
        if (CCDS_UI_Drag.dragInput != Vector2.zero) {

            //  Disabling auto orbiting for a short time.
            orbiting = false;
            orbitingTimer = 1.5f;

        }

        //  Setting orbiting timer.
        if (orbitingTimer > 0f) {

            orbitingTimer -= Time.deltaTime;

        } else {

            orbitingTimer = 0f;
            orbiting = true;

        }

        //  If orbiting is enabled, increase X input.
        if (orbiting)
            x += orbitSpeed * xSpeed * Time.deltaTime;

        //  Setting inputs.
        x += CCDS_UI_Drag.dragInput.x * xSpeed * 0.02f;
        y += CCDS_UI_Drag.dragInput.y * ySpeed * 0.02f;

        //  Resetting the inputs on CCDS_UI_Drag after getting them.
        CCDS_UI_Drag.ResetInputs();

        //  Limiting the Y angle.
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        //  Smoothing inputs.
        x_Smoothed = Mathf.Lerp(x_Smoothed, x, Time.deltaTime * 10f);
        y_Smoothed = Mathf.Lerp(y_Smoothed, y, Time.deltaTime * 10f);

        //  Calculating target rotation and position.
        Quaternion rotation = Quaternion.Euler(y_Smoothed, x_Smoothed, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
        position += offset;

        //  And finally setting position and rotation of the camera.
        transform.rotation = rotation;
        transform.position = position;

    }

    /// <summary>
    /// Clamps the angle with min and max values.
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private float ClampAngle(float angle, float min, float max) {

        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);

    }

    private void Reset() {

        if (target == null)
            target = FindFirstObjectByType<CCDS_SpawnPoint>().gameObject;

        GetComponent<Camera>().fieldOfView = 30f;

    }

}
