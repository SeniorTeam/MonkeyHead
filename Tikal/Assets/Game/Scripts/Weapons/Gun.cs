using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Ammunition))]

public class Gun : Weapon 
{
	[SerializeField] GameObject BulletSpawnPoint;
	[SerializeField] GameObject Bullet;
	[SerializeField] GameObject GunShotSpark;
	[SerializeField] AudioClip  GunShotSound;
	
	float AmmunitionLeft;
	
	//Ammunition _ammunition;

	float ReloadTime = 2.0f;
	
	Transform[] children;
	GameObject _Gun;
	GameObject EyeLevel;
	GameObject Hud;
	
	bool CanShoot = true;
	bool CanReload = true;
	bool shooting = false;
	float sprayCount;
	bool HUDOn = true;
	
	const float DEATH_TIME = 30;
	float deathTimer;
	
	public override void Initialize()
	{
		//GunShotSpark.GetComponent<ParticleSystem>().Stop();
		
		base.Initialize();
		
		//_ammunition = transform.GetComponent<Ammunition>();
		//_ammunition.InitializeHudAmmo(_Ammo, _ClipSize);
		deathTimer = DEATH_TIME;
		audio.clip = GunShotSound;
		AmmunitionLeft = _ClipSize;
	}

	public override void StartAttack (GameObject eye, GameObject hud)
	{
		base.StartAttack (eye, hud);
		
		EyeLevel = eye;
		Hud = hud;
		PlayerShooting();
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
	
	void Update()
	{
		SparkUpdate();
		
		if (!Equipped)
		{
			if (deathTimer > 0)	
				deathTimer -= Time.deltaTime;
			else
				Destroy(gameObject);
		}
		else
			deathTimer = DEATH_TIME;	
	}
	
	void SparkUpdate()
	{
		GunShotSpark.transform.position = BulletSpawnPoint.transform.position;	
		GunShotSpark.transform.forward = transform.forward;
		GunShotSpark.transform.eulerAngles = transform.eulerAngles;
	}
	
	IEnumerator SparkShow()
	{
		GunShotSpark.GetComponent<ParticleSystem>().Play();
		
		float time = .2f;
		
		while(time > 0)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		
		//GunShotSpark.GetComponent<ParticleSystem>().Stop();
		yield return null;
	}
	
	#region Controls
	void PlayerShooting()
	{
		if (CanShoot)
		{
			if (_CurrentClip > 0 )
			{
				if (!shooting)
				{
					//Debug.Log("BANG");
					shooting = true;
					_CurrentClip--;
					sprayCount = _Spray;
					GunShoot();
					//audio.Play();
					audio.PlayOneShot(GunShotSound);
					//CreateBullet();
					StartCoroutine(FireRateTime());
				}
			}
			else
			{
//				if ( _Ammo == 0)
//					Debug.Log("OUT OF AMMO");
//				else
//					Debug.Log("RELOAD FIRST");
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
		
		GameObject _bullet = Instantiate(Bullet, BulletSpawnPoint.transform.position, Quaternion.Euler(Vector3.forward) ) as GameObject;
		_bullet.rigidbody.AddForce(transform.forward * 4500);
		
		if (sprayCount > 1)
		{
			sprayCount--;
			CreateBullet();
		}
	}
	
	void GunShoot()
	{
		StartCoroutine( SparkShow() );
		
		Vector3 forward = EyeLevel.transform.TransformDirection(Vector3.forward);
		Ray ray = new Ray(EyeLevel.transform.position, forward);
		RaycastHit hit;
		Transform found = null;
		
		
		if ( Physics.Raycast(ray, out hit) )
		{
		   found = hit.transform;
			
			// If hit target
			if (found != null)
			{
				#region Objects Can Shoot
				switch (found.tag)
				{
					case "Enemy":
					{
						Destroy(found.gameObject);
						break;
					}
					case "Player":
					{
						if (found.GetComponent<Player_Info>().isPlayerAlive)
						{
							Hud.GetComponentInChildren<CrossHair>().HitTarget();
							found.GetComponent<Player_Info>().ApplyDamage(_Damage);
						}
					
						break;
					}
					case "Wall":
					{
						//Vector3 coords = hit.barycentricCoordinate;
						//Instantiate(Resources.Load("Prefabs/Weapons/Bullets/Snow_Splatter"), coords, Quaternion.Euler(Vector3.forward) );
						break;
					}
					case "Untagged":
					{
						//Debug.Log(hit.barycentricCoordinate);
						break;
					}
					
				}
				
				//Debug.Log("n: " + found.name + " t: " + found.tag);
				#endregion
			}
			
		}
		// Debug Players eyesight
		//Debug.DrawRay (EyeLevel.transform.position, forward, Color.red);

	}
	
	public override void ReloadWeapon()
	{
		if (CanReload)
			StartCoroutine(Reload());

		base.ReloadWeapon();	
	}
	
	IEnumerator Reload()
	{
		CanShoot = false;
		CanReload = false;
		
		// Am I out of ammo or is the clip full if so skip if statement
		if ( _Ammo + _CurrentClip > _CurrentClip &&
			_CurrentClip != _ClipSize)
		{
		
			//Debug.Log("RELOADING-> ");
			// Reload Time
			float time = ReloadTime;
			while (time > 0)
			{
				time -= Time.deltaTime;
				yield return null;
			}
			
			while (true)
			{
				if ( _Ammo > 0 && _CurrentClip < _ClipSize)
				{
					_CurrentClip++;
					_Ammo--;
				}
				else if ( _Ammo == 0 )
				{
					//Debug.Log("OUT OF AMMO");
					break;
				}
				else if ( _CurrentClip == _ClipSize)
					break;	
				
				yield return null;
			}
			
			// Debug for ammo
			if ( (_Ammo - _ClipSize) > 0)
				AmmunitionLeft = _Ammo - _ClipSize;
			else
				AmmunitionLeft = 0;
			
			
			//Debug.Log("RELOADING-> COMPLETE");
		}
		
		CanShoot = true;
		CanReload = true;
	}
	
	#endregion
	
	#region HUD
	void OnGUI()
	{
		if (!Active) return;
		if (!HUDOn) return;
		
		GUI.Label(new Rect(DebugScreenCoords.x,DebugScreenCoords.y, 300,20), _Name + " : " + _CurrentClip + " - " + AmmunitionLeft);
	}
	
	#endregion
	
}
