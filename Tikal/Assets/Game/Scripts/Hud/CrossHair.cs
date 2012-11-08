using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour 
{	
	[SerializeField] Texture2D NeutralCrossHair;
	[SerializeField] Texture2D EnemyCrossHair;
	[SerializeField] Texture2D AllyCrossHair;
	[SerializeField] Texture2D ObjectCrossHair;
	
	bool hitTarget = false;
	
	
	public void ChangeCrossHair(string type)
	{
		if ( !hitTarget )
		{
			switch (type)
			{
				case "enemy":
				{
					gameObject.guiTexture.texture = EnemyCrossHair;
					break;
				}
				case "ally":
				{
					gameObject.guiTexture.texture = AllyCrossHair;
					break;
				}
				case "object":
				{
					gameObject.guiTexture.texture = ObjectCrossHair;
					break;
				}
			}
		}
	}
	
	public void ResetCrossHair()
	{
		if ( !hitTarget )
			gameObject.guiTexture.texture = NeutralCrossHair;
	}
	
	public void HitTarget()
	{
		if (!hitTarget) 
			StartCoroutine ( HitActiveObject() );
	}
	
	IEnumerator HitActiveObject()
	{
		hitTarget = true;
		float timer = 0.75f;
		
		gameObject.guiTexture.texture = AllyCrossHair;
		
		while (timer > 0)
		{
			timer -= Time.deltaTime;
			yield return null;	
		}
		
		gameObject.guiTexture.texture = NeutralCrossHair;
		hitTarget = false;
	}
	
}
