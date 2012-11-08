using UnityEngine;
using System.Collections;

public class BoulderWall : MonoBehaviour 
{
	
	public void DestroyWall()
	{
		foreach(Transform stone in transform)
		{
			stone.rigidbody.AddExplosionForce(2000, transform.position, 10);	
			StartCoroutine( DistortPiece(stone) );
		}
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			DestroyWall();	
		}
	}
	
	
	IEnumerator DistortPiece(Transform stone)
	{
		Vector3 scale = stone.localScale;
		float sx = Random.Range (scale.x/20, scale.x/3);
		float sy = Random.Range (scale.y/20, scale.y/3);
		float sz = Random.Range (scale.z/20, scale.z/3);
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
	}
}
