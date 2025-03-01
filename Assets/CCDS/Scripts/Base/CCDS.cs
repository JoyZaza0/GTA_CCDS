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
using UnityEngine.SceneManagement;

/// <summary>
/// General API class.
/// </summary>
public class CCDS {

    /// <summary>
    /// Default time scale.
    /// </summary>
    static float defaultTimeScale = -1f;

    /// <summary>
    /// First gameplay passed. Used to display welcome screen.
    /// </summary>
    public static void SetFirstGameplay() {

        CCDS_SaveGameManager.saveData.firstGameplay = false;

    }

    /// <summary>
    /// Sets the player name.
    /// </summary>
    /// <param name="playerName"></param>
    public static void SetPlayerName(string playerName) {

        Load();
        CCDS_SaveGameManager.saveData.playerName = playerName;
        CCDS_SaveGameManager.saveData.firstGameplay = false;
        Save();

    }

    /// <summary>
    /// Gets the player name.
    /// </summary>
    /// <returns></returns>
    public static string GetPlayerName() {

        Load();
        return CCDS_SaveGameManager.saveData.playerName;

    }
    

	public static Vector3 GetPlayerCharacterPosition()
	{
		Load();
		return CCDS_SaveGameManager.saveData.playerCharacterPosition;
	}
	
	public void SetPlayerCharacterPosition(Vector3 position)
	{
		Load();
		CCDS_SaveGameManager.saveData.playerCharacterPosition = position;
		Save();
	}

    /// <summary>
    /// Gets the money.
    /// </summary>
    /// <returns></returns>
    public static int GetMoney() {

        Load();
        return CCDS_SaveGameManager.saveData.playerMoney;

    }

    /// <summary>
    /// Changes the player money. It can be positive or negative.
    /// </summary>
    /// <param name="amount"></param>
    public static void ChangeMoney(int amount) {

        Load();
        CCDS_SaveGameManager.saveData.playerMoney += amount;
        CCDS_SaveGameManager.saveData.firstGameplay = false;
        Save();

        CCDS_Events.Event_OnMoneyChanged();

    }

    /// <summary>
    /// Gets the latest selected player vehicle.
    /// </summary>
    /// <returns></returns>
    public static int GetVehicle() {

        UnlockVehicle(CCDS_Settings.Instance.defaultSelectedVehicleIndex);
        Load();
        return CCDS_SaveGameManager.saveData.selectedVehicle;

    }

    /// <summary>
    /// Sets the selected vehicle as player vehicle.
    /// </summary>
    /// <param name="vehicleIndex"></param>
    public static void SetVehicle(int vehicleIndex) {

        Load();
        CCDS_SaveGameManager.saveData.selectedVehicle = vehicleIndex;
        Save();

    }

    /// <summary>
    ///  Unlocks the target vehicle.
    /// </summary>
    /// <param name="vehicleIndex"></param>
    public static void UnlockVehicle(int vehicleIndex) {

        Load();

        if (!CCDS_SaveGameManager.saveData.ownedVehicles.Contains(vehicleIndex))
            CCDS_SaveGameManager.saveData.ownedVehicles.Add(vehicleIndex);

        Save();

    }

    /// <summary>
    /// Deleting save key for the target vehicle.
    /// </summary>
    /// <param name="vehicleIndex"></param>
    public static void LockVehicle(int vehicleIndex) {

        Load();

        if (CCDS_SaveGameManager.saveData.ownedVehicles.Contains(vehicleIndex))
            CCDS_SaveGameManager.saveData.ownedVehicles.RemoveAt(vehicleIndex);

        Save();

    }

    /// <summary>
    /// Purchases and unlocks all vehicles.
    /// </summary>
    /// <param name="vehicleIndex"></param>
    public static void UnlockAllVehicles() {

        for (int i = 0; i < CCDS_PlayerVehicles.Instance.playerVehicles.Length; i++) {

            if (CCDS_PlayerVehicles.Instance.playerVehicles[i] != null)
                UnlockVehicle(i);

        }

    }

    /// <summary>
    /// Deleting save key for all vehicles.
    /// </summary>
    /// <param name="vehicleIndex"></param>
    public static void LockAllVehicles() {

        for (int i = 0; i < CCDS_PlayerVehicles.Instance.playerVehicles.Length; i++) {

            if (CCDS_PlayerVehicles.Instance.playerVehicles[i] != null)
                LockVehicle(i);

        }

    }

    /// <summary>
    /// Is this vehicle owned by the player?
    /// </summary>
    /// <param name="vehicleIndex"></param>
    /// <returns></returns>
    public static bool IsOwnedVehicle(int vehicleIndex) {

        Load();

        if (CCDS_SaveGameManager.saveData.ownedVehicles.Contains(vehicleIndex))
            return true;

        return false;

    }

    /// <summary>
    /// Sets the target scene to load.
    /// </summary>
    /// <param name="sceneIndex"></param>
    public static void SetScene(int sceneIndex) {

        Load();
        CCDS_SaveGameManager.saveData.selectedScene = sceneIndex;
        Save();

    }

    /// <summary>
    /// Gets the latest selected scene.
    /// </summary>
    /// <returns></returns>
    public static int GetScene() {

        Load();
        return CCDS_SaveGameManager.saveData.selectedScene;

    }

    /// <summary>
    /// Loads the latest selected scene.
    /// </summary>
    public static void StartGameplayScene() {

        SceneManager.LoadScene(GetScene());

    }

    /// <summary>
    /// Pause the game.
    /// </summary>
    public static void PauseGame() {

        if (defaultTimeScale == -1)
            defaultTimeScale = Time.timeScale;

        Time.timeScale = 0;
        AudioListener.pause = true;
        CCDS_Events.Event_OnPaused();

    }

    /// <summary>
    /// Resume the game.
    /// </summary>
    public static void ResumeGame() {

        if (defaultTimeScale == -1)
            defaultTimeScale = Time.timeScale;

        Time.timeScale = defaultTimeScale;

        AudioListener.pause = false;
        AudioListener.volume = GetAudioVolume();

        CCDS_Events.Event_OnResumed();

    }

    /// <summary>
    /// Restart the game.
    /// </summary>
    public static void RestartGame() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    /// <summary>
    /// Back to the main menu.
    /// </summary>
    public static void MainMenu() {

        SceneManager.LoadScene(0);
        CCDS_Events.Event_OnMainMenu();

    }

    /// <summary>
    /// Sets the volume of the audiolistener.
    /// </summary>
    /// <param name="volume"></param>
    public static void SetAudioVolume(float volume) {

        AudioListener.volume = volume;

        Load();
        CCDS_SaveGameManager.saveData.audioVolume = volume;
        Save();

    }

    /// <summary>
    /// Sets the music volume.
    /// </summary>
    /// <param name="volume"></param>
    public static void SetMusicVolume(float volume) {

        Load();
        CCDS_SaveGameManager.saveData.musicVolume = volume;
        Save();

    }

    /// <summary>
    /// Get volume of the audiolistener.
    /// </summary>
    /// <returns></returns>
    public static float GetAudioVolume() {

        Load();
        return CCDS_SaveGameManager.saveData.audioVolume;

    }

    /// <summary>
    /// Get volume of the music.
    /// </summary>
    /// <returns></returns>
    public static float GetMusicVolume() {

        Load();
        return CCDS_SaveGameManager.saveData.musicVolume;

    }

    /// <summary>
    /// Disables the traffic for a while.
    /// </summary>
    //public static void DisableTrafficForAWhile() {

    //    RTC_SceneManager sceneManager = RTC_SceneManager.Instance;

    //    if (!sceneManager) {

    //        Debug.LogError("RTC_SceneManager couldn't found in the scene to enable or disable the traffic.");
    //        return;

    //    }

    //    if (sceneManager.allVehicles != null) {

    //        for (int i = 0; i < sceneManager.allVehicles.Length; i++) {

    //            if (sceneManager.allVehicles[i] != null)
    //                sceneManager.allVehicles[i].gameObject.SetActive(false);

    //        }

    //    }

    //}

    /// <summary>
    /// Is this the first gameplay?
    /// </summary>
    /// <returns></returns>
    public static bool IsFirstGameplay() {

        Load();
        return CCDS_SaveGameManager.saveData.firstGameplay;

    }

    /// <summary>
    /// Saves the player save data.
    /// </summary>
    public static void Save() {

        CCDS_SaveGameManager.Save();

    }

    /// <summary>
    /// Loads the player save data.
    /// </summary>
    public static void Load() {

        CCDS_SaveGameManager.Load();

        if (CCDS_SaveGameManager.saveData.firstGameplay) {

            CCDS_SaveGameManager.saveData.playerName = CCDS_Settings.Instance.defaultPlayerName;
            CCDS_SaveGameManager.saveData.playerMoney = CCDS_Settings.Instance.defaultMoney;
            CCDS_SaveGameManager.saveData.selectedVehicle = CCDS_Settings.Instance.defaultSelectedVehicleIndex;
            CCDS_SaveGameManager.saveData.audioVolume = CCDS_Settings.Instance.defaultAudioVolume;
            CCDS_SaveGameManager.saveData.musicVolume = CCDS_Settings.Instance.defaultMusicVolume;

        }

    }

    /// <summary>
    /// Resets the game by deleting the save data and reloading the scene.
    /// </summary>
    public static void ResetGame() {

        PlayerPrefs.DeleteAll();
        CCDS_SaveGameManager.Delete();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

    }

}
