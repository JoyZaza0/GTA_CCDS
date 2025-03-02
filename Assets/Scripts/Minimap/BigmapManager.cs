using UnityEngine;
using MTAssets.EasyMinimapSystem;
using UnityEngine.UI;
using TMPro;

public class BigmapManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _missionName;
	[SerializeField] private TextMeshProUGUI _missionDescription;
	[SerializeField] private Button _teleportButton;
	private MinimapItem _lastClickedItem;


	public void OnClickInMinimapRendererArea(Vector3 clickWorldPos, MinimapItem clickedMinimapItem)
	{
		if(clickedMinimapItem == null) return;
		
		if(_lastClickedItem)
		{
			
			if(_lastClickedItem.Equals(clickedMinimapItem))
			{
				_lastClickedItem.particlesHighlightMode = MinimapItem.ParticlesHighlightMode.WavesIncrease;
				
				ClearInfo();
				return;
			}
			else
			{
				_lastClickedItem.particlesHighlightMode = MinimapItem.ParticlesHighlightMode.WavesIncrease;
			}
		}
		
		CCDS_Marker marker = clickedMinimapItem.GetComponent<CCDS_Marker>();
		
		if(marker)
		{
			_missionName.text = marker.lable.text;
			_missionDescription.text = marker.connectedMission.misssionStartInfo;
				
			clickedMinimapItem.particlesHighlightMode = MinimapItem.ParticlesHighlightMode.Disabled;
				
			_lastClickedItem = clickedMinimapItem;
			_teleportButton.gameObject.SetActive(true);
			
			return;
		}
		
		CCDS_RepairStation repairStation = clickedMinimapItem.GetComponent<CCDS_RepairStation>();
		
		if(repairStation)
		{
			_missionName.text = "Repair Station";
			_missionDescription.text = "Repair You Car";
				
			clickedMinimapItem.particlesHighlightMode = MinimapItem.ParticlesHighlightMode.Disabled;
				
			_lastClickedItem = clickedMinimapItem;
			_teleportButton.gameObject.SetActive(true);
			
		}
	}
	
	
	public void ClearInfo()
	{
		_missionName.text = "";
		_missionDescription.text = "";
		_lastClickedItem = null;
		_teleportButton.gameObject.SetActive(false);
	}
	
	public void TeleportToMarker(int price)
	{
		if(_lastClickedItem == null) return;
		
		var marker = _lastClickedItem.GetComponent<CCDS_Marker>();
		
		if(marker) 
		{
			if(CCDS.GetMoney() >= price)
			{
				if(BCG_EnterExitManager.Instance.activePlayer.inVehicle)
				{
					RCCP.Transport(marker.teleportPoint.position,marker.teleportPoint.rotation);
					CCDS.ChangeMoney(-price);
					GetComponent<MinimapManager>().OpenBigmap(false);
					ClearInfo();
				}
				else 
				{
					var player = BCG_EnterExitManager.Instance.activePlayer.transform;
				
					player.SetPositionAndRotation(marker.teleportPoint.position,marker.teleportPoint.rotation);
					CCDS.ChangeMoney(-price);
					GetComponent<MinimapManager>().OpenBigmap(false);
					ClearInfo();
				}
			}
			else
			{
				CCDS_UI_Informer.Instance.Info("Not Enough Money");
			}
			
			return;
		}
		
		var repairStation = _lastClickedItem.GetComponent<CCDS_RepairStation>();
		
		if(repairStation)
		{
			if(CCDS.GetMoney() >= price)
			{
				if(BCG_EnterExitManager.Instance.activePlayer.inVehicle)
				{
					RCCP.Transport(repairStation.TeleportPoint.position,repairStation.TeleportPoint.rotation);
					CCDS.ChangeMoney(-price);
					GetComponent<MinimapManager>().OpenBigmap(false);
					ClearInfo();
				}
				else 
				{
					var player = BCG_EnterExitManager.Instance.activePlayer.transform;
				
					player.SetPositionAndRotation(repairStation.TeleportPoint.position,repairStation.TeleportPoint.rotation);
					CCDS.ChangeMoney(-price);
					GetComponent<MinimapManager>().OpenBigmap(false);
					ClearInfo();
				}
			}
			else
			{
				CCDS_UI_Informer.Instance.Info("Not Enough Money");
			}
		}
		
	}
}
