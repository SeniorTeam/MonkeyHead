using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy_Sight : MonoBehaviour 
{
	[SerializeField] Game c_Game;
	[SerializeField] GameObject Head;
	[SerializeField] AnimationClip[] HeadAnimation;
	
	bool canAttack = true;
	
	List<Player_Info> ListOfPlayers = new List<Player_Info>();
	
	public GameObject TargetPlayer;
	
	float sightDistance = 10;
	bool canHeadSearch = true;
	
	
	#region Initializer
	void Awake()
	{
		NodeA.renderer.enabled = false;
		NodeB.renderer.enabled = false;
	}
	
	public void Initialize()
	{
		StartCoroutine(Init());
	}
	
	IEnumerator Init()
	{
		yield return new WaitForSeconds(0.1f);	
		
		//SetVariables();
		//DebugTest();
		
		while (!c_Game.StartGame)
		{
			yield return null;	
		}
		StartCoroutine(CanHeadSearch());
		//StartCoroutine(CatchPlayer());	
	}
	#endregion
	
	public void UpdateFunction()
	{
		EnemyCheck();
		CheckPlayer();
	}
		
	// Kill Player / Damage Player
	public void CheckPlayer()
	{
		if (TargetPlayer != null)
		{
			//Debug.Log("Player != null");
			Vector3 pos = TargetPlayer.transform.position;
			
			if ( isPlayerInRange(pos.y, DistanceForm(pos.x, pos.z), 4.0f) )
			{
				if (canAttack)
					StartCoroutine( EnemyAttack() );
			}
		}
	}
	
	// Enemy is Attacking
	IEnumerator EnemyAttack()
	{
		canAttack = false;
		
		TargetPlayer.GetComponent<Player_Info>().ApplyDamage(40);
		
		// Timer delay so enemy wont keep applying damage, 1 second delay for default
		float time = 1.0f;
		while (time > 0)
		{
			time -= Time.deltaTime;	
			yield return null;
		}
		canAttack = true;
	}
	
	
	void EnemyDetectionOfPlayer(GameObject player)
	{
		if (isPlayerInSight(player))
		{
			DetermineHeadLookAt(true);
			Vector3 forward = Head.transform.TransformDirection(Vector3.forward) * sightDistance;
			Debug.DrawRay (Head.transform.position, forward, Color.red);
		}
		else
		{
			DetermineHeadLookAt(false);	
		}
	}
	
	// Where is head looking at
	void DetermineHeadLookAt(bool canLook)
	{
		// If player found
		if (canLook)
		{
			// Stop searching animation 
			canHeadSearch = false;
			// Lookat at player
			Head.transform.LookAt(TargetPlayer.transform.position);
		}
		else
		{
			// Start searching animation
			canHeadSearch = true;
			// Head does not look at player
			Head.transform.LookAt(null);
			// current target to follow is set to null
			TargetPlayer = null;
		}
	}
	
	// if player found enemy can move towards player
	public bool CanEnemyMove()
	{
		if (canHeadSearch)
			return false;
		else
			return true;
	}
	
	bool isPlayerInRange(float y, float xz, float distance)
	{
		bool inRange = true;
		
		if ( y > (Mathf.Abs(transform.position.y) + distance) )
		{
			//Debug.Log("Y: " + y + "   D: " + (Mathf.Abs(transform.position.y) + distance));
			inRange = false;
		}
		if (xz > distance)
		{
			//Debug.Log("XZ: " + xz + "   D: " + distance);
			inRange = false;
		}
					
		return inRange;					
	}
				
	private float DistanceForm (float player_x, float player_z)
	{
		float x1 = player_x;
		float x2 = transform.position.x;
		float y1 = player_z;
		float y2 = transform.position.z;
		
		float distance;
		
		float xFactor = Mathf.Pow( (x2 - x1), 2);
		float yFactor = Mathf.Pow( (y2 - y1), 2);
		
		distance = Mathf.Sqrt( (xFactor + yFactor) );
		distance = Mathf.Abs(distance);
		
		return distance;
	}
	
	IEnumerator CanHeadSearch()
	{
		if (canHeadSearch)
		{
			float TimeWait = Random.Range(3.0f, 4.0f);
			int rand = Random.Range (0, HeadAnimation.Length);
			float time = HeadAnimation[rand].length;
			Head.animation.clip = HeadAnimation[rand];
			
			while (TimeWait > 0)
			{
				if (!canHeadSearch)
				{
					yield return null;
					break;
				}
				
				TimeWait -= Time.deltaTime;
				yield return null;
			}
			
			if (canHeadSearch)
			{
				Head.animation.Play();
				
				while (time > 0)
				{	
					time -= Time.deltaTime;
					
					if (!canHeadSearch)
					{
						Head.animation.Stop();	
						break;
					}
					yield return null;
				}
			}
		}
		else
		{
			Head.animation.Stop();	
		}
	
		yield return null;	
		StartCoroutine(CanHeadSearch());
		yield return null;	
		
	}
	
	const float EnemyRangeDistance = 20;
	void CheckPlayersDistances()
	{
		ListOfPlayers.Clear();
		Player_Info[] playersOnMap = FindObjectsOfType(typeof(Player_Info)) as Player_Info[];
		foreach (Player_Info p in playersOnMap)
		{
			Vector3 pos = p.transform.position;
			
			if ( isPlayerInRange(pos.y, DistanceForm(pos.x, pos.z), EnemyRangeDistance) )
				ListOfPlayers.Add(p);
		}
		
		foreach (Player_Info player in ListOfPlayers)
		{
			EnemyDetectionOfPlayer(player.transform.gameObject);
		}
		
	}
	
	bool canCheck = true;
	void EnemyCheck()
	{
		if (canCheck)
			StartCoroutine( SearchForPlayerTimer() );
	}
	
	float timer = 1;
	IEnumerator SearchForPlayerTimer()
	{
		canCheck = false;
		
		while (timer > 0)
		{
			timer -= Time.deltaTime;
			yield return null;
		}
		
		CheckPlayersDistances();
		canCheck = true;
	}
	

	#region Barycentric Technique
	// Barycentric Technique
	// P = A + u * (C - A) + v * (B - A)
	Vector3 v0, v1, v2;
	Vector3 P, A, B, C;
	float dot00, dot01, dot02, dot11, dot12;
	[SerializeField] GameObject NodeA;
	[SerializeField] GameObject NodeB;
	float   height;
	bool isPlayerInSight(GameObject player)
	{
		bool inArea = false;
		
		// Set Points
		P = player.transform.position;
		A = NodeA.transform.position;
		B = NodeB.transform.position; 
		C = Head.transform.position; 
		
		Debug.DrawLine(C, A, Color.green);
		Debug.DrawLine(C, B, Color.green);
		Debug.DrawLine(A, B, Color.green);
		
		// Compute vectors
		v0 = C - A;
		v1 = B - A;
		v2 = P - A;
		
		// Compute dot products
		dot00 = Vector3.Dot(v0, v0);
		dot01 = Vector3.Dot(v0, v1);
		dot02 = Vector3.Dot(v0, v2);
		dot11 = Vector3.Dot(v1, v1);
		dot12 = Vector3.Dot(v1, v2);
		
		// Compute barycentric coordinates
		float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
		float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
		float v = (dot00 * dot12 - dot01 * dot02) * invDenom;
		
		// Check if point is in triangle
		if ( (u >= 0) && (v >= 0) && (u + v < 1) )
		{
			TargetPlayer = player;
			inArea = true;
		}
		
		return inArea;
	}
	#endregion
	
	
}
