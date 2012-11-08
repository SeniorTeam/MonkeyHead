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
	[SerializeField] GameObject Spawn;
	[SerializeField] GameObject Node;
	[SerializeField] GameObject Target;
	[SerializeField] GameObject ePrefab;
	[SerializeField] GameObject Head;
	[SerializeField] AnimationClip[] HeadAnimation;
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
	bool canHeadSearch = false;
	#endregion
	
	#region Initializer;
	void Awake()
	{
		Player = GameObject.Find("Player");
		//StartCoroutine(Initialize());
		StartCoroutine(CanHeadSearch());
	}
	IEnumerator Initialize()
	{
		yield return new WaitForSeconds(0.1f);	
		
		SetVariables();
		//DebugTest();
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
		EnemySight();
		
	}
	
	void EnemySight()
	{
		float sightDistance = 4;
		Vector3 forward = Head.transform.TransformDirection(Vector3.forward) * sightDistance;
		Ray ray = new Ray(Head.transform.position, forward);
		RaycastHit hit;
		CharacterMotor found = null;
		
		if ( Physics.Raycast(ray, out hit, sightDistance) )
		{
		   found = hit.collider.GetComponent<CharacterMotor>();
			
			// If hit target
			if (found != null)
			{
				Debug.Log("Spotted: " + hit.transform.name);	
			}
		}
		
		// Debug enemies eyesight
		Debug.DrawRay (Head.transform.position, forward, Color.green);
		
	}
	
	IEnumerator CanHeadSearch()
	{
		float TimeWait = Random.Range(3.0f, 10.0f);
		
		while (TimeWait > 0)
		{
			TimeWait -= Time.deltaTime;
			yield return null;
		}
		canHeadSearch = true;
		
		//HeadAnimation.
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
