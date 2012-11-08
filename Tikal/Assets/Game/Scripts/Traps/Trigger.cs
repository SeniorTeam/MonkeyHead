using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour 
{
	public enum TrapType
	{
		SpikesOne = 0,
		RockDropOne
	}
	
	public TrapType trapType;
	
	[SerializeField] GameObject Trap;
	[SerializeField] bool OneTime;
	[SerializeField] float CoolDownTime;
	[SerializeField] float Delay;
	bool trapActive = true;
	
	void OnTriggerEnter(Collider hit)
	{
		if (hit.tag == "Player")
		{
			if (trapActive)
			{
				StartCoroutine(CoolDown());
			}
		}
	}

	IEnumerator CoolDown()
	{
		trapActive = false;
		float delay = Delay;
		while( delay > 0)
		{
			delay -= Time.deltaTime;
			yield return null;
		}
		
		ActivateTrap();
		
		float time = CoolDownTime;
		
		while (	time > 0)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		
		if (!OneTime)
			trapActive = true;
		yield return null;
	}
	
	void ActivateTrap()
	{
		switch (trapType)
		{
			case TrapType.SpikesOne:
			{
				Trap.GetComponent<SpikesOne>().InitiateTrigger();
				break;
			}
			
			case TrapType.RockDropOne:
			{
				Trap.GetComponent<RockDropOne>().InitiateTrigger();
				break;
			}
		}
	}
}
