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
/// Cart item used in the main menu.
/// </summary>
[System.Serializable]
[AddComponentMenu("BoneCracker Games/CCDS/Misc/CCDS Cart Item")]
public class CCDS_CartItem {

    public enum CartItemType { Paint, Wheel, Spoiler, Decal, Neon, UpgradeEngine, UpgradeHandling, UpgradeSpeed }
    public CartItemType itemType;

    public string saveKey;
    public string itemName;
    public int price;

    public CCDS_CartItem(CartItemType itemType, string saveKey, string itemName, int price) {

        this.itemType = itemType;
        this.saveKey = saveKey;
        this.itemName = itemName;
        this.price = price;

    }

}
