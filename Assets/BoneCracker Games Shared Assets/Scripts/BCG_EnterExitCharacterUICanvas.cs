//----------------------------------------------
//            BCG Shared Assets
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Enter Exit for BCG Vehicles. UI canvas of the FPS / TPS character controller.
/// </summary>
[AddComponentMenu("BoneCracker Games/BCG Shared Assets Pro/UI/BCG Enter Exit UI Canvas")]
public class BCG_EnterExitCharacterUICanvas : MonoBehaviour {

    /// <summary>
    /// Which canvas group will be displayed now?
    /// </summary>
    public enum DisplayType { OnFoot, InVehicle }

    /// <summary>
    /// Which canvas group will be displayed now?
    /// </summary>
    public DisplayType displayType = DisplayType.OnFoot;

    /// <summary>
    /// Event when this UI canvas spawned.
    /// </summary>
    /// <param name="canvas"></param>
    public delegate void onBCGPlayerCanvasSpawned(BCG_EnterExitCharacterUICanvas canvas);
    public static event onBCGPlayerCanvasSpawned OnBCGPlayerCanvasSpawned;

    /// <summary>
    /// In vehicle canvas group.
    /// </summary>
    public GameObject UisInVehicle;

    /// <summary>
    /// On foot canvas group.
    /// </summary>
	public GameObject UisOnFoot;
	public CanvasGroup playerCanvasGroup;
	private bool hidePlayerCanvasGroup;

    private void OnEnable() {

        if (OnBCGPlayerCanvasSpawned != null)
            OnBCGPlayerCanvasSpawned(this);

    }

    private void Start() {

        if (BCG_EnterExitSettings.Instance.mobileController)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);

    }

    private void Update() {

        switch (displayType) {

            case DisplayType.InVehicle:

	            if (!UisInVehicle.activeInHierarchy)
	            {
		            UisInVehicle.SetActive(true);            	
	            }
	            
	            if(playerCanvasGroup.alpha > 0)
	            {
		            playerCanvasGroup.alpha = 0f;
		            playerCanvasGroup.blocksRaycasts = false;
	            }
	          
	            //if (UisOnFoot.activeInHierarchy)
	            //{
		        //    UisOnFoot.SetActive(false);
	            //}
		     
                break;

            case DisplayType.OnFoot:

	            if (UisInVehicle.activeInHierarchy)
	            {
		            UisInVehicle.SetActive(false);	            	
	            }
	            
	            if(playerCanvasGroup.alpha <= 0 && hidePlayerCanvasGroup == false)
	            {
		            playerCanvasGroup.alpha = 1;
		            playerCanvasGroup.blocksRaycasts = true;
	            }
	            
	            //if (!UisOnFoot.activeInHierarchy)
	            //{   	
		        //    UisOnFoot.SetActive(true);	            	
	            //}

                break;

        }

    }
    
	public void HidePlayerUi(bool hide)
	{
		hidePlayerCanvasGroup = hide;
	}

}
