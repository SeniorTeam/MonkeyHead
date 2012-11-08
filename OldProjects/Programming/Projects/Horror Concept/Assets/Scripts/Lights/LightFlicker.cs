using UnityEngine;
using System.Collections;


public class LightFlicker : MonoBehaviour 
{
	public bool DoesLightFlicker;
	public bool LightOn;
	[SerializeField] Light PointLight;
	[SerializeField] GameObject Glass;
	
	Shader offShader, onShader;
	int flickerAmount = 0;
	float timeDelay;		// Time inbetween each group of flickers
	float timeFlicker;		// Time inbetween each individual flicker
	
	void Start()
	{	
		if (DoesLightFlicker)
		{
			onShader = Shader.Find("Unlit/Transparent");
			offShader  = Shader.Find("Transparent/Diffuse");
			
			if (LightOn)
				LightTurnOn();
			else
				LightTurnOff();
			
			CreateFlicker();
		}
	}

	#region Functions
	int   NewFlickerAmount()
	{
		return Random.Range(5,10);
	}
	float NewFlickerTime()
	{
		return Random.Range(0.0f, 0.3f);
	}
	float NewFlickerDelay()
	{
		return Random.Range(3.0f, 5.0f);
	}
	
	void CreateFlicker()
	{
		flickerAmount = NewFlickerAmount();
		timeDelay = NewFlickerDelay();
		timeFlicker = NewFlickerTime();
		
		StartCoroutine(CanFlicker());
	}
	
	IEnumerator CanFlicker()
	{
		int   amount = flickerAmount;
		float delay = timeDelay;
		float flicker = timeFlicker;
		
		while (delay > 0)
		{
			delay -= Time.deltaTime;
			yield return null;
		}
		
		SwitchLightPreference();
		
		while (amount > 0)
		{
			
			while (flicker > 0)
			{
				flicker -= Time.deltaTime;
				yield return null;
			}
			
			flicker = timeFlicker;
			amount--;
			SwitchLightPreference();
			yield return null;
		}
		
		CreateFlicker();
	}
	
	void SwitchLightPreference()
	{
		if (LightOn)
			LightTurnOff();
		else
			LightTurnOn();
	}
	public void LightTurnOn()
	{
		LightOn = true;
		
		Glass.renderer.material.shader = onShader;
		PointLight.light.enabled = true;
	}
	public void LightTurnOff()
	{
		LightOn = false;
		
		Glass.renderer.material.shader = offShader;
		PointLight.light.enabled = false;
	}
	#endregion
	
	
}
