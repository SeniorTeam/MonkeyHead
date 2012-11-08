using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Weapon : MonoBehaviour 
{	
	Game c_Game;
	GameObject _Game;
	WeaponManager _manager;
	GameObject _WeaponManager;
	public string WeaponName;
	public AnimationClip wAnimation;
	public Material HudMaterial;
	
	public bool Equipped;
	public bool[] inPlayerRadius;
	public bool Active;
	bool Attacking;
	float distanceToPickUp = 2.5f;
	string pickup;
	
	public List<GUITexture> HudBullets = new List<GUITexture>();
	
	public Vector2 DebugScreenCoords;
	
	
	#region Stats
	public string _Name;
	public string _Type;
		
	public float  _Ammo;
	public float  _Clips;
	public float  _ClipSize;
	public float  _CurrentClip;
	public float  _Damage;
	public float  _FireRate;
	public float  _Range;
	public float  _Splash;
	public float  _Spray;
	#endregion
	
	Player_Info Player;
	
	void Awake()
	{
		_Game = GameObject.Find("Game");
		c_Game = _Game.GetComponentInChildren<Game>();
		_WeaponManager = GameObject.Find("Weapons Manager");
		
		_manager = _WeaponManager.GetComponentInChildren<WeaponManager>();
		
		inPlayerRadius = new bool[4];
		
		StartCoroutine (GameStart());
	}
	
	IEnumerator GameStart()
	{
		while (!_manager.WeaponsLoaded)
			yield return null;	
		while (!c_Game.StartGame)
			yield return null;
		
		Initialize();
		yield return null;
	}
	
	public virtual void Initialize()
	{
		foreach (WeaponManager.Stat _stat in _manager.wStats)
		{
			if (WeaponName == _stat._Name)
			{
				_Name     	 = _stat._Name.ToString();
				_Type     	 = _stat._Type.ToString();
				_Ammo     	 = _stat._Ammo;
				_Clips    	 = _stat._Clips;
				_ClipSize 	 = _Ammo/_Clips;
				_CurrentClip = _Ammo/_Clips;
				_Damage   	 = _stat._Damage;
				_FireRate 	 = _stat._FireRate;
				_Range    	 = _stat._Range;
				_Splash   	 = _stat._Splash;
				_Spray    	 = _stat._Spray;
			}
		}

		pickup = "Pick Up " + WeaponName.ToString();
		
		foreach (Player_Info _player in c_Game.player)
		{
			StartCoroutine(PlayerDistance(_player));
		}
	}
	
	IEnumerator PlayerDistance(Player_Info Player)
	{
		if ( Player.gameObject != null)
		{
			Vector3 pPos = Player.transform.position;
			Vector3 wPos = transform.position;
			
			float hPositive = wPos.y + distanceToPickUp;	// weapon height plus distancePickUp
			float hNegative = wPos.y - distanceToPickUp;	// weapon height minus distancePickUp
			
			// if weapon is more than 5 units above or below the players point
			// the weapon is declared out of reach and will not glow or be able
			// to get. Wait one second to see if it is out of reach again, skip
			// rest of function;
			if (pPos.y >  hPositive || pPos.y < hNegative)
			{
				//Debug.Log(gameObject.name + "/" + WeaponName.ToString() + " OUT of height range");
				
				inPlayerRadius[Player.PlayerNumber-1] = false;
				yield return new WaitForSeconds(1.0f);
				StartCoroutine(PlayerDistance(Player));
			}
			else // In players height range
			{
				// If player distance to weapon is less than Distance
				if ( isPlayerInDistance(DistanceForm(pPos.x, pPos.z, wPos.x, wPos.z)) )
				{
					//Debug.Log(gameObject.name + "/" + WeaponName.ToString() + " IN of xz range");
					inPlayerRadius[Player.PlayerNumber-1] = true;
					WeaponText(pickup, Player.gameObject);
				}
				else
				{
					//Debug.Log(gameObject.name + "/" + WeaponName.ToString() + " OUT of Distance range");
					//Debug.Log("Distance: " + DistanceForm(pPos.x, pPos.z, wPos.x, wPos.z));
					inPlayerRadius[Player.PlayerNumber-1] = false;
				}
				
				// If not equipped search again after .25 second delay
				if (!Equipped)
				{
					yield return new WaitForSeconds(0.25f);
					StartCoroutine(PlayerDistance(Player));	
				}
			
			}
		}
		
	}
	
	private float DistanceForm (float player_x, float player_z, float weapon_x, float weapon_z)
	{
		float x1 = player_x;
		float x2 = weapon_x;
		float y1 = player_z;
		float y2 = weapon_z;
		
		float distance;
		
		float xFactor = Mathf.Pow( (x2 - x1), 2);
		float yFactor = Mathf.Pow( (y2 - y1), 2);
		
		distance = Mathf.Sqrt( (xFactor + yFactor) );
		distance = Mathf.Abs(distance);
		
		return distance;
	}

	void WeaponText (string text, GameObject Player)
	{		
		GameObject inv = Player.GetComponentInChildren<Inventory>().gameObject;
		inv.GetComponent<Inventory>().WeaponPickUpText.guiText.text = text;
		
	}
	
	private bool isPlayerInDistance(float distance)
	{
		bool inDistance = false;
		
		if ( distance < distanceToPickUp)
			inDistance = true;
		
		return inDistance; 	
	}	
	
	// Toggle all of the weapons renderers off
	public void ToggleWeaponRenderer(bool toggle)
	{
		Renderer[] allChildren = gameObject.GetComponentsInChildren<Renderer>();
		
		foreach (Renderer child in allChildren) 
		{
			if (child.renderer != null)
		   		child.renderer.enabled = toggle;
		}
	}
	
	// Toggle all of the colliders to triggers
	public void ToggleWeaponColliders(bool toggle)
	{
		Collider[] allChildren = gameObject.GetComponentsInChildren<Collider>();
		
		foreach (Collider child in allChildren) 
		{
			if (child.collider != null)
		   		child.collider.isTrigger = toggle;
		}
	}
	
	public void WeaponAdd(int number)
	{
		inPlayerRadius[number-1] = false;
		Equipped = true;
		ToggleWeaponColliders(true);
		rigidbody.useGravity = false;
		 
	}
	
	public void WeaponDrop(Transform inventory)
	{
		Active = false;
		Equipped = false;
		transform.position = inventory.position;
			
		ToggleWeaponColliders(false);
		rigidbody.useGravity = true;
		
		foreach (Player_Info _player in c_Game.player)
		{
			StartCoroutine(PlayerDistance(_player));
		}
	}
	
	public virtual void StartAttack(GameObject EyeLevel, GameObject Hud)
	{
		Attacking = true;
	}
	
	public virtual void EndAttack()
	{
		Attacking = false;
	}
	
	public virtual void ReloadWeapon()
	{
		
	}
}
