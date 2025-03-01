using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
	public VehicleSpawnPointsContainer spawnPointContainer;
	
	private void Awake()
	{
		spawnPointContainer = GetComponent<VehicleSpawnPointsContainer>();
	}
	
	
	public CCDS_SpawnPoint GetClosestPoint(Transform player)
	{
		if(spawnPointContainer == null || spawnPointContainer.spawnPoints.Count == 0)
		{
			Debug.LogError($" SpawnPointContainer is null");
			return null;
		}
			
		CCDS_SpawnPoint reslult = null;
		float closest = Mathf.Infinity;
		
		for (int i = 0; i < spawnPointContainer.spawnPoints.Count; i++) 
		{
			float distance = Vector3.Distance(spawnPointContainer.spawnPoints[i].transform.position,player.position);
			
			if(distance < closest)
			{
				reslult = spawnPointContainer.spawnPoints[i];
				closest = distance;
			}
			
		}
		
		return reslult;
	}
}
