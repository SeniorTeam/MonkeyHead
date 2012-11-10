using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour 
{
	[SerializeField] Camera RadarCamera;
	[SerializeField] GameObject[] RadarComponents;
	[SerializeField] int CameraHeight;
	public GameObject Player;
	
	HUD _Hud;
	Game c_Game;
	GameObject _Game;
	
	bool gameStart;
	
	
	void Awake()
	{
		RadarCamera.rect = new Rect (0, 0, 0, 0);
		_Game = GameObject.Find("Game");
		c_Game = _Game.GetComponent<Game>();
		StartCoroutine( GameStart() );
	}
	
	IEnumerator GameStart()
	{
		while (!c_Game.StartGame)
			yield return null;
		
		_Hud = transform.parent.GetComponent<HUD>();
		Player = GameObject.Find(_Hud.PlayerName);
		
		while (Player == null)
		{
			Player = GameObject.Find(_Hud.PlayerName);
			yield return null;
		}
		
		Initialize();
		yield return null;
	}

	void Initialize ()
	{		
		gameStart = true;
		DetermineCullingMask();
		DetermineCameraPort();
	}

	void DetermineCameraPort ()
	{
		RadarCamera.gameObject.active = false;
		int numP = c_Game.NumPlayers;	
		
		if (numP == 1)
			RadarCamera.rect = new Rect (0, 0, .18f, .25f);
		else if (numP == 2)
		{
			if (_Hud.HudNumber == 1)
				RadarCamera.rect = new Rect (0, .5f, .16f, .2f);
			else if (_Hud.HudNumber == 2)
				RadarCamera.rect = new Rect (0, 0, .16f, .2f);
		}
		else if (numP == 3)
		{
			if (_Hud.HudNumber == 1)
				RadarCamera.rect = new Rect (0, .5f, .16f, .2f);
			else if (_Hud.HudNumber == 2)
				RadarCamera.rect = new Rect (0, 0, .12f, .16f);
			else if (_Hud.HudNumber == 3)
				RadarCamera.rect = new Rect (.5f, 0, .12f, .16f);
		}
		else if (numP == 4)
		{
			if (_Hud.HudNumber == 1)
				RadarCamera.rect = new Rect (0, .5f, .12f, .16f);
			else if (_Hud.HudNumber == 2)
				RadarCamera.rect = new Rect (.5f, .5f, .12f, .16f);
			else if (_Hud.HudNumber == 3)
				RadarCamera.rect = new Rect (0, 0, .12f, .16f);
			else if (_Hud.HudNumber == 4)
				RadarCamera.rect = new Rect (.5f, 0, .12f, .16f);
		}
		
		RadarCamera.gameObject.active = true;
	}

	void DetermineCullingMask ()
	{
		// CullingMask
		// int represents layer number
		// ex 1:      1 | 1 << 9;     // layer 1 + layer 9
		// ex 2:      1 << 9;         // layer 9
		
		int CML = 13 + _Hud.HudNumber;  // CullingMask number and Layer number
		
		foreach (GameObject r in RadarComponents)
		{
			r.layer = CML;
		}
		
		RadarCamera.cullingMask = 1 << CML;
		
	}
	
	void Update()
	{
		if (gameStart)
		{
			FollowPlayer();	
		}
	}
	
	void FollowPlayer ()
	{
		if (Player == null)
		{
			Player = GameObject.Find(_Hud.PlayerName);
			DetermineCameraPort();
		}
		else
		{
			Vector3 pos = Player.transform.position;
			
			transform.position = new Vector3 (pos.x, transform.position.y , pos.z);
			RadarCamera.transform.position = new Vector3 (pos.x, CameraHeight , pos.z);
		}
	}
}
