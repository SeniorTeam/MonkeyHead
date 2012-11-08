using UnityEngine;
using System.Collections;

public class Boulder : MonoBehaviour 
{
	[SerializeField] GameObject WallDestroy;
	[SerializeField] GameObject MiniBoulders;
	[SerializeField] GameObject BoulderExplosion;
	[SerializeField] GameObject SpeedZone;
	
	bool explodeOnce;
	
	public void InitiateVelocity()
	{
	}
	
	void OnTriggerEnter(Collider hit)
	{
		if (hit.name == WallDestroy.name)
		{
			hit.GetComponent<BoulderWall>().DestroyWall();
			
			if (!explodeOnce)
				StartCoroutine( DestroyBoulder() );
		}
		
		if (hit.tag == "Player")
		{
			hit.GetComponent<Player_Info>().PlayerDied();
		}
	}
	
	void OnTriggerStay(Collider hit)
	{
		if (hit.name == SpeedZone.name)
			rigidbody.AddForce(-20.0f,0, 0);
	}
	
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.D))
		{
			StartCoroutine( DestroyBoulder() );
		}
	}
	
	IEnumerator DestroyBoulder()
	{
		explodeOnce = true;
		
		Instantiate(BoulderExplosion, WallDestroy.transform.position, new Quaternion(1,1,1,1));
		//GameObject SmallBoulders = Instantiate(MiniBoulders, transform.position, Quaternion.identity) as GameObject;
		
		StartCoroutine(DestroyObject(gameObject) );
		
//		foreach (Transform smallBoulder in SmallBoulders.transform)
//		{
//			smallBoulder.rigidbody.AddExplosionForce(500, transform.position, 10);
//			StartCoroutine( TurnCollidersOn(smallBoulder) );
//			StartCoroutine (DistortPiece(smallBoulder) );
//			
//			yield return null;
//		}
		
		yield return null;
	}
	
	IEnumerator DistortPiece(Transform stone)
	{
		Vector3 scale = stone.localScale;
		float sx = Random.Range (scale.x/20, scale.x/5);
		float sy = Random.Range (scale.y/20, scale.y/5);
		float sz = Random.Range (scale.z/20, scale.z/5);
		Vector3 scalar;
		
		
		Vector3 rot = stone.localScale;
		float rx = Random.Range (0, 360);
		float ry = Random.Range (0, 360);
		float rz = Random.Range (0, 360);
		Vector3 rotation;
		
		float totalTime = 0.75f;
		
		float time = 0;
		while (true)
		{
			if ( time >= totalTime)
			{
				scalar.x = sx;
				scalar.y = sy;
				scalar.z = sz;
				stone.localScale = new Vector3 (scalar.x, scalar.y, scalar.z);
				
				rotation.x = rx;
				rotation.y = ry;
				rotation.z = rz;
				stone.eulerAngles = new Vector3 (rotation.x, rotation.y, rotation.z);
		
				break;
			}
			else
				time += Time.deltaTime * (1/totalTime);
			
			// Scale
			scalar.x = Mathf.Lerp(scale.x, sx, time);
			scalar.y = Mathf.Lerp(scale.y, sy, time);
			scalar.z = Mathf.Lerp(scale.z, sz, time);
			
			// Rotation
			rotation.x = Mathf.Lerp(rot.x, rx, time);
			rotation.y = Mathf.Lerp(rot.y, ry, time);
			rotation.z = Mathf.Lerp(rot.z, rz, time);
			
			stone.localScale = new Vector3(scalar.x, scalar.y, scalar.z);
			stone.eulerAngles = new Vector3(rotation.x, rotation.y, rotation.z);
			
			yield return null;
		}
		
		Destroy(stone.gameObject);
	}
	
	IEnumerator DestroyObject (GameObject currentObject) 
	{
		Color color = currentObject.renderer.material.color;
		float alpha = color.a;
		float totalTime = 0.75f;
		
		float time = 0;
		while (true)
		{
			if ( time >= totalTime)
			{
				alpha = 0;
				currentObject.renderer.material.color = new Color (color.r, color.g, color.b, alpha);
				Destroy(currentObject);
				break;
			}
			else
				time += Time.deltaTime * (1/totalTime);
			
			alpha = Mathf.Lerp(1, 0, time);
			
			currentObject.renderer.material.color = new Color (color.r, color.g, color.b, alpha);
			
			yield return null;
		}
	}
	
	IEnumerator TurnCollidersOn(Transform stone)
	{
		yield return new WaitForSeconds(0.2f);
		stone.collider.enabled = true;
	}
	
}
