using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class MissionFailedPopup : MonoBehaviour
{
	[SerializeField] private Button restartButton;
	[SerializeField] private TextMeshProUGUI lable;
	
	private void OnDisable()
	{
		restartButton.onClick.RemoveAllListeners();
	}
	
	public void Setup(string info,UnityAction restartOnClick)
	{
		restartButton.onClick.AddListener(restartOnClick);
		lable.text = info;
	}
	
	
}
