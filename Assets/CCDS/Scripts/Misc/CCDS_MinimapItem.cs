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
/// Minimap item used with CCDS_MinimapManager.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Misc/CCDS Minimap Item")]
public class CCDS_MinimapItem : ACCDS_Component {

    /// <summary>
    /// Root transform. Minimap item will be child object of this transform.
    /// </summary>
    public Transform root;

    /// <summary>
    /// Position offset.
    /// </summary>
    public Vector3 offset = new Vector3(0f, 25f, 0f);

    /// <summary>
    /// Sets the root transform of the minimap item.
    /// </summary>
    /// <param name="newRoot"></param>
    public void SetRoot(Transform newRoot) {

        root = newRoot;

    }

    private void Update() {

        //  Setting root if not selected.
        if (!root)
            root = transform.root;

        //  Return if root not selected.
        if (!root) {

            Debug.LogError("Root couldn't found for this " + transform.name + ", please select root of this minimap item!");
            return;

        }

        //  Setting position and rotation of the minimap item.
        transform.position = root.position;
        transform.position += offset;
        transform.rotation = Quaternion.identity;

    }

}
