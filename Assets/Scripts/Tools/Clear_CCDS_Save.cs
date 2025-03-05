using UnityEngine;

public class Clear_CCDS_Save : MonoBehaviour
{
	public bool ClearPlayerPrefs;
	public void Clear()
	{
		CCDS_SaveGameManager.Delete();
		
		if(ClearPlayerPrefs)
			PlayerPrefs.DeleteAll();
	}
}
