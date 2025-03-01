using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class MissionVictoryPopupUI : MonoBehaviour
{
	[SerializeField] private Button nextButton;
	[SerializeField] private TextMeshProUGUI victoryInfo;
	
	private void OnDisable()
	{
		nextButton.onClick.RemoveAllListeners();
	}
	
	public void Setup(string info,UnityAction nextOnClick,bool hasNextMission)
	{		
		nextButton.gameObject.SetActive(hasNextMission);
		
		if(nextOnClick != null)
			nextButton.onClick.AddListener(nextOnClick);
			
		victoryInfo.text = info;
		
	}
}
