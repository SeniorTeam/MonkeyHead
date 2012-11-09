using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour 
{
	[SerializeField] TextMesh CountDownTimer;
	[SerializeField] string LevelToLoad;
	[SerializeField] GameObject StartButton;
	[SerializeField] GameObject TransparentLayer;
	[SerializeField] float FadeTime = 1.5f;
	
	bool[] PlayersActive;
	bool[] PlayerReady;
	
	Color TransColor;
	
	bool MenuActive = false;
	bool displayCounter = false;
	float countdownTimer = 3;
	
	
	void Awake()
	{	
		CountDownTimer.text = "";
		PlayersActive = new bool[4];
		PlayerReady = new bool[4];
		
		for (int i = 0; i < 4; i++)
		{
			PlayersActive[i] = false;
			PlayerReady[i] = false;
		}
	
		TransColor = TransparentLayer.renderer.material.color;
		TransColor.a = 1;
		TransparentLayer.renderer.material.color = TransColor;
		
		StartCoroutine(Fade(true));
	}
	
	void DetectPlayers()
	{
		int playerNum = 1;
		while (playerNum < 5) 
		{
			// Hit start to log in
		    if ( Input.GetKeyDown( transform.GetComponent<Player_Controls>().Get_START( playerNum ) ) )
				PlayersActive[playerNum-1] = true;
			
			// Hit A to log in
			if ( Input.GetKeyDown( transform.GetComponent<Player_Controls>().Get_A( playerNum ) ) )
				PlayerReady[playerNum-1] = true;
			
			// Hit B to log in
			if ( Input.GetKeyDown( transform.GetComponent<Player_Controls>().Get_B( playerNum ) ) )
			{
				if (!PlayerReady[playerNum-1])
					PlayersActive[playerNum-1] = false;
				else
					PlayerReady[playerNum-1] = false;
			}
			
		    playerNum++;
		}	
		
	}
	
	void Update()
	{
		if (MenuActive)
		{
			CheckReady();
			DetectPlayers();
		}
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
			StartGame();
		else
		{
			TransparentLayer.active = false;
			MenuActive = true;
		}
	}
	
	void StartGame()
	{
		int  Players = 0;
		int PlayerNum = 1;
		string PlayerName = "Player_";
		for (int i=0; i < 4; i++)
		{
			if (PlayersActive[i])
			{
				PlayerName = "Player_" + PlayerNum.ToString();
				PlayerPrefs.SetInt(PlayerName, (i+1) );
				Players++;	
				PlayerNum++;
			}
		}
		
		PlayerPrefs.SetInt("NumberOfPlayers", Players );
		Application.LoadLevel( LevelToLoad.ToString() );
}
	
	void CheckReady ()
	{
		int PlayersLogged = 0;
		int PlayersReady = 0;
		for (int i=0; i < 4; i++)
		{
			if (PlayersActive[i])
				PlayersLogged++;	
		}
		for (int i=0; i < 4; i++)
		{
			if (PlayerReady[i])
				PlayersReady++;	
		}
		
		if (PlayersLogged > 0)
		{
			if ( PlayersLogged == PlayersReady)
				Timer();
			else
			{
				displayCounter = false;
				countdownTimer = 3;
			}
		}
	}
	
	void Timer()
	{
		displayCounter = true;
		if (countdownTimer > 0)
			countdownTimer -= Time.deltaTime;
		else
		{
			displayCounter = false;
			MenuActive = false;
			StartCoroutine(Fade(false));
		}
		
	}
	
	void OnGUI()
	{
		// If selected start fading out so dont display Debug
		if (MenuActive)
		{
			GUI.Label(new Rect( Screen.width/2 + 190, Screen.height/2, 150, 25), "Players Active");
			
			for (int i = 0; i < 4; i++)
			{
				if (PlayersActive[i])
				{
					if (PlayerReady[i])
						GUI.color = Color.green;
					else
						GUI.color = Color.white;
					
					GUI.Label(new Rect( Screen.width/2 + 200, Screen.height/2 + ((i+1) * 20), 150, 25), "Player " + (i+1));
				}
			}
		}
		
		if (displayCounter)
			CountDownTimer.text = Mathf.CeilToInt(countdownTimer).ToString();
		else
			CountDownTimer.text = "";
	}
}

