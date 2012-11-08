using UnityEngine;
using System.Collections;

public class DissovleLoadingScreen : MonoBehaviour 
{
	[SerializeField] Game game;
	[SerializeField] EnemyManager enemy;
	[SerializeField] GUIText LoadingText;
	
	#region Songs
	public AudioClip[] Songs;
	int playListSize;
	
	float[] NoteDuration = {0.508f, 0.535f};
	public float Duration;
	bool onGui = true;
	
	#endregion
	
	float TransparentTime = 1.0f;
	
	void Awake()
	{
		playListSize = 0;
		foreach (AudioClip song in Songs)
		{
			if (song != null)
				playListSize++;
		}
		
		gameObject.renderer.enabled = true;
	}
	
	IEnumerator WaitForLoad()
	{
		while(true)
		{
			if (game.LoadingComplete)break;
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
	
	void OnGUI()
	{
		if (onGui)
		{
			for (int i=0; i < playListSize; i++)
			{	
				if (GUI.Button( new Rect((Screen.width/2) - 100, (110 * i) + 20, 200, 100), Songs[i].name + "\n  -" + Songs[i].length))
				{
					Duration = NoteDuration[i];
					game.Song = Songs[i];
					LoadingText.enabled = true;
					enemy.StartLoad = true;
					StartCoroutine(WaitForLoad());
					onGui = false;
				}
			}
		}
		
	}
	
	
	
}
