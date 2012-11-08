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
	
	public float PlayersHealth;
	private float PlayersMaxHealth;
	
	// Players color
	[HideInInspector]
	public GameObject[] Cloths;
	
	void Awake()
	{
		isPlayerAlive = true;
		PlayersHealth = 100;
		PlayersMaxHealth = PlayersHealth;
		_Game = GameObject.Find("Game");
		c_Game = _Game.GetComponentInChildren<Game>();
	}
	
	#region Player Death
	public void PlayerDied()
	{
		isPlayerAlive = false;
		gameObject.GetComponentInChildren<Inventory>().DropWeapons();
	}
	
	public void KillPlayer()
	{
		c_Game.KillPlayer(PlayerNumber, gameObject);	
	}
	#endregion
	
	#region Players Health
	public void ApplyDamage(float damage)
	{
		PlayersHealth -= damage;
		
		if (PlayersHealth <= 0)
			PlayerDied();
	}
	
	public void ApplyHealth(float amount)
	{
		if ( (PlayersHealth + amount) > PlayersMaxHealth )
			PlayersHealth = PlayersMaxHealth;	
		else
			PlayersHealth += amount;
	}
	
	public float GetPlayersHealth()
	{
		return PlayersHealth;	
	}
	#endregion
	

}
