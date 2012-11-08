using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour 
{	
	[SerializeField] WeaponManager _manager;
	public string WeaponName;
	public AnimationClip wAnimation;
	public Material HudMaterial;
	
	public bool Equipped;
	public bool inPlayerRadius;
	public bool Active;
	bool Attacking;
	float distanceToPickUp = 2.5f;
	string pickup;
	
	
	#region Stats
	public string _Name;
	public string _Type;
		
	public float  _Ammo;
	public float  _Damage;
	public float  _FireRate;
	public float  _Range;
	public float  _Splash;
	public float  _Spray;
	#endregion
	
	GameObject Player;
	GameObject Inventory;
	
	void Awake()
	{
		StartCoroutine (GameStart());
	}
	
	void Update()
	{
		if (Equipped)
		{
			WeaponUpdate();
		}
	}
	
	IEnumerator GameStart()
	{
		while (!_manager.WeaponsLoaded)
			yield return null;	
		
		Initialize();
		yield return null;
	}
	
	void Initialize()
	{
		foreach (WeaponManager.Stat _stat in _manager.wStats)
		{
			if (WeaponName == _stat._Name)
			{
				_Name     = _stat._Name.ToString();
				_Type     = _stat._Type.ToString();
				_Ammo     = _stat._Ammo;
				_Damage   = _stat._Damage;
				_FireRate = _stat._FireRate;
				_Range    = _stat._Range;
				_Splash   = _stat._Splash;
				_Spray    = _stat._Spray;
			}
		}

		Player = GameObject.Find("Player");
		Inventory = GameObject.Find("Inventory");
		pickup = "Pick Up " + WeaponName.ToString();
		StartCoroutine(PlayerDistance());
	}
	
	IEnumerator PlayerDistance()
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
			//Debug.Log(gameObject.name + "/" + weapon.ToString() + " OUT of height range");
			
			inPlayerRadius = false;
			yield return new WaitForSeconds(1.0f);
			StartCoroutine(PlayerDistance());
		}
		else // In players height range
		{
			//Debug.Log(gameObject.name + "/" + weapon.ToString() + " IN height range");
			
			// If player distance to weapon is less than Distance
			if ( isPlayerInDistance(DistanceForm(pPos.x, pPos.z, wPos.x, wPos.z)) )
			{
				inPlayerRadius = true;
				WeaponText(pickup);
			}
			else
				inPlayerRadius = false;
			
			// If not equipped search again after .25 second delay
			if (!Equipped)
			{
				yield return new WaitForSeconds(0.25f);
				StartCoroutine(PlayerDistance());	
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

	void WeaponText (string text)
	{
		Inventory.GetComponent<Inventory>().WeaponPickUpText.text = text;
	}
	
	private bool isPlayerInDistance(float distance)
	{
		bool inDistance = false;
		
		if ( distance < distanceToPickUp)
			inDistance = true;
		
		return inDistance; 	
	}	

	void WeaponUpdate ()
	{
		Vector3 pos = Inventory.transform.position;
		Vector3 forward = Player.transform.forward;
		
		
		transform.forward = forward;
		transform.eulerAngles = Player.transform.eulerAngles;
		transform.position = pos;
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
	
	public void WeaponAdd()
	{
		inPlayerRadius = false;
		Equipped = true;
		ToggleWeaponColliders(true);
		rigidbody.useGravity = false;
		 
	}
	
	public void WeaponDrop()
	{
		Active = false;
		Equipped = false;
		transform.position = Inventory.transform.position;
			
		ToggleWeaponColliders(false);
		rigidbody.useGravity = true;
		StartCoroutine(PlayerDistance());
	}
	
	void OnTriggerEnter(Collider hit)
	{
		if (Attacking)
		{
			if (hit.gameObject.tag == "Enemy")
			{
				Debug.LogWarning("WOW WHAT A HIT");	
			}
		}
	}
	
	public virtual void StartAttack()
	{
		Attacking = true;
	}
	
	public virtual void EndAttack()
	{
		Attacking = false;
	}
	
}
