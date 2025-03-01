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
/// Arrow indicator item used with CCDS_Player.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Misc/CCDS Arrow Indicator")]
public class CCDS_ArrowIndicator : ACCDS_Component {

    /// <summary>
    /// Root transform. Minimap item will be child object of this transform.
    /// </summary>
    public Transform root;

    /// <summary>
    /// Target to indicate.
    /// </summary>
    public Vector3 target = Vector3.zero;

    /// <summary>
    /// Position offset.
    /// </summary>
    public Vector3 offset = new Vector3(0f, 1.5f, 5f);

    /// <summary>
    /// Extra position offset.
    /// </summary>
    [HideInInspector] public Vector3 extraOffset = Vector3.zero;

    private void Update() {

        //  Setting root if not selected.
        if (!root)
            root = transform.root;

        //  Return if root not selected.
        if (!root) {

            Debug.LogError("Root couldn't found for this " + transform.name + ", please select root of this arrow indicator item!");
            return;

        }

        //  Setting position of the arrow item.
        transform.position = root.position;

        //  If target is not vector3 zero, point the target. Otherwise set local rotation to Quaternion.identity.
        if (target != Vector3.zero) {

            Quaternion targetRotation = Quaternion.LookRotation(target - (transform.position - (Quaternion.LookRotation(transform.forward, Vector3.up) * (offset + extraOffset))));

            //  Setting rotation of the arrow item.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            transform.rotation = Quaternion.Euler(Mathf.Lerp(0f, 20f, Mathf.Abs(Quaternion.Angle(Quaternion.Euler(0f, transform.eulerAngles.y, 0f), transform.rotation)) / 180f), transform.eulerAngles.y, 0f);

        } else {

            //  Setting rotation of the arrow item.
            transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 5f);

        }

        transform.position += Quaternion.LookRotation(root.forward, Vector3.up) * (offset + extraOffset);

    }

    /// <summary>
    /// Sets the new target for this arrow indicator.
    /// </summary>
    /// <param name="newTarget"></param>
    public void SetTarget(Vector3 newTarget) {

        target = newTarget;

    }

    /// <summary>
    /// Cleans the target of this arrow indicator.
    /// </summary>
    public void CleanTarget() {

        target = Vector3.zero;

    }

}
