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
using TMPro;

/// <summary>
/// UI cart item displayed in the cart panel.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/CCDS UI Cart Item")]
public class CCDS_UI_CartItem : ACCDS_Component {

    /// <summary>
    /// Item type and properties.
    /// </summary>
    public CCDS_CartItem item;

    /// <summary>
    /// Item name text.
    /// </summary>
    public TextMeshProUGUI itemNameText;

    /// <summary>
    /// Item price text.
    /// </summary>
    public TextMeshProUGUI priceText;

    /// <summary>
    /// Sets the new item.
    /// </summary>
    /// <param name="newItem"></param>
    public void SetItem(CCDS_CartItem newItem) {

        item = newItem;

        itemNameText.text = item.itemName;
        priceText.text = "$ " + item.price.ToString("F0");

    }

}
