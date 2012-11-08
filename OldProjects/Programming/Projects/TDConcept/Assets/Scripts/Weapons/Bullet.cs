using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	public Vector3 StartPosition{get; set;}
	public float Damage {get; set;}
	public float Range {get; set;}
	public int Angle {get;set;}
	public Vector2 SinCos {get; set;}
	public int Direction {get; set;}
	
	public float BulletSpeed = 1.0f;
	public float deathTime;
	
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
		transform.eulerAngles = new Vector3 (0, Angle, 0);

		DeathTime();
		canMove = true;
		StartCoroutine(StartBulletDeathTimer(deathTime));
	}
	
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
	
	IEnumerator StartBulletDeathTimer(float time)
	{
		while (time > 0)
		{
			time -= Time.deltaTime;
			yield return null;	
		}
		DestroyBullet();
	}
	
	void Update()
	{
		if (canMove)
		{
			MoveBullet();	
		}
	}
	
	void MoveBullet()
	{
		Vector3 pos = transform.position;
		
		if (Direction > 0)
		{
			pos.x += bSpeed.x;
			//pos.z += bSpeed.y;
		//	Debug.Log(pos.x);
		}
		
		transform.position = pos;
	}
	
	void DestroyBullet()
	{
		Destroy(gameObject);	
	}
	
	void OnTriggerEnter(Collider hit)
	{
		Debug.Log(hit.gameObject.name);
	}
	
}
