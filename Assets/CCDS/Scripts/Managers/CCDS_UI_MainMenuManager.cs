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
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Mainmenu UI manager.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/CCDS UI Main Menu Manager")]
public class CCDS_UI_MainMenuManager : ACCDS_Manager {

    private static CCDS_UI_MainMenuManager instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static CCDS_UI_MainMenuManager Instance {

        get {

            if (instance == null)
                instance = FindFirstObjectByType<CCDS_UI_MainMenuManager>();

            return instance;

        }

    }

    [Header("UI Panels")]
    public GameObject[] panels;
    public GameObject welcomePanel;

    [Header("UI Texts")]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI panelTitleText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI vehiclePriceText;

    [Header("UI Buttons")]
    public GameObject selectVehicleButton;
    public GameObject purchaseVehicleButton;
    public GameObject purchaseCartButton;

    [Header("UI Sliders For Vehicle Stats")]
    public Image vehicleStats_Engine;
    public Image vehicleStats_Handling;
    public Image vehicleStats_Speed;

    [Header("UI InputField For Player Name")]
    public TMP_InputField playerNameInputField;

    [Space()] public Image vehicleStats_Engine_Upgraded;
    public Image vehicleStats_Handling_Upgraded;
    public Image vehicleStats_Speed_Upgraded;

    public GameObject cartPanel;
    public GameObject cartItemsContent;
    public CCDS_UI_CartItem cartItemReference;
    private CCDS_UI_PurchaseItem[] itemPurchaseButtons;

    private void Awake() {

        //  Enable the welcome panel if player doesn't have any save data yet.
        if (CCDS.IsFirstGameplay()) {

            welcomePanel.SetActive(true);
            playerNameInputField.SetTextWithoutNotify(CCDS_Settings.Instance.defaultPlayerName);

        } else {

            welcomePanel.SetActive(false);
            playerNameInputField.SetTextWithoutNotify("");

        }

        //  Setting the player nickname.
        SetPlayerNameText(CCDS.GetPlayerName());

        //  Displaying the money text.
        CCDS_OnMoneyChanged();

    }

    private void OnEnable() {

        //  Displaying the money text when it changes.
        CCDS_Events.OnMoneyChanged += CCDS_OnMoneyChanged;

        //  When player vehicle changed in the main menu.
        CCDS_Events.OnVehicleChangedMM += CCDS_Events_OnVehicleChangedMM;

    }

    private void Update() {

        //  Return if main manager couldn't found.
        if (!CCDS_MainMenuManager.Instance)
            return;

        //  Finding the current player vehicle.
	    BCG_EnterExitVehicle currentVehicle = CCDS_MainMenuManager.Instance.currentVehicle;

        //  If current vehicle is not null, display stats of the vehicle.
        if (currentVehicle) {

            if (cartPanel.activeInHierarchy)
                UpdateCartItemsList();

        }

    }

    /// <summary>
    /// When player vehicle changed in the main menu.
    /// </summary>
    private void CCDS_Events_OnVehicleChangedMM() {

        CheckCurrentVehicle();

    }

    /// <summary>
    /// Displaying the money text.
    /// </summary>
    private void CCDS_OnMoneyChanged() {

        //  Displaying the money text.
        moneyText.text = "$ " + CCDS.GetMoney().ToString();

    }

    public void UpdateCartItemsList() {

        //  Getting all items in the cart.
        CCDS_UI_CartItem[] items = cartItemsContent.GetComponentsInChildren<CCDS_UI_CartItem>(true);

        //  Destroying all items before instantiating them.
        foreach (CCDS_UI_CartItem item in items) {

            if (!Equals(item.gameObject, cartItemReference.gameObject))
                Destroy(item.gameObject);
            else if (cartItemReference.gameObject.activeSelf)
                cartItemReference.gameObject.SetActive(false);

        }

        //  Instantiating all items in the cart and setting them.
        for (int i = 0; i < CCDS_MainMenuManager.Instance.itemsInCart.Count; i++) {

            CCDS_UI_CartItem cartItem = Instantiate(cartItemReference.gameObject, cartItemsContent.transform).GetComponent<CCDS_UI_CartItem>();
            cartItem.SetItem(CCDS_MainMenuManager.Instance.itemsInCart[i]);
            cartItem.gameObject.SetActive(true);

        }

        //  Calculating the total price.
        int totalPrice = 0;

        //  Calculating the total price.
        for (int i = 0; i < CCDS_MainMenuManager.Instance.itemsInCart.Count; i++) {

            if (CCDS_MainMenuManager.Instance.itemsInCart[i] != null)
                totalPrice += CCDS_MainMenuManager.Instance.itemsInCart[i].price;

        }

        //  Enable the purchase button if total price is above 0. Disable otherwise.
        if (totalPrice > 0)
            purchaseCartButton.SetActive(true);
        else
            purchaseCartButton.SetActive(false);

        //  Set price text if purchase button is enabled.
        if (purchaseCartButton.activeSelf)
            purchaseCartButton.GetComponentInChildren<TextMeshProUGUI>().text = "Purchase For\n$ " + totalPrice.ToString("F0");

    }

    /// <summary>
    /// Opens the target UI panel and disables all other ones.
    /// </summary>
    /// <param name="activePanel"></param>
    public void OpenPanel(GameObject activePanel) {

        //  Disabling all panels.
        for (int i = 0; i < panels.Length; i++) {

            if (panels[i] != null)
                panels[i].SetActive(false);

        }

        //  Enabling the active panel.
        if (activePanel != null)
            activePanel.SetActive(true);

    }

    /// <summary>
    /// Sets player name text.
    /// </summary>
    /// <param name="title"></param>
    public void SetPlayerNameText(string title) {

        playerNameText.text = title;

    }

    /// <summary>
    /// Sets title text.
    /// </summary>
    /// <param name="title"></param>
    public void SetPanelTitleText(string title) {

        panelTitleText.text = title;

    }

    /// <summary>
    /// Sets the player name with inputfield.
    /// </summary>
    public void SetPlayerName(TMP_InputField newInputField) {

        playerNameInputField.SetTextWithoutNotify(newInputField.text);
        CCDS.SetPlayerName(newInputField.text);
        welcomePanel.SetActive(false);
        SetPlayerNameText(CCDS.GetPlayerName());
        CCDS_UI_Informer.Instance.Info("Welcome " + newInputField.text + "!");

    }

    /// <summary>
    /// Saves the current vehicle as selected player vehicle.
    /// </summary>
    public void SelectVehicle() {

        //  Return if main manager couldn't found.
        if (!CCDS_MainMenuManager.Instance)
            return;

        //  Select the vehicle.
        CCDS_MainMenuManager.Instance.SelectVehicle();

    }

    /// <summary>
    /// Enables next vehicle and disables all other ones.
    /// </summary>
    public void NextVehicle() {

        //  Return if main manager couldn't found.
        if (!CCDS_MainMenuManager.Instance)
            return;

        //  Next vehicle.
        CCDS_MainMenuManager.Instance.NextVehicle();

    }

    /// <summary>
    /// Enables previous vehicle and disables all other ones.
    /// </summary>
    public void PreviousVehicle() {

        //  Return if main manager couldn't found.
        if (!CCDS_MainMenuManager.Instance)
            return;

        //  Previous vehicle.
        CCDS_MainMenuManager.Instance.PreviousVehicle();

    }

    /// <summary>
    /// Enables the current vehicle and disables all other ones.
    /// </summary>
    public void EnableVehicle() {

        //  Return if main manager couldn't found.
        if (!CCDS_MainMenuManager.Instance)
            return;

        //  Enabling the vehicle.
        CCDS_MainMenuManager.Instance.EnableVehicle();

    }

    /// <summary>
    /// Purchases the current vehicle.
    /// </summary>
    public void PurchaseVehicle() {

        //  Return if main manager couldn't found.
        if (!CCDS_MainMenuManager.Instance)
            return;

        //  Purchasing the vehicle.
        CCDS_MainMenuManager.Instance.PurchaseVehicle();

    }

    public void CheckCurrentVehicle() {

        //  Return if main manager couldn't found.
        if (!CCDS_MainMenuManager.Instance)
            return;

        //  Finding the current player vehicle.
	    BCG_EnterExitVehicle currentVehicle = CCDS_MainMenuManager.Instance.currentVehicle;

        //  If current vehicle is not null, display stats of the vehicle.
        if (currentVehicle) {

            //  Fill amount of the engine torque.
	        if (vehicleStats_Engine && currentVehicle.CarController.Engine)
		        vehicleStats_Engine.fillAmount = Mathf.InverseLerp(-400f, 800f, currentVehicle.CarController.Engine.maximumTorqueAsNM);

            //  Fill amount of the stability strength.
	        if (vehicleStats_Handling && currentVehicle.CarController.Stability)
		        vehicleStats_Handling.fillAmount = Mathf.InverseLerp(0f, 1f, (currentVehicle.CarController.Stability.tractionHelperStrength + currentVehicle.CarController.Stability.steerHelperStrength) * .5f);

            //  Fill amount of the speed.
	        if (vehicleStats_Speed && currentVehicle.CarController.Differential)
		        vehicleStats_Speed.fillAmount = 1f - Mathf.InverseLerp(3.1f, 5.31f, currentVehicle.CarController.Differential.finalDriveRatio);

            //  Fill amount of the upgraded engine torque.
	        if (vehicleStats_Engine_Upgraded && currentVehicle.CarController.Customizer && currentVehicle.CarController.Customizer.UpgradeManager && currentVehicle.CarController.Customizer.UpgradeManager.Engine)
		        vehicleStats_Engine_Upgraded.fillAmount = Mathf.InverseLerp(-400f, 800f, currentVehicle.CarController.Customizer.UpgradeManager.Engine.defEngine * currentVehicle.CarController.Customizer.UpgradeManager.Engine.efficiency);
            else if (vehicleStats_Engine_Upgraded)
                vehicleStats_Engine_Upgraded.fillAmount = 0f;

            //  Fill amount of the upgraded handling strength.
	        if (vehicleStats_Handling_Upgraded && currentVehicle.CarController.Customizer && currentVehicle.CarController.Customizer.UpgradeManager && currentVehicle.CarController.Customizer.UpgradeManager.Handling)
		        vehicleStats_Handling_Upgraded.fillAmount = Mathf.InverseLerp(0f, 1f, currentVehicle.CarController.Customizer.UpgradeManager.Handling.defHandling * currentVehicle.CarController.Customizer.UpgradeManager.Handling.efficiency);
            else if (vehicleStats_Handling_Upgraded)
                vehicleStats_Handling_Upgraded.fillAmount = 0f;

            //Fill amount of the upgraded speed.
	        if (vehicleStats_Speed_Upgraded && currentVehicle.CarController.Customizer && currentVehicle.CarController.Customizer.UpgradeManager && currentVehicle.CarController.Customizer.UpgradeManager.Speed)
		        vehicleStats_Speed_Upgraded.fillAmount = 1f - Mathf.InverseLerp(3.1f, 5.31f, Mathf.Lerp(currentVehicle.CarController.Customizer.UpgradeManager.Speed.defRatio, currentVehicle.CarController.Customizer.UpgradeManager.Speed.defRatio * .6f, currentVehicle.CarController.Customizer.UpgradeManager.Speed.efficiency - 1f));
            else if (vehicleStats_Speed_Upgraded)
                vehicleStats_Speed_Upgraded.fillAmount = 0f;

            //  Enabling or disabling select and purchase buttons.
            selectVehicleButton.SetActive(CCDS.IsOwnedVehicle(CCDS_MainMenuManager.Instance.selectedVehicleIndex));
            purchaseVehicleButton.SetActive(!selectVehicleButton.activeSelf);

            //  If purchase button is active, set text of the price.
            if (purchaseVehicleButton.activeSelf)
                vehiclePriceText.text = "$ " + CCDS_PlayerVehicles.Instance.playerVehicles[CCDS_MainMenuManager.Instance.selectedVehicleIndex].price.ToString();
            else
                vehiclePriceText.text = "";

        }

    }

    /// <summary>
    /// Selects the target scene.
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void SelectScene(int sceneIndex) {

        //  Return if main manager couldn't found.
        if (!CCDS_MainMenuManager.Instance)
            return;

        //  Selecting the scene.
        CCDS_MainMenuManager.Instance.SelectScene(sceneIndex);

    }

    /// <summary>
    /// Starts the target scene.
    /// </summary>
    public void StartScene() {

        //  Return if main manager couldn't found.
        if (!CCDS_MainMenuManager.Instance)
            return;

        //  Starting the scene.
        CCDS_MainMenuManager.Instance.StartScene();

    }

    /// <summary>
    /// Purchases all items in the cart.
    /// </summary>
    public void PurchaseCart() {

        CCDS_MainMenuManager.Instance.PurchaseCart();
        CheckCurrentVehicle();

    }

    /// <summary>
    /// Clears the cart.
    /// </summary>
    public void ClearCart() {

        CCDS_MainMenuManager.Instance.ClearCart();
        CheckCurrentVehicle();

    }

    /// <summary>
    /// Adds the money for testing purposes.
    /// </summary>
    public void Testing_AddMoney() {

        CCDS_MainMenuManager.Instance.Testing_AddMoney();

    }

    /// <summary>
    /// Unlocks all vehicles for testing purposes.
    /// </summary>
    public void Testing_UnlockAllCars() {

        CCDS_MainMenuManager.Instance.Testing_UnlockAllCars();

    }

    /// <summary>
    /// Resets save data and reloads the scene for testing purposes.
    /// </summary>
    public void Testing_ResetSave() {

        CCDS_MainMenuManager.Instance.Testing_ResetSave();

    }

    private void OnDisable() {

        //  Displaying the money text when it changes.
        CCDS_Events.OnMoneyChanged -= CCDS_OnMoneyChanged;

        //  When player vehicle changed in the main menu.
        CCDS_Events.OnVehicleChangedMM -= CCDS_Events_OnVehicleChangedMM;

    }

}
