using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class RepairPopup : MonoBehaviour
{
	[SerializeField] private Button repairButton;
	//[SerializeField] private TextMeshProUGUI infoText;
	
	private void OnDisable()
	{
		repairButton.onClick.RemoveAllListeners();
	}
	
	public void Setup(UnityAction nextOnClick)
	{			
		if(nextOnClick != null)
			repairButton.onClick.AddListener(nextOnClick);					
	}
}
