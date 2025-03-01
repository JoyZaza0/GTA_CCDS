using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class VehicleSpawnPointsContainer : MonoBehaviour
{
	public List<CCDS_SpawnPoint> spawnPoints = new List<CCDS_SpawnPoint>();
	
	private void Awake()
	{
		GetAllSpawnPoints();
	}
	
	public void GetAllSpawnPoints()
	{
		if(spawnPoints == null)
			spawnPoints = new List<CCDS_SpawnPoint>();
			
		spawnPoints.Clear();
		
		spawnPoints = GetComponentsInChildren<CCDS_SpawnPoint>(true).ToList();
	}
	
	//private void OnDrawGizmos()
	//{
	//	if (spawnPoints == null)
	//		return;

	//	for (int i = 0; i < spawnPoints.Count; i++)
	//	{
	//		if (spawnPoints[i] != null && spawnPoints[i].gameObject.activeSelf) 
	//		{
	//			Gizmos.color = new Color(0.0f, 1.0f, 1.0f, 0.3f);
	//			Gizmos.DrawCube(spawnPoints[i].transform.position, spawnPoints[i].transform.localScale);
	//			//Gizmos.DrawWireSphere(spawnPoints[i].transform.position, 20f);
	//		}
	//	}
	//}
}
