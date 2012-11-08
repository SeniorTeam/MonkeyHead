using UnityEngine;
using System.Collections;

public class JailCellCamera : MonoBehaviour 
{
	[SerializeField] GameObject JailCamera;
	
	
	void Awake()
	{
		JailCamera.active = false;	
	}
	
	void OnTriggerEnter(Collider hit)
	{
		if (hit.gameObject.tag == "Player")
		{
			//Debug.Log("Entered: " + gameObject.name);
			//JailCamera.active = true;
		}
	}
	
	void OnTriggerExit(Collider hit)
	{
		if (hit.gameObject.tag == "Player")
		{
			//Debug.Log("Exited: " + gameObject.name);
			//JailCamera.active = false;
		}
	}
	
}
