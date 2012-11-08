using UnityEngine;
using System.Collections;

public class LogoLoad : MonoBehaviour 
{
	[SerializeField] GameObject SplashLogo;
	[SerializeField] AudioClip  _audio;
	
	[SerializeField] string SceneToLoad;
	
	[SerializeField] float LogoDisplayTime = 2.0f;
	[SerializeField] float DelayToLoadScreen = 1.0f;
	[SerializeField] float FadeTime = 2.0f;
	
	Color splashColor;
	
	void Awake()
	{
		splashColor = SplashLogo.renderer.material.color;
		splashColor.a = 0;
		SplashLogo.renderer.material.color = splashColor;
		
		StartCoroutine(FadeIn());
	}
	
	IEnumerator FadeIn()
	{
		//Debug.Log("FadeIn");
		float alpha = SplashLogo.renderer.material.color.a;
		
		float time = FadeTime;
		while (time > 0)
		{
			time -= Time.deltaTime;
			alpha = Mathf.Lerp(1, 0, time);
			SplashLogo.renderer.material.color = new Color (splashColor.r, splashColor.g, splashColor.b, alpha);
			
			yield return null;
		}
		
		AudioPlay();
		StartCoroutine(DelayTime(LogoDisplayTime, "FadeOut"));
	}
	
	IEnumerator DelayTime(float time, string type)
	{
		while (time > 0)
		{
			time -= Time.deltaTime;		
			yield return null;
		}
		
		switch(type)
		{
			case "FadeOut":
			{
				StartCoroutine(FadeOut());
				break;
			}
			case "LoadScene":
			{
				LoadScene();
				break;
			}
		}
		
		yield return null;
		
	}
	
	IEnumerator FadeOut()
	{
		//Debug.Log("FadeOut");
		float alpha = SplashLogo.renderer.material.color.a;
		
		float time = FadeTime;
		while (time > 0)
		{
			time -= Time.deltaTime;
			alpha = Mathf.Lerp(0, 1, time);
			SplashLogo.renderer.material.color = new Color (splashColor.r, splashColor.g, splashColor.b, alpha);
								
			yield return null;
		}
			
		StartCoroutine(DelayTime(DelayToLoadScreen, "LoadScene"));
	}

	void AudioPlay ()
	{
		if (_audio != null)
		{
			audio.clip = _audio;
			audio.Play();
		}
	}
	
	void LoadScene ()
	{
		//Debug.Log("LoadScene");
		if (SceneToLoad != null)
			Application.LoadLevel(SceneToLoad);
	}
}
