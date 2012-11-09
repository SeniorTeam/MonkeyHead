using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour 
{
	Game c_Game;
	HUD  c_Hud;
	GameObject _Game;

	
	void Awake()
	{
		_Game = GameObject.Find("Game");
		c_Game = _Game.GetComponent<Game>();
		
		c_Hud = transform.parent.GetComponent<HUD>();
		
		StartCoroutine( Init() );
	}
	
	IEnumerator Init()
	{
		while (!c_Game.StartGame)
		{
			yield return null;	
		}
		
		GetDamageScreenSize();
	}
	
	//define Fade parmeters
	IEnumerator HitMarker (float start, float end, float time, GameObject currentObject) 
	{
		Color color = currentObject.guiTexture.color;
		float alpha = color.a;
		float totalTime = time;
		
		time = 0;
		while (true)
		{
			if ( time >= totalTime)
			{
				alpha = end;
				currentObject.guiTexture.color = new Color (color.r, color.g, color.b, alpha);
				break;
			}
			else
				time += Time.deltaTime * (1/totalTime);
			
			alpha = Mathf.Lerp(start, end, time);
			
			currentObject.guiTexture.color = new Color (color.r, color.g, color.b, alpha);
			
			yield return null;
		}
		
		yield return new WaitForSeconds(0.2f);
		
		time = 0;
		while (true)
		{
			if ( time >= totalTime)
			{
				alpha = start;
				currentObject.guiTexture.color = new Color (color.r, color.g, color.b, alpha);
				break;
			}
			else
				time += Time.deltaTime * (1/totalTime);
			
			alpha = Mathf.Lerp(end, start, time);
			
			currentObject.guiTexture.color = new Color (color.r, color.g, color.b, alpha);
			yield return null;
		}
	}
	
	public void ApplyHit()
	{
		StartCoroutine( HitMarker(0, 0.4f, 0.5f, gameObject) );
	}
	
	void GetDamageScreenSize()
	{
		int NumPlayers = c_Game.NumPlayers;
		
		Rect pixel = gameObject.guiTexture.pixelInset;
		float w = Screen.width;
		float h = Screen.height;
		
		
		if ( NumPlayers == 4)
		{
			w = w/2;
			h = h/2;
		}
		else if ( NumPlayers == 3)
		{
			if ( c_Hud.HudNumber == 1)
			{
				//w = w;
				h = h/2;
			}
			else
			{
				w = w/2;
				h = h/2;
			}
		}
		else if ( NumPlayers == 2)
		{
			//w = w;
			h = h/2;
		}
		else if ( NumPlayers == 1)
		{
			//w = w;
			//h = h;
		}
		
		
		gameObject.guiTexture.pixelInset = new Rect (pixel.x, pixel.y, w, h);
	}
}
