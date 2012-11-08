using UnityEngine;
using System.Collections;

public class Gun : Weapon 
{
	float ReloadTime = 2.0f;
	
	Transform[] children;
	GameObject _Gun;
	
	Vector3 BulletSpawnPoint;
	
	bool CanShoot = true;
	bool shooting = false;
	float sprayCount;
	bool HUDOn = true;

	public override void StartAttack ()
	{
		PlayerShooting();
		base.StartAttack ();
	}
	
	public override void EndAttack ()
	{
		
		base.EndAttack ();
	}
	
	#region Activate Deactivate Weapon
	public void DeactivateWeapon()
	{
		//Debug.Log(Name + "  is Deactive");
		HUDOn = false;
	}
	
	public void ActivateWeapon()
	{
		HUDOn = true;
	}
	#endregion
	
	#region Controls
	void PlayerShooting()
	{
		if (CanShoot)
		{
			if (_Ammo > 0)
			{
				if (!shooting)
				{
					//Debug.Log("BANG");
					shooting = true;
					_Ammo--;
					sprayCount = _Spray;
					CreateBullet();
					StartCoroutine(FireRateTime());
				}
			}
			else
			{
				Debug.Log("RELOAD FIRST");
			}
		}
	}
	
	IEnumerator FireRateTime()
	{
		float time = _FireRate;
		
		while(time > 0)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		shooting = false;
	}
	
	void CreateBullet()
	{
		//RandomAngle(sprayCount);
		
		children = gameObject.GetComponentsInChildren<Transform>();
		foreach ( Transform child in children)
		{
			if (child.name == "BulletSpawn")
				BulletSpawnPoint = child.transform.position;
		}
		
		GameObject _bullet = Instantiate(Resources.Load("Prefabs/Weapons/Bullets/Bullet"), BulletSpawnPoint, Quaternion.Euler(Vector3.forward) ) as GameObject;
		
		
		_bullet.GetComponent<Bullet>().StartPosition = BulletSpawnPoint;
		_bullet.GetComponent<Bullet>().Damage = _Damage;
		_bullet.GetComponent<Bullet>().Range = _Range;
		_bullet.rigidbody.AddForce(transform.forward * 2000);
		
		
		if (sprayCount > 1)
		{
			sprayCount--;
			CreateBullet();
		}
	}
	
	/*
	void PlayerReload()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			CanShoot = false;
			StartCoroutine(Reload());
		}
	}
	
	IEnumerator Reload()
	{
		Debug.Log("RELOADING-> ");
		float time = ReloadTime;
		while (time > 0)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		
		CanShoot = true;
		Debug.Log("RELOADING-> COMPLETE");
		CurrentAmmo = _Ammo;
		
	}
	*/
	#endregion
	
	
	#region HUD
	void OnGUI()
	{
		if (!Active) return;
		
		if (!HUDOn) return;
			GUI.Label(new Rect(10,120,300,20), "Ammo: " + _Ammo);
	}
	#endregion
	
}
