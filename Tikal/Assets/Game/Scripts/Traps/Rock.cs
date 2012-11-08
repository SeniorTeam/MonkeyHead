using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour 
{
	
	bool RocksActive = true;
	Vector3 pos;
	float timer = 3;
	
	void Awake()
	{
		pos = transform.position;
		float rand1 = Random.Range(-3.0f, 3.0f);
		float rand2 = Random.Range(0.0f, 2.0f);
		
		transform.position = new Vector3((pos.x + rand1), pos.y, (pos.z + rand2));
		StartCoroutine(CoolDown());
	}
	
	void OnTriggerEnter(Collider hit)
	{
		if (hit.tag == "Player")
		{
			if (RocksActive)
				hit.GetComponent<Player_Info>().PlayerDied();
		}
	}


	IEnumerator CoolDown ()
	{
		while (timer > 0)
		{
			timer -= Time.deltaTime;
			transform.rigidbody.AddForce( new Vector3( Random.Range( 4.0f, 8.0f ), Random.Range( -2.0f, -0.1f ), Random.Range( 2.0f, 3.0f )) );
			yield return null;
		}
		RocksActive = false;
		yield return null;
	}
}
