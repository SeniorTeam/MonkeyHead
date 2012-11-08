using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{	
	#region Declaration
	[SerializeField] GameObject ePrefab;
	GameObject Player;
	
	float PositionStop = -49;		//TEST FOR NOW  USE ASTAR AFTER
	
	// Imported Modifiers
	public Vector3 Position{get;set;}
	public string Name{get;set;}
	public float Health{get;set;}
	public float Speed{get;set;}
	public float Attack{get;set;}
	public float Size{get;set;}
	
	// Actual Stats
	Vector3 EnemyPosition;
	
	bool CanMove = true;
	#endregion
	
	#region Initializer;
	void Awake()
	{
		Player = GameObject.Find("Player");
		StartCoroutine(Initialize());
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
		Vector3 scale = transform.localScale;
		transform.localScale = new Vector3(scale.x * Size, scale.y * Size, scale.z * Size);
		EnemyPosition = transform.position;
	}
	void DebugTest()
	{
		Debug.Log("Name: " + Name);
		Debug.Log("Health:" + Health);
		Debug.Log("Speed:" + Speed);
		Debug.Log("Attack:" + Attack);
		Debug.Log("Size:" + Size);
	}
	#endregion
	
	
	void Update()
	{
		MoveEnemy();
		if (Input.GetKeyDown(KeyCode.Z))
		{
			DestroyEnemy();
		}
	}
	
	void MoveEnemy()
	{
		if (CanMove)
		{
			if (EnemyPosition.x > PositionStop)
			{
				EnemyPosition.x -= Speed;
				transform.position = EnemyPosition;
			}
			else
			{
				CanMove = false;
				InvokeRepeating("AttackTower", 1, 1);	
			}
		}
	}
	
	void AttackTower()
	{
		//Debug.Log("ATTACKING");
		Player.GetComponent<Player>().AttackTower(Attack);
	}
	
	public void DestroyEnemy()
	{
		CancelInvoke("AttackTower");
		Destroy(gameObject);
	}
	
}
