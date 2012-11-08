using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	[SerializeField] GameObject StartButton;
	[SerializeField] GameObject TransparentLayer;
	[SerializeField] float FadeTime = 1.5f;
	
	string buttonName;
	Vector3 normSize, hoverSize;
	
	Color TransColor;
	
	bool MenuActive = false;
	
	
	void Awake()
	{
		buttonName = StartButton.name.ToString();
		
		Vector3 scale = StartButton.transform.localScale;
		normSize = new Vector3 (scale.x, scale.y, scale.z);
		hoverSize =  new Vector3 (scale.x - .01f , 0, scale.z - .01f);
		
		TransColor = TransparentLayer.renderer.material.color;
		TransColor.a = 1;
		TransparentLayer.renderer.material.color = TransColor;
		
		StartCoroutine(Fade(true));
	}
	
	void Update()
	{
		if (MenuActive)
			CheckButtonTouch();	
	}
	
	IEnumerator Fade(bool fadeIn)
	{
		int start, end;
		
		if (!fadeIn)
		{
			TransparentLayer.active = true;
			start = 1;
			end = 0;
		}
		else
		{
			start = 0;
			end = 1;
		}
		
		float alpha = TransparentLayer.renderer.material.color.a;
		
		float time = FadeTime;
		while (time > 0)
		{
			time -= Time.deltaTime;
			alpha = Mathf.Lerp(start, end, time);
			TransparentLayer.renderer.material.color = new Color (TransColor.r, TransColor.g, TransColor.b, alpha);
								
			yield return null;
		}
		
		
		if (!fadeIn)
			Application.LoadLevel("SnowLand");
		else
		{
			TransparentLayer.active = false;
			MenuActive = true;
		}
	}
	
	void CheckButtonTouch ()
	{
		Vector3 mousePos = Input.mousePosition;
		
		RaycastHit hit;
		string found = null;
		
		if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit) )
		{	
			found = hit.transform.name.ToString();
			
			if (found == buttonName)
				StartButton.transform.localScale = hoverSize;	
			else
				StartButton.transform.localScale = normSize;
		}
		
		// Button Click
		if (Input.GetMouseButtonDown(0) )
		{
			if (found != null)
			{
				if (found == buttonName)
				{
					StartButton.transform.localScale = normSize;
					StartCoroutine(Fade(false));
				}
			}
		}
		
	}
}
