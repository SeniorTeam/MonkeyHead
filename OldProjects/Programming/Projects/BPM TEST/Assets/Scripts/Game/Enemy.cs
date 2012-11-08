using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	#region Declarations
	
	#region Enemy Stats
	public Vector3 Position {get; set;}
	public string  Name     {get; set;}
	public int     Health   {get; set;}
	public int     Attack   {get; set;}
	public float   Speed    {get; set;}
	public int     Id       {get; set;}
	Vector3 EnemyPosition;
	#endregion
	
	const int HealthDead = 0;
	int HealthSubtract = 1;
	GameObject eManager; 
	#endregion
	
	#region Initialize
	void Awake()
	{	
		eManager = GameObject.Find("EnemyManager");
		StartCoroutine(Initialize());
	}
	
	IEnumerator Initialize()
	{
		yield return new WaitForSeconds(0.1f);	
		SetVariables();
	}
	
	void SetVariables()
	{
		transform.position = Position;
	}
	#endregion
	
	void Update()
	{
		EnemyPosition = transform.position;	
		
		if (Input.GetKeyDown(KeyCode.R))
		{
			ApplyDamage();
		}

		GravityOn();
		ApplySpeed();
	}

	void GravityOn ()
	{
		if (transform.position.x > 12)
		{
			rigidbody.useGravity = true;	
		}
		
		if (transform.position.y < -3)
		{
			Destroy(gameObject);
		}
	}
	
	#region Enemy Stats
	public void ApplyDamage ()
	{
		if (Health > HealthDead)
			Health -= HealthSubtract;
		else
		{
			eManager.GetComponent<EnemyManager>().EnemyList.Remove(gameObject.GetComponent<Enemy>());
			Destroy(gameObject);
		}
	}

	void ApplySpeed ()
	{
		EnemyPosition.x += Speed;
		
		transform.position = EnemyPosition;
	}
	#endregion
}
