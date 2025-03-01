using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RCCP_Camera))]
[AddComponentMenu("BoneCracker Games/CCDS/Cameras/CCDS Camera Gameplay")]
public class CCDS_Camera : ACCDS_Component {

    public RCCP_Camera RCCPCamera {

        get {

            if (rccpCamera == null)
                rccpCamera = GetComponent<RCCP_Camera>();

            return rccpCamera;

        }

    }
    private RCCP_Camera rccpCamera;

    /// <summary>
    /// Arrow item for indicating the mission objectives.
    /// </summary>
    public CCDS_ArrowIndicator arrowIndicator;
    public CCDS_ArrowIndicator ArrowIndicator {

        get {

            if (arrowIndicator == null) {

                if (CCDS_Settings.Instance.arrowForPlayer != null) {

	                arrowIndicator = Instantiate(CCDS_Settings.Instance.arrowForPlayer.gameObject, GetComponentInChildren<Camera>(true).transform).GetComponent<CCDS_ArrowIndicator>();
	                arrowIndicator.root = GetComponentInChildren<Camera>(true).transform;

                }

            }

            if (arrowIndicator != null)
                return arrowIndicator;
            else
                return null;

        }

    }

    private void Update() {

        //  Operating the arrow indicator if on mission. 
        if (CCDS_Settings.Instance.showArrowIndicator)
            Indicator();

    }

    /// <summary>
    /// Operating the arrow indicator if player is on mission. 
    /// </summary>
    private void Indicator() {

        if (!CCDS_Settings.Instance.showArrowIndicator)
            return;

        if (!CCDS_SceneManager.Instance)
            return;

        CCDS_Player player = CCDS_SceneManager.Instance.GameplayManager.player;

        if (!player) {

            ArrowIndicator.gameObject.SetActive(false);
            return;

        }

        bool canControl = player.CarController.canControl;
        bool onMission = player.OnMission;

        //  Operating the arrow indicator if player is on mission. 
        if (ArrowIndicator != null) {

            //  Enabling the arrow indicator only if player is in control, on mission, and mission has a target mission objective. Disabling otherwise.
            if (canControl && onMission && SceneManager.GameplayManager.currentMission.currentTarget != Vector3.zero)
                ArrowIndicator.gameObject.SetActive(true);
            else
                ArrowIndicator.gameObject.SetActive(false);

            //  Position and rotation of the arrow indicator if its enabled.
            if (ArrowIndicator.gameObject.activeSelf) {

                ArrowIndicator.SetTarget(SceneManager.GameplayManager.currentMission.currentTarget);
                ArrowIndicator.extraOffset = new Vector3(0f, 0f, -Mathf.InverseLerp(40f, 60f, RCCPCamera.targetFieldOfView) * 1f);

            } else {

                ArrowIndicator.CleanTarget();

            }

        }

    }

}
