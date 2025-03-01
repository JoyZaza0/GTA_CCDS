using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CCDS_SaveGameManager {

    public static CCDS_SaveData saveData = new CCDS_SaveData();

    public static void Save() {

        if (saveData == null)
            saveData = new CCDS_SaveData();

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(Application.persistentDataPath + "/CCDS_SaveData.json", json);

    }

    public static void Load() {

        if (saveData == null)
            saveData = new CCDS_SaveData();

        if (!File.Exists(Application.persistentDataPath + "/CCDS_SaveData.json"))
            return;

        string json = File.ReadAllText(Application.persistentDataPath + "/CCDS_SaveData.json");

        if (!string.IsNullOrEmpty(json))
            saveData = (CCDS_SaveData)JsonUtility.FromJson(json, typeof(CCDS_SaveData));

    }

    public static void Delete() {

        saveData = new CCDS_SaveData();

        if (!File.Exists(Application.persistentDataPath + "/CCDS_SaveData.json"))
            return;

        File.Delete(Application.persistentDataPath + "/CCDS_SaveData.json");

    }

}
