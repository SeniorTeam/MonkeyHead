using UnityEngine;
using System.Collections;

public class Enemy_Sight : MonoBehaviour 
{
	[SerializeField] Game c_Game;
	[SerializeField] GameObject Head;
	[SerializeField] AnimationClip[] HeadAnimation;
	
	GameObject Player;
	
	float sightDistance = 10;
	bool canHeadSearch = true;
	public bool PlayerCaught;
	
	
	#region Initializer
	void Awake()
	{
		NodeA.renderer.enabled = false;
		NodeB.renderer.enabled = false;
	}
	
	public void Initialize()
	{
		Player = GameObject.Find("Player");
		PlayerCaught = false;
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
		if (!PlayerCaught)
		{
			//HeadSearching();
			EnemyDetectionOfPlayer();
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
			//Application.LoadLevel("Horror Concept");	
		}
		yield return null;
	}
	
	void EnemyDetectionOfPlayer()
	{
		DetermineHeadLookAt(isPlayerInSight());
		Vector3 forward = Head.transform.TransformDirection(Vector3.forward) * sightDistance;
		Debug.DrawRay (Head.transform.position, forward, Color.red);
	}

	void HeadSearching()
	{
		Vector3 forward = Head.transform.TransformDirection(Vector3.forward) * sightDistance;
		Ray ray = new Ray(Head.transform.position, forward);
		RaycastHit hit;
		CharacterMotor found = null;
		
		if ( Physics.Raycast(ray, out hit) )
		{
		    found = hit.collider.GetComponent<CharacterMotor>();
			
			//if (found != null)
			//	Debug.Log(found.gameObject.name);
			
			// If hit target
			if (found != null && !PlayerCaught)
			{
				Vector3 pos = found.transform.position;
					
				if ( isPlayerInRange(pos.y, DistanceForm(pos.x, pos.z), sightDistance) )
				{
					canHeadSearch = false;
					Debug.Log (hit.transform.name + " is being Followed");
					Head.transform.LookAt(Player.transform.position);
				}
				else
				{
					canHeadSearch = true;
					Head.transform.LookAt(null);
					//Debug.Log (transform.name + " lost its target");
				}
				
			}
			else
			{
				canHeadSearch = true;
				Head.transform.LookAt(null);
			}
	
			
			//DetermineHeadLookAt();
		}
		
		// Debug enemies eyesight
		Debug.DrawRay (Head.transform.position, forward, Color.green);
		
	}
	
	void DetermineHeadLookAt(bool canLook)
	{
		if (canLook)
		{
			canHeadSearch = false;
			Head.transform.LookAt(Player.transform.position);
		}
		else
		{
			canHeadSearch = true;
			Head.transform.LookAt(null);
		}
	}
	
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
			Debug.Log("Y: " + y + "   D: " + (Mathf.Abs(transform.position.y) + distance));
			inRange = false;
		}
		if (xz > distance)
		{
			Debug.Log("XZ: " + xz + "   D: " + distance);
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
	

	
	// Barycentric Technique
	// P = A + u * (C - A) + v * (B - A)
	Vector3 v0, v1, v2;
	Vector3 P, A, B, C;
	float dot00, dot01, dot02, dot11, dot12;
	[SerializeField] GameObject NodeA;
	[SerializeField] GameObject NodeB;
	float   height;
	bool isPlayerInSight()
	{
		// Set Points
		P = Player.transform.position;
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
		return (u >= 0) && (v >= 0) && (u + v < 1);
	}
	
	
	
}
