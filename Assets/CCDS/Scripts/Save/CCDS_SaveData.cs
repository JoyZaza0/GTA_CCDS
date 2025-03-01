using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CCDS_SaveData {

    public string playerName = "PlayerName";
    public int playerMoney = 0;
    public int selectedVehicle = 0;
    public int selectedScene = 1;

    public List<int> ownedVehicles = new List<int>();

    public float audioVolume = 1f;
    public float musicVolume = .65f;

	public bool firstGameplay = true;
	public Vector3 playerCharacterPosition = new Vector3();

    public CCDS_SaveData() { }

}
