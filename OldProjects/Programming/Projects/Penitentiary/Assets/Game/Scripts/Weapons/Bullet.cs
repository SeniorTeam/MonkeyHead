using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	public Vector3 StartPosition{get; set;}
	public float Damage {get; set;}
	public float Range {get; set;}

	public float deathTime = 4;
	
	Vector2 bSpeed;
	
	bool canMove = false;
	
	Vector3 EndPosition;
	
	void Awake()
	{
		StartCoroutine(Initialize());
	}
	
	IEnumerator Initialize()
	{
		yield return new WaitForSeconds(0.05f);	
		SetVariables();
	}
	
	void SetVariables()
	{
		
		//DeathTime();
		canMove = true;
		StartCoroutine(StartBulletDeathTimer(deathTime));
	}
	
	/*
	void DeathTime()
	{
		// Find Position two, endpoint
		EndPosition.x = StartPosition.x + (SinCos.x * Range);
		EndPosition.z = StartPosition.z + (SinCos.y * Range);
		
		//Debug.Log("x: " + EndPosition.x);
		//Debug.Log("z: " + EndPosition.y);
		
		// Distance formula
		float dx = Mathf.Pow((EndPosition.x - StartPosition.x), 2);
		float dy = Mathf.Pow((EndPosition.z - StartPosition.z), 2);
		float distance = Mathf.Sqrt(dx + dy);
		
		// D/V = T
		deathTime = distance/(BulletSpeed * 10);
		
		//Debug.Log("T:  " + deathTime);
		bSpeed.x = 1.0f;
		//bSpeed.x = Mathf.Abs(StartPosition.x/EndPosition.x);
		//bSpeed.y = Mathf.Abs(StartPosition.z/EndPosition.z);
	}
	*/
	
	IEnumerator StartBulletDeathTimer(float time)
	{
		while (time > 0)
		{
			time -= Time.deltaTime;
			yield return null;	
		}
		DestroyBullet();
	}
	
	void DestroyBullet()
	{
		Destroy(gameObject);	
	}
	
	void OnTriggerEnter(Collider hit)
	{
		if (hit.gameObject.tag == "Floor")
		{
			Instantiate(Resources.Load("Prefabs/Weapons/Bullets/Snow_Splatter"), transform.position, Quaternion.Euler(Vector3.forward) );
			DestroyBullet();
		}
		else if (hit.gameObject.tag == "Enemy")
		{
			Destroy(hit.gameObject);
			DestroyBullet();
		}
		
		//Debug.Log(hit.gameObject.name);
	}
	
}
