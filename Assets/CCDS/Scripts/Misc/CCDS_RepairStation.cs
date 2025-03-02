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

/// <summary>
/// Repair station to repair the player vehicle. Works with trigger collider.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Misc/CCDS Repair Station")]
public class CCDS_RepairStation : ACCDS_Component {

    /// <summary>
    /// UI canvas for displaying the repair station text.
    /// </summary>
	public Transform UI;
	public Transform TeleportPoint;
	[SerializeField] private int repair = 500;

    private void Update() {

	    if(BCG_EnterExitManager.Instance.activePlayer == null) 
		    return;
		    

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
	//	TeleportPoint = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
	//	TeleportPoint.SetParent(transform);
	//	TeleportPoint.SetLocalPositionAndRotation(Vector3.zero,Quaternion.Euler(Vector3.zero));
	//	TeleportPoint.name = "Teleport Point";		
	//}
	
    /// <summary>
    /// On trigger enter.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other) {

        //  Getting the player.
        CCDS_Player player = other.GetComponentInParent<CCDS_Player>();

        //  If trigger is not player, return.
        if (!player)
            return;

        //  Return if player is not damaged.
        if (player.damage <= 0)
            return;

        //  If player doesn't have damage component, return.
        if (!player.CarController.Damage)
            return;

	    CCDS_UI_Informer.Instance.OpenRepairPopup(() => Repair(player));

        ////  Repairing the player vehicle.
        //player.CarController.Damage.repaired = false;
        //player.CarController.Damage.repairNow = true;
        //player.damage = 0f;

        ////  Displaying info.
        //CCDS_UI_Informer.Instance.Info("Repaired!");

    }

    /// <summary>
    /// On trigger exit.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other) {

        //  Getting the player.
        CCDS_Player player = other.GetComponentInParent<CCDS_Player>();

        //  If trigger is not player, return.
        if (!player)
	        return;
            

        ////  If player doesn't have damage component, return.
        if (!player.CarController.Damage)
            return;
		
		CCDS_UI_Informer.Instance.CloseRepairPopup();

        //  Exisitng the trigger zone, stop repairing the vehicle.
        player.CarController.Damage.repaired = true;
        player.CarController.Damage.repairNow = false;

    }
    
	public void Repair(CCDS_Player player)
	{			
		if(CCDS.GetMoney() >= repair)
		{
			player.CarController.Damage.repaired = false;
			player.CarController.Damage.repairNow = true;
			player.damage = 0f;

			CCDS_UI_Informer.Instance.CloseRepairPopup();
			CCDS_UI_Informer.Instance.Info("Repaired!");
			
			//player.CarController.Damage.repaired = true;
			//player.CarController.Damage.repairNow = false;
		}
		else
		{
			CCDS_UI_Informer.Instance.CloseRepairPopup();
			CCDS_UI_Informer.Instance.Info("Not Enough Money");
		}
	
	}
			
	

}
