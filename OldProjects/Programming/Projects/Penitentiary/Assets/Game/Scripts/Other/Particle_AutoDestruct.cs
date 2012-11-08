using UnityEngine;
using System.Collections;

public class Particle_AutoDestruct : MonoBehaviour 
{
	[SerializeField] float TimeOfDestruction;
	
	void Update()
	{
		Destroy(gameObject, TimeOfDestruction);
	}
	
}
