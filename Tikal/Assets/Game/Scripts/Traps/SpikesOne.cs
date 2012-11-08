using UnityEngine;
using System.Collections;

public class SpikesOne : MonoBehaviour 
{
	void OnTriggerEnter(Collider hit)
	{
		if (hit.tag == "Player")
		{
			hit.GetComponent<Player_Info>().PlayerDied();
		}
	}
	
	public void InitiateTrigger()
	{
		transform.animation.Play();
	}
	
}
