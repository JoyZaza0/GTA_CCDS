using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MTAssets.EasyMinimapSystem;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{	
	public enum IconMode{Player,Car}
	[SerializeField] private FCG.TrafficSystem _trafficSystem;
	
	[SerializeField] private MinimapCamera _bigmapCamera;
	
	[SerializeField] private MinimapRenderer _minimapRenderer;
	[SerializeField] private MinimapRenderer _bigmapRenderer;
	
	[SerializeField] private Sprite _playerIcon;
	[SerializeField] private Sprite _carIcon;
	[SerializeField] private Sprite _particleIcon;
	
	[SerializeField] private GameObject _missionPanel;
	
	[SerializeField] private Vector3 _minimapPlayerIconSize;
	[SerializeField] private Vector3 _minimapCarIconSize;
	[SerializeField] private Color _particleColor;
	

	private void OnEnable()
	{
		BCG_EnterExitPlayer.OnBCGPlayerEnteredAVehicle += OnEnterVehicle;
		BCG_EnterExitPlayer.OnBCGPlayerExitedFromAVehicle += OnExitVehicle;
		BCG_EnterExitVehicle.OnBCGVehicleSpawned += OnPlayerVehicleSpawned;
		BCG_EnterExitPlayer.OnBCGPlayerSpawned += OnCharacterSpawned;
	}
	
	private void OnDisable()
	{
		BCG_EnterExitPlayer.OnBCGPlayerEnteredAVehicle -= OnEnterVehicle;
		BCG_EnterExitPlayer.OnBCGPlayerExitedFromAVehicle -= OnExitVehicle;
		BCG_EnterExitPlayer.OnBCGPlayerSpawned -= OnCharacterSpawned;
	}
	
	private void OnEnterVehicle(BCG_EnterExitPlayer player, BCG_EnterExitVehicle vehicle)
	{
		if(CCDS_SceneManager.Instance.levelType == CCDS_SceneManager.LevelType.MainMenu)
			return;
			
		MinimapCamera camera = vehicle.GetComponent<MinimapCamera>();
		
		if(camera != null)
		{
			_minimapRenderer.minimapCameraToShow = camera;
		}
		
		MinimapItem minimapItem = vehicle.GetComponent<MinimapItem>();
		
		if(minimapItem)
		{
			SetPlayerMinimapIcon(minimapItem,IconMode.Player,false);
			SetAllMinimapItemRotationTarget(minimapItem,false);
		}
				
		_trafficSystem.player = vehicle.transform;
	}
	
	private void OnExitVehicle(BCG_EnterExitPlayer player, BCG_EnterExitVehicle vehicle)
	{
		if(CCDS_SceneManager.Instance.levelType == CCDS_SceneManager.LevelType.MainMenu)
			return;
			
		MinimapCamera camera = player.GetComponent<MinimapCamera>();
		
		if(camera != null)
		{
			_minimapRenderer.minimapCameraToShow = camera;
		}
		
		MinimapItem minimapItemCar = vehicle.GetComponent<MinimapItem>();
		
		if(minimapItemCar)
		{
			SetPlayerMinimapIcon(minimapItemCar,IconMode.Car,false);
		}
				
		MinimapItem minimapItemPlayer = player.GetComponent<MinimapItem>();
		
		if(minimapItemPlayer)
		{
			SetPlayerMinimapIcon(minimapItemPlayer,IconMode.Player,false);
		}
		
		SetAllMinimapItemRotationTarget(minimapItemCar,false);
		_trafficSystem.player = player.transform;
	}
	
	private void OnPlayerVehicleSpawned(BCG_EnterExitVehicle vehicle)
	{
		if(CCDS_SceneManager.Instance.levelType == CCDS_SceneManager.LevelType.MainMenu)
			return;
			
		MinimapItem minimapItem = vehicle.GetComponent<MinimapItem>();
		
		if(minimapItem != null)
		{
			SetPlayerMinimapIcon(minimapItem,IconMode.Car,false);
			SetAllMinimapItemRotationTarget(minimapItem,false);
			//SetAllMarkersMinimapItemsSize(minimapItem,false);
		}
		
	}
	
	private void OnCharacterSpawned(BCG_EnterExitPlayer player)
	{
		_trafficSystem.player = player.transform;
		
		var camera = player.GetComponent<MinimapCamera>();
		
		if(_minimapRenderer.gameObject.active == false)
		{
			if(camera)
			{
				_minimapRenderer.minimapCameraToShow = camera;
				_minimapRenderer.gameObject.SetActive(true);
			}
		}
		
		MinimapItem minimapItem = player.GetComponent<MinimapItem>();
		
		if(minimapItem != null)
		{
			SetAllMarkersMinimapItemsSize(minimapItem,false);
		}
	}
	
	//private IEnumerator Setup(MinimapItem minimapItem)
	//{
	//	yield return new WaitForEndOfFrame();	
	//}
	
	private void SetPlayerMinimapIcon(MinimapItem item, IconMode iconMode,bool isBigMap)
	{
		if(iconMode == IconMode.Player)
		{
			item.itemSprite = _playerIcon;
			item.sizeOnMinimap = isBigMap ?_minimapPlayerIconSize * 2 : _minimapPlayerIconSize;
		}
		else if(iconMode == IconMode.Car)
		{
			item.itemSprite = _carIcon;
			item.sizeOnMinimap = isBigMap ? _minimapCarIconSize * 2 : _minimapCarIconSize;
		}
	}
	
	
	private void SetAllMarkersMinimapItemsSize(MinimapItem item, bool isBigmap)
	{
		//MinimapItem[] items = item.GetListOfAllMinimapItemsInThisScene();
		MinimapItem[] items = _minimapRenderer.minimapItemsToHightlight.ToArray();
		
		if(items != null)
		{
			if(isBigmap)
			{
				for (int i = 0; i < items.Length; i++)
				{
					items[i].sizeOnMinimap = new Vector3(40,0,40);
					items[i].sizeOnHighlight = 70;
					
					if(items[i].GetComponent<CCDS_AI_Cop>())
					{
						continue;
					}
					
					items[i].particlesSprite = _particleIcon;
					items[i].particlesColor = _particleColor;
					items[i].particlesSizeMultiplier = 0.4f;
					items[i].particlesHighlightMode = MinimapItem.ParticlesHighlightMode.WavesIncrease;
				}
			}
			else
			{
				for (int i = 0; i < items.Length; i++)
				{
					items[i].sizeOnMinimap = new Vector3(18,0,18);
					items[i].sizeOnHighlight = 30;
					
					Debug.Log(items[i].name);
					
					if(items[i].GetComponent<CCDS_AI_Cop>())
					{
						continue;
					}
					
					items[i].particlesHighlightMode = MinimapItem.ParticlesHighlightMode.Disabled;
				}
			}
		}
	}
	
	private void SetAllMinimapItemRotationTarget(MinimapItem item,bool isBigmap)
	{
		var items = item.GetListOfAllMinimapItemsInThisScene();
		
		if(isBigmap)
		{
			Transform target = _bigmapCamera.transform;
			
			for (int i = 0; i < items.Length; i++)
			{
				items[i].followRotationOf = MinimapItem.FollowRotationOf.CustomGameObject;
				items[i].customGameObjectToFollowRotation = target;
			}
			
			return;
		}
		
		bool inVehicle = BCG_EnterExitManager.Instance.activePlayer.inVehicle? true : false;		
		
		if(inVehicle)
		{
			Transform target = BCG_EnterExitManager.Instance.activePlayer.targetVehicle.transform;
			
			for (int i = 0; i < items.Length; i++)
			{
				items[i].followRotationOf = MinimapItem.FollowRotationOf.CustomGameObject;
				items[i].customGameObjectToFollowRotation = target;
			}
		}
		else
		{
			Transform target = BCG_EnterExitManager.Instance.activePlayer.transform;
			
			for (int i = 0; i < items.Length; i++)
			{
				items[i].followRotationOf = MinimapItem.FollowRotationOf.CustomGameObject;
				items[i].customGameObjectToFollowRotation = target;
			}

		}
		
	}
	
	
	public void OnClickInMinimapRendererArea(Vector3 clickWorldPos, MinimapItem clickedMinimapItem) 
	{
		OpenBigmap(true);
		
		bool inVehicle = BCG_EnterExitManager.Instance.activePlayer.inVehicle? true : false;
		
		if(inVehicle)
		{
			_bigmapCamera.transform.position = BCG_EnterExitManager.Instance.activePlayer.targetVehicle.transform.position;
		}
		else
		{
			_bigmapCamera.transform.position = BCG_EnterExitManager.Instance.activePlayer.transform.position;
		}
	}
	
	public void OnDragInMinimapRendererArea(Vector3 onStartThisDragWorldPos, Vector3 onDraggingWorldPos)
	{
		Vector3 deltaPositionToMoveMap = (onDraggingWorldPos - onStartThisDragWorldPos) * -1.0f;
		_bigmapCamera.transform.localPosition += (deltaPositionToMoveMap * 10.0f * Time.deltaTime);
	}
	
	public void OpenBigmap(bool isOpen)
	{
		_minimapRenderer.gameObject.SetActive(!isOpen);
		_bigmapRenderer.gameObject.SetActive(isOpen);		
		_bigmapCamera.gameObject.SetActive(isOpen);
		
		MinimapItem playerItem;
		MinimapItem carItem;
		
		if(BCG_EnterExitManager.Instance.activePlayer.inVehicle)
		{
			playerItem = BCG_EnterExitManager.Instance.activePlayer.targetVehicle.GetComponent<MinimapItem>();
			
			if(playerItem)
				SetPlayerMinimapIcon(playerItem,IconMode.Player,isOpen);
		}
		else
		{
			playerItem = BCG_EnterExitManager.Instance.activePlayer.GetComponent<MinimapItem>();
			
			if(playerItem)
				SetPlayerMinimapIcon(playerItem,IconMode.Player,isOpen);
			
			carItem = CCDS_GameplayManager.Instance.player.GetComponent<MinimapItem>();
			
			if(carItem)
				SetPlayerMinimapIcon(carItem,IconMode.Car,isOpen);
		} 
	
		
		if(playerItem != null)
		{
			SetAllMinimapItemRotationTarget(playerItem, isOpen);
			SetAllMarkersMinimapItemsSize(playerItem, isOpen);
		}
		
	
		BCG_EnterExitManager.Instance.cachedCanvas.HidePlayerUi(isOpen);
		BCG_EnterExitManager.Instance.cachedCanvas.playerCanvasGroup.alpha = isOpen ? 0 : 1;
		BCG_EnterExitManager.Instance.cachedCanvas.playerCanvasGroup.blocksRaycasts = isOpen? false : true;
		
		_missionPanel.gameObject.SetActive(isOpen);
		
		CCDS_UI_Manager.Instance.Fade();

	}
	
}
