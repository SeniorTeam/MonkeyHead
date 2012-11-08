using UnityEngine;
using System.Collections;

public class HudInventory : MonoBehaviour 
{
	[SerializeField] string InventoryAnimationName;
	[SerializeField] string AnimationUpName;
	[SerializeField] string AnimationDownName;
	[SerializeField] GameObject WeaponOne;
	[SerializeField] GameObject WeaponTwo;
	[SerializeField] Material DefaultWeponMaterial;
	
	
	bool isHudActive;
	bool typeSwap;
	
	
	void Awake()
	{
		isHudActive = false;
		typeSwap = false;
		WeaponOne.renderer.material = DefaultWeponMaterial;
		WeaponTwo.renderer.material = DefaultWeponMaterial;
	}
	
	public void DisplayHud()
	{
		if (!isHudActive)
		{
			isHudActive = true;
			//animation.Stop(InventoryAnimationName);
			animation[InventoryAnimationName].time = 0;
			animation[InventoryAnimationName].speed = 1;
			animation.Play(InventoryAnimationName);
		}
		else
		{
			isHudActive = false;
			//animation.Stop(InventoryAnimationName);
			animation[InventoryAnimationName].time = animation[InventoryAnimationName].length;
			animation[InventoryAnimationName].speed = -1;
			animation.Play(InventoryAnimationName);
		}
	}
	
	public void WeaponAdd(Material mat, int count)
	{		
		
		
		if (!typeSwap)
		{
			if (count > 0)
			{
				SwapWeapons();
				WeaponTwo.renderer.material = mat;
			}
			else
				WeaponOne.renderer.material = mat;	
		}
		else
		{
			if (count > 0)
			{
				SwapWeapons();
				WeaponOne.renderer.material = mat;
			}
			else
				WeaponTwo.renderer.material = mat;	
		}
	}
	
	public void WeaponDrop()
	{		
		
		// If typeSwap is false that means weapon One is the top plane and
		// weapon two is the bottom one in the hud inventory
		if (!typeSwap)
		{
			typeSwap = true;
			WeaponOne.animation.Play(AnimationDownName);
			WeaponTwo.animation.Play(AnimationUpName);
			WeaponOne.renderer.material = DefaultWeponMaterial;	
		}
		else
		{
			typeSwap = false;
			WeaponOne.animation.Play(AnimationUpName);
			WeaponTwo.animation.Play(AnimationDownName);
			WeaponTwo.renderer.material = DefaultWeponMaterial;
		}
		
	}
	
	public void SwapWeapons()
	{
		if (!typeSwap)
		{
			typeSwap = true;
			WeaponOne.animation.Play(AnimationDownName);
			WeaponTwo.animation.Play(AnimationUpName);
		}
		else
		{
			typeSwap = false;
			WeaponOne.animation.Play(AnimationUpName);
			WeaponTwo.animation.Play(AnimationDownName);
		}
	}
}
