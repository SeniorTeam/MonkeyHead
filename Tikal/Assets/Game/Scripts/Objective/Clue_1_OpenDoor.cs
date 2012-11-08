using UnityEngine;
using System.Collections;

public class Clue_1_OpenDoor : Weapon 
{
	[SerializeField] GameObject DropOff;
	[SerializeField] GameObject ArtifactSpot;
	[SerializeField] GameObject BoulderHolder;
	
	bool doOnce;
	
	public override void Initialize()
	{
		base.Initialize();
	}
	
	void OnTriggerStay(Collider hit)
	{
		if (!Active && !doOnce)
		{	
			if (hit.gameObject.name == DropOff.name)
			{
				doOnce = true;
				transform.position = ArtifactSpot.transform.position;
				transform.rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
				ArtifactSpot.animation.Play();
				BoulderHolder.collider.isTrigger = true;
				transform.GetComponent<Clue_1_OpenDoor>().enabled = false;
			}
		}
	}
}
