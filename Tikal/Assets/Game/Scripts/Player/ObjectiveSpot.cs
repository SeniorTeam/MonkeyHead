using UnityEngine;
using System.Collections;

public class ObjectiveSpot : MonoBehaviour 
{
	public GameObject holder;
	
	void Awake()
	{
		holder = gameObject;	
	}
}
