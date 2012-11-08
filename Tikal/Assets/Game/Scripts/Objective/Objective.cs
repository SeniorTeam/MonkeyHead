using UnityEngine;
using System.Collections;

public class Objective : Weapon  
{
	[SerializeField] GameObject DropOff;
	bool Treasure = true;
	
	public override void Initialize()
	{
		base.Initialize();
	}
	
	void OnTriggerEnter(Collider hit)
	{
		if (Active && Treasure)
		{
			if (hit.gameObject.name == DropOff.name)
			{
				Debug.Log("WIN");
				Application.LoadLevel("Test_One");
			}
		}
	}
	
}
