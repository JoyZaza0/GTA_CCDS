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
using TMPro;

/// <summary>
/// Mission marker. It's basically a trigger collider. Once player triggers it, gameplay manager script will be used (CCDS_GameplayManager.Instance.EnteredMarker(this)).
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Missions/CCDS Marker")]
public class CCDS_Marker : ACCDS_Component {

	public CCDS_GameModes.Mode missionMode;
    public ACCDS_Mission connectedMission;
	public TextMeshProUGUI lable;
	public Transform teleportPoint;
    /// <summary>
    /// UI canvas for camera rotation.
    /// </summary>
    [Space()] public Transform UI;

    private void Update() {
		
	    if(BCG_EnterExitManager.Instance.activePlayer == null) 
		    return;
		    
	    //  Set rotation of the UI canvas.
	    if(BCG_EnterExitManager.Instance.activePlayer.inVehicle)
	    {
		    if (UI && RCCP_SceneManager.Instance.activePlayerCamera)
			    UI.rotation = RCCP_SceneManager.Instance.activePlayerCamera.transform.rotation;
	    }
	    else
	    {
		    if (UI && Camera.main)
			    UI.rotation = Camera.main.transform.rotation;
	    }
       
    }
    
	//[ContextMenu("Create Teleport")]
	//public void CreateTeleport()
	//{
	//	teleportPoint = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
	//	teleportPoint.SetParent(transform);
	//	teleportPoint.SetLocalPositionAndRotation(Vector3.zero,Quaternion.Euler(Vector3.zero));
	//	teleportPoint.name = "Teleport Point";		
	//}
	
    /// <summary>
    /// On trigger.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {

        //  Return if gameplay manager not found.
        if (!CCDS_GameplayManager.Instance) {

            Debug.LogError("CCDS_GameplayManager couldn't found, can't start the mission! Create CCDS_SceneManager and check the scene setup. Tools --> BCG --> CCDS --> Create --> Scene Managers --> Gameplay --> CCDS Scene Manager");
            return;

        }

        //  Finding the player vehicle.
        CCDS_Player player = CCDS_GameplayManager.Instance.player;

        //  Return if player not found.
        if (!player) {

            Debug.LogError("Couldn't found the player vehicle!");
            return;

        }

        CCDS_Player triggeredPlayer = other.GetComponentInParent<CCDS_Player>();

        if (!triggeredPlayer)
            return;

        //  If triggered vehicle and local player vehicle is the same, load the main menu.
        if (!Equals(player.gameObject, triggeredPlayer.gameObject))
            return;

        //  Calling ''EnteredMarker'' on the gameplay manager to initialize and start the mission.
	    //CCDS_GameplayManager.Instance.EnteredMarker(this);
	    string info = connectedMission.misssionStartInfo;
	    CCDS_UI_Informer.Instance.OpenMissionPoup(info,() => 
	    {
	    	CCDS_GameplayManager.Instance.EnteredMarker(this);
		    CCDS_UI_Informer.Instance.CloseMissionPopup();
	    });
    }
    
	private void OnTriggerExit(Collider other)
	{
		CCDS_UI_Informer.Instance.CloseMissionPopup();
	}

}
