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
/// Trailblazer obstacle component. Must be attached to the trailblazer obstacle.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Missions/CCDS MissionObjective Trailblazer Item")]
public class CCDS_MissionObjective_TrailblazerItem : ACCDS_Component {

    /// <summary>
    /// Audioclip will be played on collision.
    /// </summary>
    public AudioClip collisionClip;

    /// <summary>
    /// Collided yet?
    /// </summary>
    public bool collided = false;

    /// <summary>
    /// Rigidbody component used to add force after the collision.
    /// </summary>
    private Rigidbody rigid;

    /// <summary>
    /// Collider component used to disable collider after the collision..
    /// </summary>
    private Collider col;

    /// <summary>
    /// Add x amount of seconds after colliding with.
    /// </summary>
    public bool addSeconds = true;

    /// <summary>
    /// Add x amount of seconds after colliding with.
    /// </summary>
    public int seconds = 1;

    private void OnEnable() {

        //  Getting rigidbody.
        if (!rigid)
            rigid = GetComponent<Rigidbody>();

        //  Getting collider.
        if (!col)
            col = GetComponentInChildren<Collider>();

        //  Restoring values back to the original on enable.
        Restart();

    }

    /// <summary>
    /// Restores values back to the original ones.
    /// </summary>
    public void Restart() {

        //  Getting rigidbody.
        if (!rigid)
            rigid = GetComponent<Rigidbody>();

        //  Getting collider.
        if (!col)
            col = GetComponentInChildren<Collider>();

        col.isTrigger = true;
        rigid.isKinematic = true;
        collided = false;

    }

    /// <summary>
    /// On trigger enter.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {

        //  Return if not collided yet.
        if (collided)
            return;

        //  Getting triggered object.
        CCDS_Player player = other.GetComponentInParent<CCDS_Player>();

        //  If it's not player, return.
        if (!player)
            return;

        //  If triggered object is actual player vehicle, collide.
        if (CCDS_GameplayManager.Instance && Equals(CCDS_GameplayManager.Instance.player.gameObject, player.gameObject))
            Collide(player.transform);

    }

    /// <summary>
    /// Collision with player.
    /// </summary>
    private void Collide(Transform playerTransform) {

        //  Collided to true.
        collided = true;

        // Playing audiclip if selected.
        if (collisionClip)
            RCCP_AudioSource.NewAudioSource(gameObject, collisionClip.name, 0f, 0f, 1f, collisionClip, false, true, true);

        //  Disabling the trigger option and enabling the rigidbody.
        col.isTrigger = false;
        rigid.isKinematic = false;

        //  Applying force to upwards.
        rigid.AddForce(Vector3.up * 3f, ForceMode.Impulse);

        //  Applying force to collision direction.
        rigid.AddForce((playerTransform.position - transform.position) * -2f, ForceMode.Impulse);

        //  If add seconds.
        if (addSeconds)
            CCDS_GameplayManager.Instance.AddTime(seconds);

    }

}
