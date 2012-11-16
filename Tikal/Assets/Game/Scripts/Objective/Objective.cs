using UnityEngine;
using System.Collections;

public class Objective : Weapon  
{
	[SerializeField] GameObject[] DropOffs;
	bool Treasure = true;
	
	public override void Initialize()
	{
		base.Initialize();
	}
	
	void OnTriggerEnter(Collider hit)
	{
		if (Active && Treasure)
		{
			foreach (GameObject dropoff in DropOffs)
			{
				if (hit.gameObject.name == dropoff.name)
				{
					Debug.Log("WIN");
					Application.LoadLevel("Level_One");
				}
			}
		}
	}
	
}
