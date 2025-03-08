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

/// <summary>
/// Main menu manager along with UI.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/Managers/CCDS Main Menu Manager")]
public class CCDS_MainMenuManager : ACCDS_Manager {

    private static CCDS_MainMenuManager instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static CCDS_MainMenuManager Instance {

        get {

            if (instance == null)
                instance = FindFirstObjectByType<CCDS_MainMenuManager>();

            return instance;

        }

    }

	private List<BCG_EnterExitVehicle> allPlayerVehicles = new List<BCG_EnterExitVehicle>();

    [Header("Spawn Point")]
    public CCDS_SpawnPoint spawnPoint;

    [Header("Current Vehicle")]
	public BCG_EnterExitVehicle currentVehicle;

    /// <summary>
    /// Enables headlights of the vehicle.
    /// </summary>
    public bool enableHeadlights = true;

    /// <summary>
    /// Current selected vehicle index.
    /// </summary>
    public int selectedVehicleIndex = 0;

    /// <summary>
    /// All purchasable items in the cart.
    /// </summary>
    public List<CCDS_CartItem> itemsInCart = new List<CCDS_CartItem>();

    private void Awake() {

        //  Resuming the game.
        CCDS.ResumeGame();

        //  Getting all player vehicles.
	    allPlayerVehicles = new List<BCG_EnterExitVehicle>();

        //  Getting selected vehicle index.
        selectedVehicleIndex = GetVehicleIndex();

    }

    private void Start() {

        //  Spawning all selectable player vehicles once and disabling all of them.
        SpawnAllPlayerVehicles();

        //  After that, only enabling the selected vehicle. Disabling all other vehicles except the selected vehicle.
        EnableVehicle();

    }

    /// <summary>
    /// Spawns all selectable player vehicles once.
    /// </summary>
    private void SpawnAllPlayerVehicles() {

        //  If spawn point couldn't found, inform and create.
        if (spawnPoint == null) {

            Debug.LogError("Spawn point couldn't found, creating it at vector3 zero. Be sure to create a spawn point and assign it in the CCDS_MainMenuManager!");

            spawnPoint = new GameObject("CCDS_SpawnPoint").AddComponent<CCDS_SpawnPoint>();
            spawnPoint.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            spawnPoint.transform.position += Vector3.up * 1.5f;
            spawnPoint.transform.position += Vector3.forward * 15f;

        }

        //  Spawning all selectable player vehicles, disabling them, and enabling or disabling the headlights as well.
        for (int i = 0; i < CCDS_PlayerVehicles.Instance.playerVehicles.Length; i++) {
	        
	        //RCCP_CarController spawned = RCCP.SpawnRCC(CCDS_PlayerVehicles.Instance.playerVehicles[i].vehicle, spawnPoint.transform.position, spawnPoint.transform.rotation, true, false, false);
	        BCG_EnterExitVehicle spawned = Instantiate(CCDS_PlayerVehicles.Instance.playerVehicles[i].vehicle, spawnPoint.transform.position, spawnPoint.transform.rotation) as BCG_EnterExitVehicle;
            allPlayerVehicles.Add(spawned);
			
	        spawned.CarController.SetCanControl(false);
	        spawned.CarController.SetEngine(false);
	        
	        if (spawned.CarController.Lights)
		        spawned.CarController.Lights.lowBeamHeadlights = true;
            else
                Debug.LogWarning("Lights couldn't found on this player vehicle named " + spawned.transform.name + ", please add lights component through the RCCP_CarController!");

	        if (spawned.CarController.Customizer) {

		        if (!PlayerPrefs.HasKey(spawned.CarController.Customizer.saveFileName)) {

			        spawned.CarController.Customizer.Save();

                } else {

	                spawned.CarController.Customizer.Load();
	                spawned.CarController.Customizer.Initialize();

                }

            } else {

                Debug.LogWarning("Customizer couldn't found on this player vehicle named " + spawned.transform.name + ", please add customizer component through the RCCP_CarController!");

            }

            spawned.gameObject.SetActive(false);

        }

    }

    /// <summary>
    /// Gets the latest selected vehicle as int index.
    /// </summary>
    /// <returns></returns>
    public int GetVehicleIndex() {

        return CCDS.GetVehicle();

    }

    /// <summary>
    /// Saves the current vehicle as selected player vehicle.
    /// </summary>
    public void SelectVehicle() {

        CCDS.SetVehicle(selectedVehicleIndex);

    }

    /// <summary>
    /// Enables next vehicle and disables all other ones.
    /// </summary>
    public void NextVehicle() {

        selectedVehicleIndex++;

        if (selectedVehicleIndex >= CCDS_PlayerVehicles.Instance.playerVehicles.Length)
            selectedVehicleIndex = 0;

        EnableVehicle();

    }

    /// <summary>
    /// Enables previous vehicle and disables all other ones.
    /// </summary>
    public void PreviousVehicle() {

        selectedVehicleIndex--;

        if (selectedVehicleIndex < 0)
            selectedVehicleIndex = CCDS_PlayerVehicles.Instance.playerVehicles.Length - 1;

        EnableVehicle();

    }

    /// <summary>
    /// Enables the current vehicle and disables all other ones.
    /// </summary>
    public void EnableVehicle() {

        //  Disabling all vehicles.
        for (int i = 0; i < allPlayerVehicles.Count; i++) {

            if (allPlayerVehicles[i] != null)
                allPlayerVehicles[i].gameObject.SetActive(false);

        }

        //  If selected vehicle is not null, save it as a player vehicle and register it.
        if (allPlayerVehicles[selectedVehicleIndex] != null) {

            currentVehicle = allPlayerVehicles[selectedVehicleIndex];
            allPlayerVehicles[selectedVehicleIndex].gameObject.SetActive(true);
	        RCCP.RegisterPlayerVehicle(allPlayerVehicles[selectedVehicleIndex].CarController, false, false);

        }

        //  If price of the vehicle is below 0, purchase it automatically.
        if (CCDS_PlayerVehicles.Instance.playerVehicles[selectedVehicleIndex].price <= 0)
            CCDS.UnlockVehicle(selectedVehicleIndex);

        CCDS_Events.Event_OnVehicleChangedMM();

    }

    /// <summary>
    /// Purchases the current vehicle.
    /// </summary>
    public void PurchaseVehicle() {

        //  Getting current money.
        int currentMoney = CCDS.GetMoney();

        //  If current money is enough to purchase the vehicle, purchase it.
        if (currentMoney >= CCDS_PlayerVehicles.Instance.playerVehicles[selectedVehicleIndex].price) {

            //  Purchasing the vehicle and changing the amount of the money.
            CCDS.UnlockVehicle(selectedVehicleIndex);
            CCDS.ChangeMoney(-CCDS_PlayerVehicles.Instance.playerVehicles[selectedVehicleIndex].price);

            //  Displaying text.
            CCDS_UI_Informer.Instance.Info("New vehicle has been purchased!");

            //  And reenabling the vehicle.
            EnableVehicle();

        } else {

            //  If current money is not enough to purchase the vehicle, display text.
            int difference = CCDS_PlayerVehicles.Instance.playerVehicles[selectedVehicleIndex].price - currentMoney;
            CCDS_UI_Informer.Instance.Info(difference + " more money needed to purchase this vehicle!");

        }

    }

    /// <summary>
    /// Selects the target scene.
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void SelectScene(int sceneIndex) {

        CCDS.SetScene(sceneIndex);

    }

    /// <summary>
    /// Starts the target scene.
    /// </summary>
    public void StartScene() {

        SaveCustomization();

        CCDS.StartGameplayScene();

    }

    /// <summary>
    /// Checks upgradable item purchased or not. If not purchased, add to the cart, remove otherwise.
    /// </summary>
    /// <param name="newItem"></param>
    public void CheckUpgradePurchased(CCDS_CartItem newItem) {

        if (!currentVehicle.CarController.Customizer) {

            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");
            return;

        }

        if (PlayerPrefs.HasKey(currentVehicle.CarController.Customizer.saveFileName + newItem.saveKey))
            RemoveItemFromCart(newItem);
        else
            AddItemToCart(newItem);

        StartCoroutine(CheckCurrentVehicle());

    }

    private IEnumerator CheckCurrentVehicle() {

        yield return new WaitForFixedUpdate();
        CCDS_UI_MainMenuManager.Instance.CheckCurrentVehicle();

    }

    /// <summary>
    /// Checks purchasable item. If not purchased, add to the cart, remove otherwise.
    /// </summary>
    /// <param name="newItem"></param>
    public void CheckItemPurchased(CCDS_CartItem newItem) {

        if (PlayerPrefs.HasKey(newItem.saveKey))
            RemoveItemFromCart(newItem);
        else
            AddItemToCart(newItem);

    }

    /// <summary>
    /// Adds a new item to the cart. Cart can't have items with same type.
    /// </summary>
    /// <param name="newItem"></param>
    public void AddItemToCart(CCDS_CartItem newItem) {

        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null) {

                if (Equals(itemsInCart[i].itemType, newItem.itemType))
                    itemsInCart.RemoveAt(i);

            }

        }

        if (!itemsInCart.Contains(newItem))
            itemsInCart.Add(newItem);

    }

    /// <summary>
    /// Removes an item from the cart. Cart can't have items with same type.
    /// </summary>
    /// <param name="newItem"></param>
    public void RemoveItemFromCart(CCDS_CartItem newItem) {

        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null) {

                if (Equals(itemsInCart[i].itemType, newItem.itemType))
                    itemsInCart.RemoveAt(i);

            }

        }

        if (itemsInCart.Contains(newItem))
            itemsInCart.Remove(newItem);

    }

    /// <summary>
    /// Clears the cart and restores the player vehicle back to the last loadout.
    /// </summary>
    public void ClearCart() {

        itemsInCart.Clear();

        LoadCustomization();
        ApplyCustomization();

        //  Updating all purchasable items in the scene.
        CCDS_UI_PurchaseItem[] uI_PurchaseItems = FindObjectsByType<CCDS_UI_PurchaseItem>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < uI_PurchaseItems.Length; i++)
            uI_PurchaseItems[i].OnEnable();

        //  Updating all upgradable items in the scene.
        CCDS_UI_PurchaseUpgrade[] uI_UpgradeItems = FindObjectsByType<CCDS_UI_PurchaseUpgrade>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < uI_UpgradeItems.Length; i++) {

            uI_UpgradeItems[i].OnEnable();

        }

    }

    /// <summary>
    /// Purchases all items in the cart and saves the player vehicle loadout..
    /// </summary>
    public void PurchaseCart() {

        //  Calculating the total price.
        int totalPrice = 0;

        //  Calculating the total price.
        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null)
                totalPrice += itemsInCart[i].price;

        }

        //  If player money is enough to purchase the cart, proceed. Otherwise return.
        if (CCDS.GetMoney() < totalPrice) {

            CCDS_UI_Informer.Instance.Info("Not enough money to purchase the cart!");
            return;

        }

        //  Consuming the money.
        CCDS.ChangeMoney(-totalPrice);

        //  Saving all purchased items.
        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null)
                PlayerPrefs.SetInt(itemsInCart[i].saveKey, 1);

        }

        //  Saving the loadout.
        SaveCustomization();

        //  Clearing the cart.
        itemsInCart.Clear();

        //  Updating all purchasable items in the scene.
        CCDS_UI_PurchaseItem[] uI_PurchaseItems = FindObjectsByType<CCDS_UI_PurchaseItem>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < uI_PurchaseItems.Length; i++)
            uI_PurchaseItems[i].CheckPurchase();

        //  Updating all upgradable items in the scene.
        CCDS_UI_PurchaseUpgrade[] uI_UpgradeItems = FindObjectsByType<CCDS_UI_PurchaseUpgrade>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < uI_UpgradeItems.Length; i++) {

            uI_UpgradeItems[i].Refresh();
            uI_UpgradeItems[i].CheckPurchase();

        }

    }

    /// <summary>
    /// Saves the current loadout.
    /// </summary>
    public void SaveCustomization() {

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Save();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");

        CCDS_UI_Informer.Instance.Info("Customization saved!");

    }

    /// <summary>
    /// Loads the latest loadout.
    /// </summary>
    public void LoadCustomization() {

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Load();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");

    }

    /// <summary>
    /// Applies the loaded loadout.
    /// </summary>
    public void ApplyCustomization() {

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Initialize();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");

    }

    /// <summary>
    /// Adding money for testing purposes.
    /// </summary>
    public void Testing_AddMoney() {

        CCDS.ChangeMoney(10000);

    }

    /// <summary>
    /// Unlocking all vehicles for testing purposes.
    /// </summary>
    public void Testing_UnlockAllCars() {

        CCDS.UnlockAllVehicles();

    }

    /// <summary>
    /// Deletes the save data and restarts the game for testing purposes.
    /// </summary>
    public void Testing_ResetSave() {

        CCDS.ResetGame();

    }

    private void Reset() {

        if (spawnPoint == null) {

            spawnPoint = new GameObject("CCDS_SpawnPoint").AddComponent<CCDS_SpawnPoint>();
            spawnPoint.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            spawnPoint.transform.position += Vector3.up * 1.5f;
            spawnPoint.transform.position += Vector3.forward * 15f;

        }

    }

}
