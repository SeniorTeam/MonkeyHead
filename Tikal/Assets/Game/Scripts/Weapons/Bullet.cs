using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	public float deathTime = 2;
	void Awake()
	{
		StartCoroutine(StartBulletDeathTimer(deathTime));
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
	
	void DestroyBullet()
	{
		Destroy(gameObject);	
	}
	
//	void OnTriggerEnter(Collider hit)
//	{
//		if (hit.gameObject.tag == "Floor")
//		{
//			Instantiate(Resources.Load("Prefabs/Weapons/Bullets/Snow_Splatter"), transform.position, Quaternion.Euler(Vector3.forward) );
//			DestroyBullet();
//		}
//		else if (hit.gameObject.tag == "Enemy")// || hit.gameObject.tag == "Player")
//		{
//			Destroy(hit.gameObject);
//			DestroyBullet();
//		}
//		else if (hit.gameObject.tag == "Player")
//		{
//			//Debug.Log(hit.gameObject.name);
//			hit.GetComponent<Player>().PlayerDied();
//			
//			
//		}
//	}
	
}
