using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SplitScreen_Cameras))]

public class Game : MonoBehaviour 
{
	[SerializeField] WeaponManager _WeaponsManager;
	
	public bool StartGame;
	bool GameLoaded;
	public int NumPlayers;
	
	SplitScreen_Cameras ssCamera;
	
	public GameObject[] Base;
	public Player_Info[] player;
	public HUD[]    hud;
	
	
	void Awake () 
	{
		PlayerPrefs.SetInt("NumberOfPlayers", 4);
		
		//Screen.showCursor = false;	
		
		NumPlayers = PlayerPrefs.GetInt("NumberOfPlayers");
		
		ssCamera = transform.GetComponent<SplitScreen_Cameras>();
		ssCamera.Initialize();
		
		StartCoroutine(FirstSpawnPlayers());
		
		StartCoroutine(StartTheGame());
	}
	
	IEnumerator StartTheGame()
	{
		while (!GameLoaded && !_WeaponsManager.WeaponsLoaded)
		{
			yield return null;	
		}
		
		StartGame = true;	
	}
	
	int players = 0;
	int PlayerController;
	IEnumerator FirstSpawnPlayers()
	{
		players = NumPlayers;
		
		int num = 1;		
		#region Add a Player
		while (players > 0)
		{
			// Create Player Name ex: Player_1
			string pName = "Player_" + num.ToString();
			PlayerController = PlayerPrefs.GetInt(pName);
			
			#region Hud
			// Creat Players Hud
			GameObject hud = Instantiate(Resources.Load("Prefabs/Hud/HUD"), new Vector3 (0,0,0), Quaternion.identity) as GameObject;
			
			// Set Players Hud Name
			hud.name = "HUD_" + num.ToString();
			// Change Players Hud Tag to Hud
			hud.tag = "Hud";
			hud.layer = 8 + num; // Hud_1 starts at layer 9, 8+1 = 9. 
			hud.GetComponent<HUD>().HudNumber = num;
			hud.GetComponent<HUD>().PlayerName = pName.ToString();
			//hud.GetComponentInChildren<CLSRadar>().Player = pName.ToString();
			
			#endregion
			
			// Create Player
			GameObject player = Instantiate(Resources.Load("Prefabs/Characters/Player/Player"), new Vector3 (Base[num-1].transform.position.x, 1.1f, Base[num-1].transform.position.z ), Quaternion.identity) as GameObject;
			
			// Change Players Parent GameObject Name
			player.name = pName.ToString();
			
			// Change Players Tag to Player
			player.tag = "Player";
			
			// Give Player Id Name
			player.GetComponentInChildren<Player_Info>().PlayerID = pName.ToString();
			player.GetComponentInChildren<Player_Info>().PlayerNumber = num;
			player.GetComponentInChildren<Player_Info>().PlayerControllerNumber = PlayerController;
			player.GetComponentInChildren<Player_Info>().PlayerHUD = "HUD_" + num.ToString();
			
			player.GetComponentInChildren<Player_Info>().PlayerTeam = CreatePlayerTeam(num);
			Base[num-1].GetComponentInChildren<SpawnSector>().BasePlayers.Add(player.GetComponentInChildren<Player_Info>());
			
			#region Player Color
			for (int i = 0; i < 4; i++)
			{
				player.GetComponentInChildren<Player_Info>().Cloths[i].renderer.material.color = ChangePlayerColor(num);
			}
			#endregion
			
			#region Assign Players Controls
			
			#if UNITY_STANDALONE_WIN
			// Movement
			player.GetComponentInChildren<Player_InputManager>().Horizontal = PlayerController.ToString() + "_Move_Horizontal_PC";
			player.GetComponentInChildren<Player_InputManager>().Vertical = PlayerController.ToString() + "_Move_Vertical_PC";
			
			// Camera
			player.GetComponentInChildren<MouseLook>().Horizontal = PlayerController.ToString() + "_Look_Horizontal_PC";
			player.GetComponentInChildren<MouseLook>().Vertical = PlayerController.ToString() + "_Look_Vertical_PC";
			#endif
			
			#if UNITY_STANDALONE_OSX
			// Movement
			player.GetComponentInChildren<Player_InputManager>().Horizontal = PlayerController.ToString() + "_Move_Horizontal_MAC";
			player.GetComponentInChildren<Player_InputManager>().Vertical = PlayerController.ToString() + "_Move_Vertical_MAC";
			
			// Camera
			player.GetComponentInChildren<MouseLook>().Horizontal = PlayerController.ToString() + "_Look_Horizontal_MAC";
			player.GetComponentInChildren<MouseLook>().Vertical = PlayerController.ToString() + "_Look_Vertical_MAC";
			#endif
			
			#endregion
			
			players--;		// Next Player on the list going positive to negative
			num++;
			// Store Player
			
			yield return null;
		}
		#endregion
		
		InitPlayerCameras(true, 0);
		yield return null;
		
	}
	
	public void KillPlayer( int PlayerNumber, GameObject player)
	{
		StartCoroutine(RemoveAndDestroyPlayer(PlayerNumber, player));
	}
	
	IEnumerator RemoveAndDestroyPlayer( int PlayerNumber, GameObject player)
	{
		yield return new WaitForSeconds(.5f);
		
		string pHud = player.GetComponent<Player_Info>().PlayerHUD;
		GameObject hud = GameObject.Find(pHud);
		Destroy(player);
		Destroy(hud);
		StartCoroutine(SpawnPlayer(PlayerNumber));
		yield return null;
	}
	
	IEnumerator SpawnPlayer(int PlayerNumber)
	{
		yield return new WaitForSeconds(2.0f);
		
		int num = PlayerNumber;
		#region Add a Player
		// Create Player Name ex: Player_1
		string pName = "Player_" + num.ToString();
		PlayerController = PlayerPrefs.GetInt(pName);
		
		#region Hud
		// Creat Players Hud
		GameObject hud = Instantiate(Resources.Load("Prefabs/Hud/HUD"), new Vector3 (0,0,0), Quaternion.identity) as GameObject;
		
		// Set Players Hud Name
		hud.name = "HUD_" + num.ToString();
		// Change Players Hud Tag to Hud
		hud.tag = "Hud";
		hud.layer = 8 + num; // Hud_1 starts at layer 9, 8+1 = 9. 
		hud.GetComponent<HUD>().HudNumber = num;
		hud.GetComponent<HUD>().PlayerName = pName.ToString();
		//hud.GetComponentInChildren<CLSRadar>().Player = pName.ToString();
		
		#endregion

		// Create Player
		GameObject player = Instantiate(Resources.Load("Prefabs/Characters/Player/Player"), new Vector3 (0,100,0), Quaternion.identity) as GameObject;
		
		// Change Players Parent GameObject Name
		player.name = pName.ToString();
		
		// Change Players Tag to Player
		player.tag = "Player";
		
		// Give Player Id Name
		player.GetComponentInChildren<Player_Info>().PlayerID = pName.ToString();
		player.GetComponentInChildren<Player_Info>().PlayerNumber = num;
		player.GetComponentInChildren<Player_Info>().PlayerControllerNumber = PlayerController;
		player.GetComponentInChildren<Player_Info>().PlayerHUD = "HUD_" + num.ToString();
		
		player.GetComponentInChildren<Player_Info>().PlayerTeam = CreatePlayerTeam(num);
		Base[num-1].GetComponentInChildren<SpawnSector>().BasePlayers.Add(player.GetComponentInChildren<Player_Info>());
		
		Vector3 SpawnPoint = Base[num-1].GetComponentInChildren<SpawnSector>().FindSpawnLocation();
		player.transform.position = new Vector3 (SpawnPoint.x, 1.1f, SpawnPoint.z );
		
		#region Player Color
		for (int i = 0; i < 4; i++)
		{
			player.GetComponentInChildren<Player_Info>().Cloths[i].renderer.material.color = ChangePlayerColor(num);
		}
		#endregion
		
		#region Assign Players Controls
			
		#if UNITY_STANDALONE_WIN
		// Movement
		player.GetComponentInChildren<Player_InputManager>().Horizontal = PlayerController.ToString() + "_Move_Horizontal_PC";
		player.GetComponentInChildren<Player_InputManager>().Vertical = PlayerController.ToString() + "_Move_Vertical_PC";
		
		// Camera
		player.GetComponentInChildren<MouseLook>().Horizontal = PlayerController.ToString() + "_Look_Horizontal_PC";
		player.GetComponentInChildren<MouseLook>().Vertical = PlayerController.ToString() + "_Look_Vertical_PC";
		#endif
		
		#if UNITY_STANDALONE_OSX
		// Movement
		player.GetComponentInChildren<Player_InputManager>().Horizontal = PlayerController.ToString() + "_Move_Horizontal_MAC";
		player.GetComponentInChildren<Player_InputManager>().Vertical = PlayerController.ToString() + "_Move_Vertical_MAC";
		
		// Camera
		player.GetComponentInChildren<MouseLook>().Horizontal = PlayerController.ToString() + "_Look_Horizontal_MAC";
		player.GetComponentInChildren<MouseLook>().Vertical = PlayerController.ToString() + "_Look_Vertical_MAC";
		#endif
		
		#endregion
		
		players--;		// Next Player on the list going positive to negative
		num++;
	
		#endregion
		
		InitPlayerCameras(false, PlayerNumber);
		yield return null;
		
	}
	
	void InitPlayerCameras(bool allPlayers, int currentPlayer)
	{
		player = FindObjectsOfType(typeof(Player_Info)) as Player_Info[];
		hud = FindObjectsOfType(typeof(HUD)) as HUD[];
			
		foreach (Player_Info p in player)
		{
			if (allPlayers)
			{
				if (p.PlayerNumber == 1)
					FindCameraOnPlayer(p, ssCamera.Player_Camera[ssCamera.PlayerCamera_String[0]][NumPlayers-1]);
				else if (p.PlayerNumber == 2)
					FindCameraOnPlayer(p, ssCamera.Player_Camera[ssCamera.PlayerCamera_String[1]][NumPlayers-1]);
				else if (p.PlayerNumber == 3)
					FindCameraOnPlayer(p, ssCamera.Player_Camera[ssCamera.PlayerCamera_String[2]][NumPlayers-1]);
				else if (p.PlayerNumber == 4)
					FindCameraOnPlayer(p,ssCamera.Player_Camera[ssCamera.PlayerCamera_String[3]][NumPlayers-1]);
			} 
			else
			{
				if (p.PlayerNumber == currentPlayer)
					FindCameraOnPlayer(p, ssCamera.Player_Camera[ssCamera.PlayerCamera_String[currentPlayer-1]][NumPlayers-1]);
			}
		}
		
		foreach (HUD h in hud)
		{
			
			if (allPlayers)
			{
				if (h.HudNumber == 1)
					ChangeHUD(h, ssCamera.Player_Camera[ssCamera.PlayerCamera_String[0]][NumPlayers-1],
						ssCamera.Player_HudCamera[ssCamera.Player_HudCamera_String[0]][NumPlayers-1],
						ssCamera.Player_CrossHair[ssCamera.Player_CrossHair_String[0]][NumPlayers-1]);
				else if (h.HudNumber == 2)
					ChangeHUD(h, ssCamera.Player_Camera[ssCamera.PlayerCamera_String[1]][NumPlayers-1],
						ssCamera.Player_HudCamera[ssCamera.Player_HudCamera_String[1]][NumPlayers-1],
						ssCamera.Player_CrossHair[ssCamera.Player_CrossHair_String[1]][NumPlayers-1]);
				else if (h.HudNumber == 3)
					ChangeHUD(h, ssCamera.Player_Camera[ssCamera.PlayerCamera_String[2]][NumPlayers-1],
						ssCamera.Player_HudCamera[ssCamera.Player_HudCamera_String[2]][NumPlayers-1],
						ssCamera.Player_CrossHair[ssCamera.Player_CrossHair_String[2]][NumPlayers-1]);
				else if (h.HudNumber == 4)
					ChangeHUD(h, ssCamera.Player_Camera[ssCamera.PlayerCamera_String[3]][NumPlayers-1],
						ssCamera.Player_HudCamera[ssCamera.Player_HudCamera_String[3]][NumPlayers-1],
						ssCamera.Player_CrossHair[ssCamera.Player_CrossHair_String[3]][NumPlayers-1]);
			} 
			else
			{
				if (h.HudNumber == currentPlayer)
					ChangeHUD(h, ssCamera.Player_Camera[ssCamera.PlayerCamera_String[currentPlayer-1]][NumPlayers-1],
						ssCamera.Player_HudCamera[ssCamera.Player_HudCamera_String[currentPlayer-1]][NumPlayers-1],
						ssCamera.Player_CrossHair[ssCamera.Player_CrossHair_String[currentPlayer-1]][NumPlayers-1]);
			}
		}
		
	}
	
	void FindCameraOnPlayer(Player_Info player, Rect view)
	{
		foreach (Transform _trans in player.transform)
		{
			
			if (_trans.tag == "MainCamera")
			{
				_trans.camera.rect = view; 
			}
		}
	}
	
	void ChangeHUD(HUD hud, Rect view, Vector3 pos, Vector2 dim)
	{
		//float width = dim.x;
		//float height = dim.y;
		
		foreach (Transform _trans in hud.transform)
		{
			_trans.gameObject.layer = hud.gameObject.layer;
			
			if (_trans.tag == "MainCamera")
			{
				_trans.camera.cullingMask = 1 << (9 + (hud.HudNumber-1) );    // int represents layer number // ex:  1 | 1 << (9+num); // layer 1 + layer 9
				_trans.camera.rect = view;
				_trans.position = new Vector3((hud.HudNumber-1)*10, -10, 0);
			}
//			if (_trans.tag == "CrossHair")
//			{
//				//_trans.position = pos;
//				Rect inset = _trans.guiTexture.pixelInset;
//				_trans.guiTexture.pixelInset = new Rect (inset.x, inset.y, width, height);
//			}
			if (_trans.name == "Weapons")
			{
				foreach (Transform _t in _trans)
				{
					_t.gameObject.layer = hud.gameObject.layer;
				}
			}
		}
		
		GameLoaded = true;
	}
		
	Color ChangePlayerColor(int num)
	{
		Color color = Color.red;
		
		if (num == 2)
			color = Color.blue;
		if (num == 3)
			color = Color.green;
		if (num == 4)
			color = Color.yellow;
		
		return color;
	}

	string CreatePlayerTeam (int num)
	{
		string s = "Red Team";
		
		if (num == 2)
			s = "Blue Team";
		if (num == 3)
			s = "Green Team";
		if (num == 4)
			s = "Yellow Team";
		
		return s;
	}
	
}
