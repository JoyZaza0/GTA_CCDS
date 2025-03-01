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
/// Engine smoke used on higher damage.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Misc/CCDS Engine Smoke")]
public class CCDS_EngineSmoke : ACCDS_Component {

    /// <summary>
    /// Engine smoke particle system.
    /// </summary>
    public ParticleSystem engineSmokeParticle;

    /// <summary>
    /// Emission module of the engine smoke particle system.
    /// </summary>
    private ParticleSystem.EmissionModule em;

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
    /// Engine smoke particle system will be enabled over this damage limit.
    /// </summary>
    [Range(0f, 90f)] public float damageLimit = 60f;

    /// <summary>
    /// Minimum emission rate.
    /// </summary>
    [Range(0f, 1000f)] public float minimumEmission = 10f;

    /// <summary>
    /// Maximum emission rate.
    /// </summary>
    [Range(0f, 1000f)] public float maximumEmission = 100f;

    /// <summary>
    /// Calculated emission rate related to the vehicle damage.
    /// </summary>
    private float EmissionRate {

        get {

            if (vehicle)
                return Mathf.Lerp(minimumEmission, maximumEmission, Mathf.InverseLerp(0f, 100f, vehicle.damage));
            else
                return maximumEmission;

        }

    }

    /// <summary>
    /// Local position offset.
    /// </summary>
    public Vector3 positionOffset = new Vector3(0f, 0f, 1.8f);

    private void Update() {

        transform.localPosition = positionOffset;

        //  Return if engine smoke particle is not selected.
        if (!engineSmokeParticle)
            return;

        em = engineSmokeParticle.emission;

        //  Return if vehicle component is not found.
        if (!Vehicle) {

            em.enabled = false;
            return;

        }

        //  If damage is above the limit, enable the particle system. Otherwise disable...
        if (vehicle.damage > damageLimit) {

            em.enabled = true;
            em.rateOverTime = new ParticleSystem.MinMaxCurve(EmissionRate, EmissionRate);

        } else {

            em.enabled = false;

        }

    }

    private void Reset() {

        engineSmokeParticle = GetComponent<ParticleSystem>();
        transform.localPosition = positionOffset;

    }

}
