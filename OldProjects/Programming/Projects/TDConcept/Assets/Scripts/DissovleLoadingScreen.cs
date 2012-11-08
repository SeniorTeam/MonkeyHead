using UnityEngine;
using System.Collections;

public class DissovleLoadingScreen : MonoBehaviour 
{
	[SerializeField] AssetManager assetManager;
	[SerializeField] GUIText LoadingText;
	
	float TransparentTime = 1.0f;
	
	void Awake()
	{
		gameObject.renderer.enabled = true;
		LoadingText.enabled = true;
		StartCoroutine(WaitForLoad());
	}
	
	IEnumerator WaitForLoad()
	{
		while(true)
		{
			if (assetManager.LoadingComplete)break;
			yield return null;	
		}
		
		LoadingText.enabled = false;
		Color c = gameObject.renderer.material.color;
		float time = TransparentTime;
		while(time > 0)
		{
			time -= Time.deltaTime;
			
			float lerp = Mathf.Lerp(0, 1, time);
			
			gameObject.renderer.material.color = new Color (c.r, c.g, c.b, lerp);
			yield return null;
		}
	}
	
	
	
}
