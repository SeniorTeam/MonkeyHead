using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RockDropOne : MonoBehaviour 
{
	[SerializeField] int NumberOfRocks;
	int NumRocks;
	
	List<Transform> ListOfRocks = new List<Transform>();

	public void InitiateTrigger()
	{
		NumRocks = NumberOfRocks;
		StartCoroutine (DropRocks());
	}
	
	IEnumerator DropRocks()
	{
		if (NumRocks > 0)
		{
			Transform rock = Instantiate(Resources.Load("Prefabs/Traps/Rock"), transform.position, Quaternion.identity) as Transform;
			ListOfRocks.Add(rock);
			
			NumRocks--;
			yield return new WaitForSeconds(0.1f);
			StartCoroutine (DropRocks());
			yield return null;
		}
		yield return null;
	}
}
