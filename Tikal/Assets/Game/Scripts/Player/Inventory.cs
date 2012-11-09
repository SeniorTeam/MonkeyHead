using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Inventory : MonoBehaviour 
{
	Game c_Game;
	GameObject _Game;	
	HudInventory c_HudInventory;
	GameObject _HudInventory;
	public GameObject WeaponPickUpText;
	[SerializeField] GameObject EyeLevel;
	[SerializeField] Player_Controls pControls;
	[SerializeField] GameObject RightArm;
	[SerializeField] GameObject WeaponStartOff;
	
	Vector2 HUD_Health_Coords;
	Vector2 HUD_Weapon_Coords;
	
	string  RightTrigger;
	string  LeftTrigger;
	string  Trigger;
		
	GameObject Player;
	GameObject Hud;
	
	int PlayerNumber;
	int PlayerControllerNumber;
	string PlayersName;
	string PlayersHUD;
	bool canGrab = true;
	bool canAttack = true;
	bool canDisplay = true;
	bool weaponAttack;
	bool foundParent;
	
	const int AMOUNT_OF_WEAPONS = 2;
	public List<Weapon> inventory = new List<Weapon>();
	
	void Awake()
	{
		_Game = GameObject.Find("Game");
		c_Game = _Game.GetComponent<Game>();
		StartCoroutine(FindParent(transform.parent.transform));
	}
	
	IEnumerator FindParent(Transform obj)
	{
		yield return new WaitForSeconds(.1f);
		
		if (obj.parent.tag != "Player")
		{
			//Debug.Log(obj.parent.name);
			StartCoroutine(FindParent(obj.parent.transform));
		}
		else
		{
			//Debug.Log("Found: " + obj.parent.name);
			Player = obj.parent.gameObject;
			PlayersName = obj.parent.name;
			PlayerNumber = pControls.PlayersNumber(PlayersName);
			PlayerControllerNumber = obj.parent.GetComponent<Player_Info>().PlayerControllerNumber;
			//Debug.Log ( "name: " + PlayersName + "  num: " + PlayerNumber + "  cont: " + PlayerControllerNumber);
			
			#if UNITY_STANDALONE_OSX
			RightTrigger = PlayerControllerNumber.ToString() + "_RightTrigger_MAC";
			LeftTrigger = PlayerControllerNumber.ToString() + "_LeftTrigger_MAC";
			#endif
			
			#if UNITY_STANDALONE_WIN
			Trigger = PlayerControllerNumber.ToString() + "_Trigger_PC";
			#endif
			
			PlayersHUD = obj.parent.GetComponent<Player_Info>().PlayerHUD.ToString();
			_HudInventory = GameObject.Find(PlayersHUD);
			Hud = _HudInventory.gameObject;
			
			if (PlayersHUD != null)
			{
				foreach (Transform _obj in _HudInventory.transform)
				{	
					//Debug.Log(_obj.name);
					
					if (_obj.name == "Text")
						WeaponPickUpText = _obj.gameObject;
					if (_obj.name == "Weapons")
					{
						c_HudInventory = _HudInventory.GetComponentInChildren<HudInventory>();
						foundParent = true;
						StartCoroutine(SearchWeapons());
						// Give player a starting weapon
						StartCoroutine(Weapon_Start());
					}
					
					yield return null;
				}
			}
			else
				Debug.Log("= null");		
		}
		
		// Get player debug stat coords
		DebugStats();
		
		yield return null;	
	}

	void DebugStats ()
	{
		// Health Location
		if (PlayerNumber == 1 || PlayerNumber == 3)
			HUD_Health_Coords.x = 10;
		if (PlayerNumber == 2 || PlayerNumber == 4)
			HUD_Health_Coords.x = (Screen.width/2) + 10;
		
		if (PlayerNumber == 1 || PlayerNumber == 2)
			HUD_Health_Coords.y = 12;
		if (PlayerNumber == 3 || PlayerNumber == 4)
			HUD_Health_Coords.y = (Screen.height/2) + 12;
		
		// Weapon Location
		if (PlayerNumber == 1 || PlayerNumber == 3)
			HUD_Weapon_Coords.x = (Screen.width/2) - 200;
		if (PlayerNumber == 2 || PlayerNumber == 4)
			HUD_Weapon_Coords.x = Screen.width - 200;
		
		if (PlayerNumber == 1 || PlayerNumber == 2)
			HUD_Weapon_Coords.y = 12;
		if (PlayerNumber == 3 || PlayerNumber == 4)
			HUD_Weapon_Coords.y = (Screen.height/2) + 12;
	}
	
	void Update()
	{
		if (c_Game.StartGame && foundParent)
		{
			WeaponInput_Controller();
			HoldWeapon();
			HudUpdate ();
		}
	}
	
	//Input for weapon use
	void WeaponInput_Controller ()
	{	
		// Pressed X to pick up weapon 
		if (Input.GetKeyDown(pControls.Get_X(PlayerControllerNumber)) )
		{
			//Debug.Log(PlayersName + "  Pressed X");
			if (inventory.Count() < AMOUNT_OF_WEAPONS)
			{
				if (canGrab)	// delay of .5 seconds, cant mash button
					StartCoroutine(Weapon_Add());
			}
		}
		
		// Pressed LB to display inventory
		if (Input.GetKeyDown(pControls.Get_LB(PlayerControllerNumber)) )
		{
			//Debug.Log(PlayersName + "  Pressed LB");
			if (canDisplay)
				StartCoroutine(Weapon_Inventory());
		}
		
		// Pressed RB to display inventory
		if (Input.GetKeyDown(pControls.Get_RB(PlayerControllerNumber)) )
		{
			//Debug.Log(PlayersName + "  Pressed RB");
			StartCoroutine(Weapon_Reload());
		}
		
		// Pressed B to remove current weapon from inventory
		if (Input.GetKeyDown(pControls.Get_B(PlayerControllerNumber)) )
		{
			//Debug.Log(PlayersName + "  Pressed B");
			StartCoroutine(Weapon_Drop());
		}
		
		// Pressed Y to swap weapons
		if (Input.GetKeyDown(pControls.Get_Y(PlayerControllerNumber)) )
		{
			//Debug.Log(PlayersName + "  Pressed 	Y");
			StartCoroutine(Weapon_Swap());
		}
		
		#if UNITY_STANDALONE_OSX
		float rightTrigger = Input.GetAxis(RightTrigger);	// Get Right trigger
		if (rightTrigger > 0.5f)
		{
			//Debug.Log(PlayersName + "  Pressed RIGHT TRIGGER");
			if (canAttack)
				StartCoroutine(Weapon_Attack());
		}
		#endif
		
		#if UNITY_STANDALONE_WIN
		float trigger = Input.GetAxis(Trigger);	// Get Right trigger
		if (trigger < -0.5f)
		{
			if (canAttack)
				StartCoroutine(Weapon_Attack());
		}
		#endif
	}
	
	void HoldWeapon()
	{
		if (inventory.Count > 0)
		{
			foreach (Weapon _weapon in inventory)
			{
				_weapon.transform.position = transform.position;	
				_weapon.transform.forward = transform.forward;
				_weapon.transform.eulerAngles = transform.eulerAngles;
			}
		}
	}

	void HudUpdate ()
	{
		foreach (Weapon _weapon in inventory)
		{
			//Debug.Log(_weapon._Name + "  " + _weapon._Ammo + "         " + _weapon._Type);
			
			if (_weapon.Active)
			{
				if ( _weapon._Type == "Gun")
					 _weapon.DebugScreenCoords = HUD_Weapon_Coords;
			}
		}
	}
	
	// Search weapons on map, if non in inventory make pickup text blank
	IEnumerator SearchWeapons ()
	{
		int counter = 0;
		
		Weapon[] weaponList = FindObjectsOfType(typeof(Weapon)) as Weapon[];
		foreach (Weapon _weapon in weaponList)
		{
			// is weapon already equipped
			if (!_weapon.Equipped)
			{
				// is player in radius
				if (_weapon.inPlayerRadius[PlayerNumber-1])
				{
					counter++;
				}
			}
			yield return null;
		}
		
		if (counter == 0)
		{
			WeaponPickUpText.guiText.text = " ";
		}
		
		yield return new WaitForSeconds(0.25f);
		StartCoroutine(SearchWeapons());
	}
	
	#region Options
	IEnumerator Weapon_Start()
	{
		Vector3 pos = gameObject.transform.position;
		GameObject _weapon = Instantiate(WeaponStartOff, new Vector3 (pos.x, pos.y, pos.z), Quaternion.identity) as GameObject;
		
		c_HudInventory.WeaponAdd(_weapon.GetComponentInChildren<Weapon>().HudMaterial, inventory.Count());
		
		_weapon.GetComponentInChildren<Weapon>().Active = true;					// Make weapon active, currently in use
		_weapon.GetComponentInChildren<Weapon>().ToggleWeaponRenderer(true); 	// turn on its renderer
			
		_weapon.GetComponentInChildren<Weapon>().WeaponAdd(PlayerNumber); 					// weapon added attributes
		inventory.Add(_weapon.GetComponentInChildren<Weapon>());				// add it to inventory
		
		
		
		HudUpdate ();
		
		yield return null;
	}
	
	// Add Weapon to inventory if it is not full already
	IEnumerator Weapon_Add ()
	{
		//Debug.Log("In Weapon Add");
		canGrab = false;
		
		// serach all weapons on map
		Weapon[] weaponList = FindObjectsOfType(typeof(Weapon)) as Weapon[];
		
		//Debug.Log("Count: " + weaponList.Count());
		
		foreach (Weapon _weapon in weaponList)
		{
			// is weapon already equipped
			if (!_weapon.Equipped)
			{
				//Debug.Log("Name: " + _weapon.name + "   Equipped: " + _weapon.Equipped);
				
				// is player in radius
				if (_weapon.inPlayerRadius[PlayerNumber-1])
				{	
					//Debug.Log("Name: " + _weapon.name + "   Radius: " + _weapon.inPlayerRadius);
					
					// Arm animation to pickup, wait .5 seconds so the animation is at
					// its lowest point, hand is down off screen picking up weapon
					// this hides the fact that the renderers are changing
					RightArm.animation.Play("Weapon_Get");
					yield return new WaitForSeconds(.5f);
					
					// if inventory already contains a weapon
					if (inventory.Count() > 0)
					{
						// find weapon that is active
						foreach (Weapon weapon in inventory)
						{
							if 	(weapon.Active)
							{
								weapon.Active = false;
								weapon.ToggleWeaponRenderer(false);
							}
						}
					}
				
					c_HudInventory.WeaponAdd(_weapon.HudMaterial, inventory.Count());
					_weapon.Active = true;				// Make weapon active, currently in use
					_weapon.ToggleWeaponRenderer(true); // turn on its renderer
						
					_weapon.WeaponAdd(PlayerNumber); 				// weapon added attributes
					inventory.Add(_weapon);				// add it to inventory
				}
			}
			
			HudUpdate();
			
			yield return null;
		}
		
		// timer delay so x button is not mashed
		float timer = .5f;
		while (timer > 0)
		{
			timer -= Time.deltaTime;
			yield return null;
		}
		
		canGrab = true;
	}
	
	// Player Died So Drop All Weapons
	public void DropWeapons()
	{
		StartCoroutine(_DropWeapons());
	}
	
	IEnumerator _DropWeapons()
	{
		while (inventory.Count != 0)
		{
			foreach (Weapon _weapon in inventory)
			{
				_weapon.WeaponDrop(gameObject.transform);			// drop current weapon attributes
				inventory.Remove(_weapon);							// remove from inventory
				break;
			}
			
			yield return null;
		}
		
		Player.GetComponent<Player_Info>().KillPlayer();
		yield return null;
	}
	// Drop weapon from inventory
	IEnumerator Weapon_Drop()
	{
		// if there is a weapon in the inventory
		if (inventory.Count() > 0 )
		{
			//Debug.Log("RELEASE");
			
			// Arm animation to pickup, wait .5 seconds so the animation is at
			// its lowest point, hand is down off screen picking up weapon
			// this hides the fact that the renderers are changing
			RightArm.animation.Play("Weapon_Get");
			c_HudInventory.WeaponDrop();
			yield return new WaitForSeconds(0.7f);
			
			// Search weapons in inventory, if active/in use, drop it
			foreach (Weapon _weapon in inventory)
			{
				if(_weapon.Active)
				{
					_weapon.WeaponDrop(gameObject.transform);			// drop current weapon attributes
					inventory.Remove(_weapon);		// remove from inventory
					break;
				}
				yield return null;
			}
			
			// turn weapon that is not in use on
			foreach (Weapon _weapon in inventory)
			{
				if(!_weapon.Active)
				{
					_weapon.ToggleWeaponRenderer(true);	// turn weapons renderers on
					_weapon.Active = true;				// make weapon active
				}		
				yield return null;
			}

		}
		
		yield return null;
	}
	
	// Display Inventory
	IEnumerator Weapon_Inventory()
	{
		canDisplay = false;
		c_HudInventory.DisplayHud();
		
		float timer = 1.0f;
		while (timer > 0)
		{
			timer -= Time.deltaTime;
			yield return null;
		}
		
		canDisplay = true;
		
		/*Debug.Log("INVENTORY");
		// Foreach weapon in inventory debug its name
		foreach (Weapon _weapon in inventory)
		{
			Debug.Log (	_weapon.WeaponName.ToString());
			yield return null;
		}
		*/
		yield return null;
	}
	
	// Display Current weapon
	IEnumerator Weapon_Current()
	{
		// is inventory empty? if not proceed
		if (inventory.Count() > 0 )
		{
			Debug.Log("CURRENT WEAPON:");
			foreach (Weapon _weapon in inventory)
			{
				// if weapon is active/in use debug its name
				if (_weapon.Active)
					Debug.Log(_weapon.WeaponName.ToString());
				
				yield return null;
			}
		}
		
		yield return null;
	}
	
	// Swap weapons in inventory
	IEnumerator Weapon_Swap()
	{
		// if inventory is greater then 1 proceed
		if (inventory.Count() > 1 )
		{
			bool swap = true;
			
			// Check to see if user has an objective item, if so can not swap
			foreach (Weapon _weapon in inventory)
			{
				if (_weapon.Active)
					if (_weapon._Type == "Objective")
						swap = false;
				
				yield return null;
			}
			
			// if no objective item in use swap
			if (swap)
			{
				//Debug.Log("SWAP");
				
				// Arm animation to pickup, wait .5 seconds so the animation is at
				// its lowest point, hand is down off screen picking up weapon
				// this hides the fact that the renderers are changing
				RightArm.animation.Play("Weapon_Get");
				c_HudInventory.SwapWeapons();
				yield return new WaitForSeconds(0.5f);
				
				// search weapons in inventory
				foreach (Weapon _weapon in inventory)
				{
					// if weapon is active/in use turn off
					if (_weapon.Active)
					{
						_weapon.ToggleWeaponRenderer(false);	// turn renderers of weapon off	
						_weapon.Active = false;					// make weapon inactive
					}
					else // if weapon is not active/in use turn on
					{
						_weapon.ToggleWeaponRenderer(true);		// turn renderers of weapon on	
						_weapon.Active = true;					// make weapon active
					}
					
					yield return null;
				}
			}
			
			yield return null;
		}
		
		HudUpdate();
		
		yield return null;
	}
	
	// Attack with current weapon
	IEnumerator Weapon_Attack()
	{
		canAttack = false;
		
		bool isGun = false;
		bool isObjective = false;
		
		//search through inventory
		foreach (Weapon _weapon in inventory)
		{
			// if weapon is active/in use  attack
			if (_weapon.Active)
			{
				if (_weapon._Type == "Gun")
					isGun = true;
				else if (_weapon._Type == "Objective")
				{
					isObjective = true;
					break;
				}
					
				_weapon.StartAttack(EyeLevel, Hud);	
				RightArm.animation.clip = _weapon.wAnimation;
				RightArm.animation.Play();	// use attack animation for current weapon
			}
			
			yield return null;
		}
		yield return null;
		
		if (!isObjective)
		{
			// timer delay so attack button is not mashed
			float timer = 1.0f;
			if (isGun)
				timer = 0.2f;
			
			while (timer > 0)
			{
				timer -= Time.deltaTime;
				yield return null;
			}
			
			foreach (Weapon _weapon in inventory)
			{
				// Stop Attack
				if (_weapon.Active)
				{
					_weapon.EndAttack();
				}
				
				yield return null;
			}
		}
		
		canAttack = true;
	}
	
	IEnumerator Weapon_Reload()
	{
		foreach (Weapon _weapon in inventory)
		{
			// if weapon is active/in use debug its name
			if (_weapon.Active)
			{
				if (_weapon._Type == "Gun")
				{
					_weapon.ReloadWeapon();	
				}
			}
			
			yield return null;
		}
	}
	
	/*IEnumerator HasObjectHitEnemy()
	{
		foreach (Weapon _weapon in inventory)
		{
			if (_weapon.Attacking)
			{
				while (	_weapon.Attacking)
				{
					yield return null;
				}
			}
		
			yield return null;
		}
	}*/
	#endregion
	
	void OnGUI()
	{
		if (c_Game.StartGame && foundParent)
		{
			GUI.Label(new Rect(HUD_Health_Coords.x, HUD_Health_Coords.y, 300,20), "Health: " + Player.GetComponent<Player_Info>().GetPlayersHealth());
		}
	}
	
}
