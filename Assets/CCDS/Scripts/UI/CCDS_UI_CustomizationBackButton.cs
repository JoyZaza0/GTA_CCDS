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
/// Back UI button used in the main menu customization panel. Used to prevent lefted items in the cart.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/CCDS UI Customization Back Button")]
public class CCDS_UI_CustomizationBackButton : ACCDS_Component {
	
	//public GameObject backButton;	

    /// <summary>
    /// Main menu panel.
    /// </summary>
	public GameObject mainMenuPanel;

    /// <summary>
    /// On click.
    /// </summary>
    public void OnClick() {

        //  Inform if player left items in the cart before going back.
        if (CCDS_MainMenuManager.Instance.itemsInCart.Count > 0) {

            CCDS_UI_Informer.Instance.Info("You've left items in your cart, please clear or purchase the items!");
            return;

        }

        //  Get back to the main menu.
        CCDS_UI_MainMenuManager.Instance.OpenPanel(mainMenuPanel);

        //  And set panel title text.
	    CCDS_UI_MainMenuManager.Instance.SetPanelTitleText("Main Menu");
        
	    transform.parent.gameObject.SetActive(false);

    }

}
