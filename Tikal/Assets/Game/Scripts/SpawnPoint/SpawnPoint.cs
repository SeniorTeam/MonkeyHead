using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour 
{
	List<Transform> PlayersInRadius = new List<Transform>();
	
	void Awake()
	{
		foreach (Transform t in transform)
		{
			t.renderer.enabled = false;	
		}
	}
	public bool CheckSpawnPoint(List<Player_Info> TeamMates)
	{
		bool canSpawn = true;
		
		if (PlayersInRadius.Count > 0 )
		{
			for ( int i = 0; i < PlayersInRadius.Count; i++) 
			{
				int j = 0;
				foreach (Player_Info _teammate in TeamMates)	
				{
					if (_teammate == null)
					{
						TeamMates.RemoveAt(j);
						break;
					}
					
					
					if (PlayersInRadius[i] == null)
					{
						PlayersInRadius.RemoveAt(i);
						break;
					}
					else if (PlayersInRadius[i].name != _teammate.name)
					{
						canSpawn = false;
						break;
					}
					
					j++;
				}
			}
		}
		
		return canSpawn;
	}
	
	void OnTriggerStay(Collider hit)
	{
		if ( hit.transform.tag == "Player")
		{
			bool canAdd = true;
			foreach (Transform p in PlayersInRadius)
			{
				if (p == null)
				{
					canAdd = false;
					break;	
				}
				if (p.name == hit.transform.name)
				{
					canAdd = false;
					break;
				}
			}
			
			if (canAdd)
				PlayersInRadius.Add(hit.transform);
		}
			
	}
	
	void OnTriggerExit(Collider hit)
	{
		if ( hit.transform.tag == "Player")
			PlayersInRadius.Remove(hit.transform);
	}
	
}
