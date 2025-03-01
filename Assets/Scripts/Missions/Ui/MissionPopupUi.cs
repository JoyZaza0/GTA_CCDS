using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class MissionPopupUi : MonoBehaviour
{
	[SerializeField] private Button startButton;
	[SerializeField] private Button cancelButton;
	
	[SerializeField] private TextMeshProUGUI desctripion;
	
	private void OnDisable()
	{
		startButton.onClick.RemoveAllListeners();
		cancelButton.onClick.RemoveAllListeners();
	}
	
	public void Setup(string info, UnityAction missionStart, UnityAction cancel = null)
	{
		if(missionStart != null)
			startButton.onClick.AddListener(missionStart);
		if(cancel != null)
			cancelButton.onClick.AddListener(cancel);
			
		desctripion.text = info;
	}
	
	
}
