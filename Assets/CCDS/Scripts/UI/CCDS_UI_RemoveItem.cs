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
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// UI remove button to remove the selected item.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/CCDS UI Remove Item")]
public class CCDS_UI_RemoveItem : ACCDS_Component, IPointerClickHandler {

    /// <summary>
    /// Item type to be removed.
    /// </summary>
    public CCDS_CartItem item;

    /// <summary>
    /// Button.
    /// </summary>
    private Button button;

    private void Awake() {

        //  Getting the button.
        button = GetComponent<Button>();

    }

    /// <summary>
    /// On click.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData) {

        //  Getting the button.
        if (!button)
            button = GetComponent<Button>();

        //  Return if no button found yet.
        if (!button)
            return;

        //  Return if the button is not interactable or active.
        if (!button.interactable || !button.gameObject.activeSelf)
            return;

        //  Removing the item from the cart.
        CCDS_MainMenuManager.Instance.RemoveItemFromCart(item);

    }

}
