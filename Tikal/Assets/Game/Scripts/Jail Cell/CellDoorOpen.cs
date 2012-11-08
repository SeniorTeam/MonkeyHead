using UnityEngine;
using System.Collections;

public class CellDoorOpen : MonoBehaviour 
{
	[SerializeField] GameObject Hinge;
	[SerializeField] bool IsDoorOpened;
	bool doorOpened;
	
	
	void Awake()
	{
		if (IsDoorOpened)
			doorOpened = true;
		else
			doorOpened = false;
	}
	
	void OnTriggerStay(Collider hit)
	{
		if (hit.gameObject.tag == "Player")
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				PlayDoorAnimation(doorOpened);
			}
		}
	}
	
	// If door is opened reverse should be true
	void PlayDoorAnimation(bool reverse)
	{
		
		if (reverse)
		{
			Hinge.animation.Play("CellDoorClose");
			doorOpened = false;
		}
		else
		{
			Hinge.animation.Play("CellDoorOpen");
			doorOpened = true;
		}
	}
	
}
