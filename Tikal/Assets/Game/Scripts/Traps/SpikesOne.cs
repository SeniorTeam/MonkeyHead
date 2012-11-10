using UnityEngine;
using System.Collections;

public class SpikesOne : MonoBehaviour 
{
	void OnTriggerEnter(Collider hit)
	{
		if (hit.tag == "Player")
		{
			hit.GetComponent<Player_Info>().ApplyDamage(100);
		}
	}
	
	public void InitiateTrigger()
	{
		transform.animation.Play();
	}
	
}
