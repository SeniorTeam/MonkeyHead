using UnityEngine;
using System.Collections;

public class Player_Info : MonoBehaviour 
{
	Game c_Game;
	GameObject _Game;
	
	public string PlayerID;	
	public int PlayerNumber;
	public int PlayerControllerNumber;
	public string PlayerHUD;
	public string PlayerTeam;
	public bool isPlayerAlive;
	
	public bool ControllerUse;
	
	public float PlayersHealth;
	private float PlayersMaxHealth;
	
	private GameObject _Hud;
	
	[SerializeField] GameObject PlayersBody;
	[SerializeField] Camera PlayersCamera;
	private int CameraDeathHeight = 5;
	private int CameraDeathRotation = 80;
	private float DeathTime = 2;
	
	// Players color
	[HideInInspector]
	public GameObject[] Cloths;
	
	void Awake()
	{
		ControllerUse = true;
		isPlayerAlive = true;
		PlayersHealth = 100;
		PlayersMaxHealth = PlayersHealth;
		_Game = GameObject.Find("Game");
		c_Game = _Game.GetComponentInChildren<Game>();
		
		StartCoroutine( Init() );
	}
	
	IEnumerator Init()
	{
		while (!c_Game.StartGame)
			yield return null;
		
		_Hud = GameObject.Find(PlayerHUD);
	}
	
	#region Player Death
	public void PlayerDied()
	{
		isPlayerAlive = false;
		ControllerUse = false;
		gameObject.GetComponentInChildren<Inventory>().DropWeapons();
	}
	
	IEnumerator PlayerDeathScene()
	{
		Vector3 pos = PlayersCamera.transform.position;
		Vector3 rot = PlayersCamera.transform.eulerAngles;
		
		PlayersBody.animation.Play();
		
		float time = 0;
		while ( time < DeathTime)
		{
			float yPos, xRot;
			
			time += Time.deltaTime;
			
			yPos = Mathf.SmoothStep(pos.y, (pos.y + CameraDeathHeight), time);
			xRot = Mathf.SmoothStep(rot.x, CameraDeathRotation, time);
			
			PlayersCamera.transform.position = new Vector3 (pos.x, yPos, pos.z);
			PlayersCamera.transform.eulerAngles = new Vector3 (xRot, rot.y, rot.z);
				
			yield return null;
		}
		
		yield return new WaitForSeconds(1.0f);
		
		c_Game.KillPlayer(PlayerNumber, gameObject);	
	}
	
	public void KillPlayer()
	{
		StartCoroutine( PlayerDeathScene() );	
	}
	#endregion
	
	#region Players Health
	public void ApplyDamage(float damage)
	{
		if (isPlayerAlive)
		{
		
			if ( _Hud == null)
				_Hud = GameObject.Find(PlayerHUD);
			
			_Hud.GetComponentInChildren<Damage>().ApplyHit();
	
			PlayersHealth -= damage;
			
			if (PlayersHealth <= 0)
				PlayerDied();
		}
	}
	
	public void ApplyHealth(float amount)
	{
		if (isPlayerAlive)
		{
			if ( (PlayersHealth + amount) > PlayersMaxHealth )
				PlayersHealth = PlayersMaxHealth;	
			else
				PlayersHealth += amount;
		}
	}
	
	public float GetPlayersHealth()
	{
		if (PlayersHealth < 0)
			PlayersHealth = 0;
		
		return PlayersHealth;	
	}
	#endregion
	

}
