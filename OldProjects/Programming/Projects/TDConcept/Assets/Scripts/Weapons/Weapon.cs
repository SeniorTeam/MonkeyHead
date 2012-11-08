using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{
	#region Declaration
	public string Name {get; set;}
	public float  Damage {get; set;}
	public float  Ammo {get; set;}
	public float  FireRate {get; set;}
	public float  Range {get; set;}
	public float  Spray {get; set;}
	public float  Splash {get; set;}
	
	public float ReloadTime = 2.0f;
	public float CurrentAmmo;
	
	Transform[] children;
	GameObject Gun;
	
	Vector3 BulletSpawnPoint;
	
	bool IsGunActive = false;
	bool CanShoot = true;
	bool shooting = false;
	float sprayCount;
	bool HUDOn = true;
	
	#region Bullet Angles
	
	// ELEVEN ANGLES (this way comp does not have to do sin and cos for each shot
	int[] _angle = {0, 10, 20, 30, 40, 45, -45, -40, -30, -20, 10};
	float[] _cos = {1.0f, 0.9848f, 0.9396f, 0.8660f, 0.7660f, 0.7071f,
					0.7071f, 0.7660f, 0.8660f, 0.9396f, 0.9848f};
	float[] _sin = {0.0f, 0.1736f, 0.3420f, 0.5000f, 0.6427f, 0.7071f,
					 -0.7071f, -0.6427f, -0.5000f, -0.3420f, -0.1736f};
	
	Vector2 BulletCosSin;
	int BulletAngle;
	#endregion
	
	#endregion
	
	#region Initialize
	void Awake()
	{
		Gun = GameObject.Find("Gun");
		gameObject.transform.position = Gun.transform.position;
		StartCoroutine(Initialize());
	}
	
	IEnumerator Initialize()
	{
		yield return new WaitForSeconds(0.1f);	
		SetVariables();
	}
	
	void SetVariables()
	{
		CurrentAmmo = Ammo;
		
		if (Name != "Five_Seven")
			DeactivateWeapon();
		else
			ActivateWeapon();
	}
	#endregion
	
	#region Activate Deactivate Weapon
	public void DeactivateWeapon()
	{
		//Debug.Log(Name + "  is Deactive");
		HUDOn = false;
		IsGunActive = false;
		StartCoroutine(SwapWeaponOnOff(IsGunActive));
	}
	
	public void ActivateWeapon()
	{
		Debug.Log(Name + "  is Active");
		HUDOn = true;
		IsGunActive = true;
		StartCoroutine(SwapWeaponOnOff(IsGunActive));
	}
	
	IEnumerator SwapWeaponOnOff(bool setting)
	{
		//Debug.Log("GameObject: " + transform.name);
		
		children = gameObject.GetComponentsInChildren<Transform>();
		foreach ( Transform child in children)
		{
			//Debug.Log("Child: " + child.name);
			
			if (child.renderer != null)
				child.renderer.enabled = setting;
			if (child.collider != null)
				child.collider.enabled = setting;
			
			if (child.name == "BulletSpawn")
				BulletSpawnPoint = child.transform.position;
			
			yield return null;
		}
	}
	#endregion
	
	
	void Update()
	{
		gameObject.transform.position = Gun.transform.position;
		
		if (IsGunActive)
		{
			CheckControls();
		}
	}
	
	#region Controls
	void CheckControls()
	{
		PlayerShoot();
		PlayerReload();
	}
	
	void PlayerShoot()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			PlayerShooting();
		}
		if (Input.GetKey(KeyCode.Space))
		{
			if (Name == "UZI")
			{
				PlayerShooting();
			}
		}
	}
	
	void PlayerShooting()
	{
		if (CanShoot)
			{
				if (CurrentAmmo > 0)
				{
					if (!shooting)
					{
						//Debug.Log("BANG");
						shooting = true;
						CurrentAmmo--;
						sprayCount = Spray;
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
		float time = FireRate;
		
		while(time > 0)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		shooting = false;
	}
	
	void CreateBullet()
	{
		RandomAngle(sprayCount);
		
		children = gameObject.GetComponentsInChildren<Transform>();
		foreach ( Transform child in children)
		{
			if (child.name == "BulletSpawn")
				BulletSpawnPoint = child.transform.position;
		}
		
		GameObject _bullet = Instantiate(Resources.Load("Prefabs/Weapons/Bullets/Bullet"), BulletSpawnPoint, Quaternion.Euler(0, BulletAngle, 0) ) as GameObject;
		
		//Debug.Log(BulletSpawnPoint);
		
		_bullet.GetComponent<Bullet>().StartPosition = BulletSpawnPoint;
		_bullet.GetComponent<Bullet>().Damage = Damage;
		_bullet.GetComponent<Bullet>().Range = Range;
		_bullet.GetComponent<Bullet>().SinCos = BulletCosSin;
		_bullet.GetComponent<Bullet>().Angle = BulletAngle;
		_bullet.GetComponent<Bullet>().Direction = 1;
		
		
		if (sprayCount > 1)
		{
			sprayCount--;
			CreateBullet();
		}
	}
	
	void RandomAngle(float spray)
	{
		int rnd;
		
		if (spray > 1)
			rnd = Random.Range(0,12);
		else
			rnd = 0;
		BulletCosSin = new Vector2(_cos[rnd], _sin[rnd]);
		BulletAngle = _angle[rnd];
	}
	
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
		CurrentAmmo = Ammo;
		
	}
	#endregion
	
	
	#region HUD
	void OnGUI()
	{
		if (!HUDOn) return;
		
		GUI.Label(new Rect(10,120,300,20), "Ammo: " + CurrentAmmo);
	}
	#endregion
	
}
