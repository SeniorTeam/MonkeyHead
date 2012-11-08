using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour 
{	
	[SerializeField] Texture2D NeutralCrossHair;
	[SerializeField] Texture2D EnemyCrossHair;
	[SerializeField] Texture2D AllyCrossHair;
	[SerializeField] Texture2D ObjectCrossHair;
	
	
	public void ChangeCrossHair(string type)
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
	
	public void ResetCrossHair()
	{
		gameObject.guiTexture.texture = NeutralCrossHair;
	}
	
}
