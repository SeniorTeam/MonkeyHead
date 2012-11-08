using UnityEngine;
using System.Collections;
using System.Linq;

public class WeaponSlot : MonoBehaviour 
{
	
	[SerializeField] WeaponManager weapon;
	[SerializeField] GameObject[] GunSlot;
	
	public int NumberOfGuns;
	public string CurrentWeapon;
	public bool WeaponSlotsLoaded;
	
	void Awake()
	{
		StartCoroutine(LoadWeaponStats());
		
		CurrentWeapon = "Five_Seven";
	}
	
	IEnumerator LoadWeaponStats()
	{
		while (true)
		{
			if (weapon.WeaponsLoaded)break;
			
			yield return null;
		}
		
		NumberOfGuns = GunSlot.Count();
		
		for (int i=0; i < NumberOfGuns; i++)
		{
			//Debug.Log("Gun Name: " + GunSlot[i].name);
			
			if (GunSlot[i].name == weapon.wName[i])
			{
				GameObject _weapon = Instantiate(Resources.Load("Prefabs/Weapons/" + weapon.wName[i])) as GameObject;
		
				_weapon.GetComponent<Weapon>().Name = weapon.wName[i];
				_weapon.GetComponent<Weapon>().Damage = weapon.wDamage[i];
				_weapon.GetComponent<Weapon>().Ammo = weapon.wAmmo[i];
				_weapon.GetComponent<Weapon>().FireRate = weapon.wFireRate[i];
				_weapon.GetComponent<Weapon>().Range = weapon.wRange[i];
				_weapon.GetComponent<Weapon>().Spray = weapon.wSpray[i];
				_weapon.GetComponent<Weapon>().Splash = weapon.wSplash[i];

				yield return null;
			}
			yield return null;
		}
		yield return null;
		WeaponSlotsLoaded = true;
		
	}
	
	void Update()
	{
		if (weapon.WeaponsLoaded)
		{
			CheckGunSwap();	
		}
	}
	
	void CheckGunSwap()
	{
		
		// hand gun
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if( CurrentWeapon != weapon.wName[0])
			{
				StartCoroutine(GunSwap("Five_Seven"));
			}
		}
		
		// sub-machine gun
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			StartCoroutine(GunSwap("UZI"));
		}
		
		// shotgun
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			StartCoroutine(GunSwap("12_Gauge"));
		}	
	}
	
	IEnumerator GunSwap(string gun)
	{
		Weapon[] weaponType = FindObjectsOfType(typeof(Weapon)) as Weapon[];
		foreach (Weapon gunType in weaponType)
		{
			if (gun == gunType.Name)
			{
				CurrentWeapon = gunType.Name;
				gunType.ActivateWeapon();
			}
			else
			{
				gunType.DeactivateWeapon();
			}
			
			yield return null;
		}

		yield return null;
	}
	
	
}
