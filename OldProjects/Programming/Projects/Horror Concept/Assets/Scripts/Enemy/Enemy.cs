using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{	
	
	public enum EnemyAI
	{
		Pace = 0,
		Guard,
		Search,
		Hunt
	}
	
	public EnemyAI ai;
	
	#region Declaration
	[SerializeField] Game c_Game;
	
	[SerializeField] GameObject Spawn;
	[SerializeField] GameObject Node;
	[SerializeField] GameObject Target;
	[SerializeField] GameObject ePrefab;
	[SerializeField] GameObject Head;
	[SerializeField] AnimationClip[] HeadAnimation;
	[SerializeField] float sightDistance = 5;
	public bool PlayerCaught;
	GameObject Player;
	
	// Imported Modifiers
	public Vector3 Position{get;set;}
	public string Name{get;set;}
	public float Health{get;set;}
	public float Speed{get;set;}
	public float Attack{get;set;}
	
	// Actual Stats
	Vector3 EnemyPosition;
	
	bool CanMove = true;
	bool canHeadSearch = true;
	#endregion
	
	#region Initializer;
	void Awake()
	{
		PlayerCaught = false;
		Player = GameObject.Find("Player");
		StartCoroutine(Initialize());
	}
	
	IEnumerator Initialize()
	{
		yield return new WaitForSeconds(0.1f);	
		
		//SetVariables();
		//DebugTest();
		
		while (!c_Game.StartGame)
		{
			yield return null;	
		}
		StartCoroutine(CanHeadSearch());
		StartCoroutine(CatchPlayer());
	}
	
	void SetVariables()
	{
		transform.position = Position;
		EnemyPosition = transform.position;
	}
	void DebugTest()
	{
		Debug.Log("Name: " + Name);
		Debug.Log("Health:" + Health);
		Debug.Log("Speed:" + Speed);
		Debug.Log("Attack:" + Attack);
	}
	#endregion
	
	
	void Update()
	{
        if (c_Game.StartGame)
		{
			if (!PlayerCaught)
				HeadSearching();
		}
		
	}

	IEnumerator CatchPlayer ()
	{
		Vector3 pos = Player.transform.position;
		if ( isPlayerInRange(pos.y, DistanceForm(pos.x, pos.z), 0.8f) )
		{
			Debug.Log ("PlayerDied");
			PlayerCaught = true;
			
			yield return new WaitForSeconds(0.9f);
		}
		yield return null;
		
		if (!PlayerCaught)
			StartCoroutine(CatchPlayer());
		else
		{
			Application.LoadLevel("Horror Concept");	
		}
		yield return null;
	}

	void HeadSearching()
	{
		Vector3 forward = Head.transform.TransformDirection(Vector3.forward) * sightDistance;
		Ray ray = new Ray(Head.transform.position, forward);
		RaycastHit hit;
		CharacterController found = null;
		Transform f = null;
		CharacterMotor a = null;
		
		if ( Physics.Raycast(ray, out hit, sightDistance) )
		{
		    found = hit.collider.GetComponent<CharacterController>();
		
			// gridgraph.cpp when init add x and y
			
			// If hit target
			if (found != null && !PlayerCaught)
			{
				Vector3 pos = found.transform.position;
				
				if ( isPlayerInRange(pos.y, DistanceForm(pos.x, pos.z), sightDistance) )
				{
					canHeadSearch = false;
					Debug.Log (hit.transform.name + " is being Followed");
				}
				else
				{
					canHeadSearch = true;
					Debug.Log (transform.name + " lost its target");
				}
			
			}
			else
			{
				canHeadSearch = true;
			}
		}
		
		if (!canHeadSearch)
		{
			Head.transform.LookAt(Player.transform);
			GetComponent<NavMeshAgent>().destination = Player.transform.position;
		}
		else
		{
			Head.transform.LookAt(null);
			GetComponent<NavMeshAgent>().destination = transform.position;
		}
		
		// Debug enemies eyesight
		Debug.DrawRay (Head.transform.position, forward, Color.green);
		
	}
	
	bool isPlayerInRange(float y, float xz, float distance)
	{
		bool inRange = true;
		
		if ( y > (Mathf.Abs(transform.position.y) + distance) )
			inRange = false;
		if (xz > distance)
			inRange = false;
					
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
		if (canHeadSearch && !PlayerCaught)
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
					yield return null;
					
					if (!canHeadSearch)
					{
						Head.animation.Stop();	
						yield return null;
						break;
					}
				}
			}
		}
	
		yield return null;	
		StartCoroutine(CanHeadSearch());
		yield return null;	
		
	}
	
	
	
	#region AI Types
	float lerp_x, lerp_y, lerp_z;
	IEnumerator AI_Pace(string location, float time)
	{
		float t = 0;
		if (location == "spawn")
		{
			while (EnemyPosition.x != Node.transform.position.x)
			{
				if (t < time)
				{
					t += Time.deltaTime;
					lerp_x = Mathf.Lerp(Spawn.transform.position.x, Node.transform.position.x, t);
					lerp_y = Mathf.Lerp(Spawn.transform.position.y, Node.transform.position.y, t);
					lerp_z = Mathf.Lerp(Spawn.transform.position.z, Node.transform.position.z, t);
					
					transform.position = new Vector3 (lerp_x, lerp_y, lerp_z);
				}
				else
				{
					StartCoroutine(AI_Pace("node", time));
					break;
				}
			}
			
		}
		else if (location == "node")
		{
			while (EnemyPosition.x != Spawn.transform.position.x)
			{
				if (t < time)
				{
					t += Time.deltaTime;
					lerp_x = Mathf.Lerp(Node.transform.position.x, Spawn.transform.position.x, t);
					lerp_y = Mathf.Lerp(Node.transform.position.y, Spawn.transform.position.y, t);
					lerp_z = Mathf.Lerp(Node.transform.position.z, Spawn.transform.position.z, t);
					
					transform.position = new Vector3 (lerp_x, lerp_y, lerp_z);
				}
				else
				{
					StartCoroutine(AI_Pace("spawn", time));
					break;
				}
			}
		}
		
		yield return null;	
	}
	
	IEnumerator AI_Guard()
	{
		
		
		yield return null;	
	}
	
	IEnumerator AI_Search()
	{
		yield return null;	
	}
	
	IEnumerator AI_Hunt()
	{
		yield return null;	
	}
	#endregion
	
}
