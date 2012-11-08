using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnSector: MonoBehaviour 
{
	public string BaseName;
	public List<Player_Info> BasePlayers = new List<Player_Info>();
	[SerializeField] GameObject HomeBaseNode;
	[SerializeField] List<GameObject> FavoredNodes = new List<GameObject>();
	
	List<SpawnPoint> ListOfPoints = new List<SpawnPoint>();		// List of Spawn points on map
	
	void Awake()
	{
		SpawnPoint[] points = FindObjectsOfType( typeof(SpawnPoint) ) as SpawnPoint[];
		foreach ( SpawnPoint sp in points)
		{
			if (sp.transform.position != HomeBaseNode.transform.position)
			{
				bool canAdd = true;
				foreach ( GameObject fp in FavoredNodes)
				{
					if (sp.transform.position == fp.transform.position)
					{
						canAdd = false;
						break;
					}
				}
				
				if (canAdd)
					ListOfPoints.Add(sp);
			}
		}
	}
	
	public Vector3 FindSpawnLocation()
	{
		// Spawn Location set to default home base node
		Vector3 SpawnLoc = HomeBaseNode.transform.position;
		
		if (CheckHomeNode()){} 			// If true player spawns at home base, it is already set as default
		else if ( CheckFavoredNodes() )	// If true player spawns at desired favored node
			SpawnLoc = FavoredNode;		
		else
			SpawnLoc = CheckAllNodes();	// If all fails find a random location to spawn
		
		return SpawnLoc;
	}
	
	bool CheckHomeNode()
	{
		bool canSpawn = false;
		
		//Debug.Log("CheckHomeNode");
		
		if (HomeBaseNode.GetComponent<SpawnPoint>().CheckSpawnPoint(BasePlayers) )
			canSpawn = true;
		
		return canSpawn;
	}
	
	Vector3 FavoredNode;
	bool CheckFavoredNodes ()
	{
		bool canSpawn = false;
		
		//Debug.Log("CheckFavoredNodes");
		
		for (int i=0; i < FavoredNodes.Count; i++)
		{
			if (FavoredNodes[i].GetComponent<SpawnPoint>().CheckSpawnPoint(BasePlayers) )
			{
				FavoredNode = FavoredNodes[i].transform.position;
				canSpawn = true;
			}
		}

		return canSpawn;
	}
	
	Vector3 CheckAllNodes()
	{
		Vector3 v = new Vector3 (0,0,0);
		
		Debug.Log("CheckAllNodes");
		
		foreach (SpawnPoint sp in ListOfPoints)
		{
			if (sp.GetComponent<SpawnPoint>().CheckSpawnPoint(BasePlayers))
			{
				v = sp.transform.position;
				break;
			}
		}
		
		return v;
	}

}
