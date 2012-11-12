using UnityEngine;
using System.Collections;

public class RadarBlipGenerator : MonoBehaviour 
{
	[SerializeField] float BlipTimerOnRadar;
	
	[SerializeField] GameObject EnemyBlip;
	[SerializeField] GameObject AllyBlip;
	
	GameObject PlayerWhoShot;
	bool[] PlayerShot = new bool[4];
	
	void Awake()
	{
		PlayerShot[0] = false;
		PlayerShot[1] = false;	
		PlayerShot[2] = false;	
		PlayerShot[3] = false;	
	}

	public void CreateBlips( int PlayerNumber, string PlayerName )
	{
		StartCoroutine( CreateEnemyBlip(PlayerNumber, PlayerName) );
	}
	
	IEnumerator CreateEnemyBlip(int PlayerNumber, string PlayerName)
	{
		// If player did not shoot continue to create blip.
		// If the player did shoot in the past wait until the
		// cooldown timer is up
		if (!PlayerShot[PlayerNumber-1])
		{
			PlayerWhoShot = GameObject.Find(PlayerName);
			
			// Time to find player, if greater player is dead or does not exist
			float timeToFindPlayer = 1;
			while (timeToFindPlayer > 0)
			{
				if (PlayerWhoShot == null)
					PlayerWhoShot = GameObject.Find(PlayerName);
				else
					break;
				
				timeToFindPlayer -= Time.deltaTime;
				yield return null;
			}
			
			// If timer ran out to find player, either they do not exist or they are dead
			// skip entire coroutine
			if (timeToFindPlayer > 0 )
			{
				PlayerShot[PlayerNumber-1] = true;
				Vector3 pos = PlayerWhoShot.transform.position;
				GameObject enemyBlip = Instantiate( EnemyBlip, new Vector3 (pos.x, pos.y, pos.z), Quaternion.identity) as GameObject;
				
				#region Blip Layers
				// Layer 18 is Player 1 Enemy Blip
				// Layer 19 is Player 2 Enemy Blip
				// Layer 20 is Player 3 Enemy Blip
				// Layer 21 is Player 4 Enemy Blip
				#endregion
				
				if (PlayerNumber == 1)
					enemyBlip.layer = 18;
				else if (PlayerNumber == 2)
					enemyBlip.layer = 19;
				else if (PlayerNumber == 3)
					enemyBlip.layer = 20;
				else if (PlayerNumber == 4)
					enemyBlip.layer = 21;
				
				float time = BlipTimerOnRadar;
				while (time > 0)
				{
					time -= Time.deltaTime;
					yield return null;
				}
				
				Destroy(enemyBlip);
				PlayerShot[PlayerNumber-1] = false;
				yield return null;
			}
		}
	}
}
