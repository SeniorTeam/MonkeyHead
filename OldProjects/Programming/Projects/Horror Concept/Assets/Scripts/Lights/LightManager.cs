using UnityEngine;
using System.Collections;

public class LightManager : MonoBehaviour
{
	[SerializeField] LightFlicker[] Lights;
	float percentOff = 0.25f;
	float percentFlicker = 0.5f;
	int[]  lights = new int[50];
	
	void Awake()
	{
		int amount = Lights.Length;
		float LightsFlicker = amount * percentFlicker;
		float LightsOn =  amount * percentOff;
		
		for (int i = 0; i < Lights.Length; i++)
			lights[i] = i;
	
		for (int i=0; i < LightsFlicker; i++ )
		{
			int r = Random.Range(0, amount);
			Lights[r].DoesLightFlicker = false;
			
			lights[r] = -1;
		
			Debug.Log(Lights[r] + " -> NO FLICKER");
		}
		
		
		for (int i = 0; i < Lights.Length; ++i)
		{
			Debug.Log(lights[i]);
		}
			
		while (LightsOn > 0)
		{
			int i;
			do{
				i= Random.Range(0, amount);
				
				if (lights[i] == -1)
					i = -1;	
				
			}while(i == -1);
			
			Debug.Log(Lights[i] + " -> OFF");
				
			Lights[i].LightOn = false;
			LightsOn--;
		}
		
	}
	
}
