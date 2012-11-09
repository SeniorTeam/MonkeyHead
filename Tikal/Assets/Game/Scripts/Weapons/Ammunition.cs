using UnityEngine;
using System.Collections;

public class Ammunition : MonoBehaviour 
{
//	[SerializeField] Texture2D BulletFull;
//	[SerializeField] Texture2D BulletEmpty;
//	
//	public bool BulletFired;
//	private float Ammo;
//	private float ClipSize;
//	
//	public void InitializeHudAmmo(float ammo, float clipSize)
//	{
//		Ammo = ammo;
//		ClipSize = clipSize;
//		StartCoroutine(Hud_Ammunition());
//	}
//	
//	IEnumerator Hud_Ammunition()
//	{
//		
//		float amount = ClipSize;
//		while (true)
//		{
//			if (amount > 0)
//			{
//				Ammunition bullet = Instantiate(Resources.Load( "Prefabs/Hud/bullet"), new Vector3(0,0,0), Quaternion.identity) as Ammunition;
//				bullet.transform.parent = transform.FindChild("Ammunition").transform;
//				bullet.BulletFired = false;
//				bullet.guiTexture.texture = BulletFull;
//				transform.GetComponent<Weapon>().HudBullets.Add(bullet.guiTexture);
//				amount--;
//			}
//			else
//				break;
//			
//			yield return null;
//		}
//		
//		int widthInc = 10;
//		int sW = Screen.width;
//		int sH = Screen.height;
//		
//		foreach (Ammunition bullet in transform.GetComponent<Weapon>().HudBullets)
//		{
//			//bullet.guiTexture.pixelInset.x = 
//		}
//	}
}
