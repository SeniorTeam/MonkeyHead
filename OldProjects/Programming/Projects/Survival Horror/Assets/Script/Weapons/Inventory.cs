using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Inventory : MonoBehaviour 
{
	
	[SerializeField] GameObject RightArm;
	public GUIText WeaponPickUpText;
	bool canGrab = true;
	bool canAttack = true;
	
	const int AMOUNT_OF_WEAPONS = 2;
	public List<Weapon> inventory = new List<Weapon>();

	
	void Awake()
	{
		WeaponPickUpText.text = " ";
		StartCoroutine(SearchWeapons());
	}
	
	void Update()
	{
		WeaponInput();
	}
	
	//Input for weapon use
	void WeaponInput ()
	{		
		// Pressed x to pick up weapon 
		if (Input.GetKeyDown(KeyCode.JoystickButton18))
		{
			if (inventory.Count() < AMOUNT_OF_WEAPONS)
			{
				if (canGrab)	// delay of .5 seconds, cant mash button
					StartCoroutine(Weapon_Add());
			}
		}
		
		// pressed d-pad up button, FOR DEBUG, display inventory
		if (Input.GetKeyDown(KeyCode.JoystickButton5))
		{
			StartCoroutine(Weapon_Inventory());
		}
		
		// pressed d-pad left button, FOR DEBUG, display current weapon
		if (Input.GetKeyDown(KeyCode.JoystickButton7))
		{
			StartCoroutine(Weapon_Current());
		}
		
		// pressed d-pad down button, FOR DEBUG, remove active inventory
		if (Input.GetKeyDown(KeyCode.JoystickButton6))
		{
			StartCoroutine(Weapon_Drop());
		}
		
		// swap weapons y button
		if (Input.GetKeyDown(KeyCode.JoystickButton19))
		{
			StartCoroutine(Weapon_Swap());
		}
	
		float leftTrigger  = Input.GetAxis("Trigger_L");	// Get Left trigger
		float rightTrigger = Input.GetAxis("Trigger_R");	// Get Right trigger
		
		// Attack, right arm
		if (rightTrigger > 0.5f)
		{
			if (canAttack)
				StartCoroutine(Weapon_Attack());
		}
		
		
		/* Get reaction from all triggers
		if (left > .2f && right > .2f)
		{
			Debug.Log("Both");
		}
		else if (left > .2f)
		{
			Debug.Log("Left");
		} 
		else if (right > .2f)
		{
			Debug.Log("Right");
		}
		*/
		
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
				if (_weapon.inPlayerRadius)
				{
					counter++;
				}
			}
			yield return null;
		}
		
		if (counter == 0)
		{
			WeaponPickUpText.text = " ";
		}
		
		yield return new WaitForSeconds(0.25f);
		StartCoroutine(SearchWeapons());
	}
	
	#region Options
	
	// Add Weapon to inventory if it is not full already
	IEnumerator Weapon_Add ()
	{
		canGrab = false;
		
		// serach all weapons on map
		Weapon[] weaponList = FindObjectsOfType(typeof(Weapon)) as Weapon[];
		
		foreach (Weapon _weapon in weaponList)
		{
			// is weapon already equipped
			if (!_weapon.Equipped)
			{
				// is player in radius
				if (_weapon.inPlayerRadius)
				{	
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
				
					_weapon.Active = true;				// Make weapon active, currently in use
					_weapon.ToggleWeaponRenderer(true); // turn on its renderer
						
					_weapon.WeaponAdd(); 				// weapon added attributes
					inventory.Add(_weapon);				// add it to inventory
				}
			}
			
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
	
	// Drop weapon from inventory
	IEnumerator Weapon_Drop()
	{
		// if there is a weapon in the inventory
		if (inventory.Count() > 0 )
		{
			Debug.Log("RELEASE");
			
			// Arm animation to pickup, wait .5 seconds so the animation is at
			// its lowest point, hand is down off screen picking up weapon
			// this hides the fact that the renderers are changing
			RightArm.animation.Play("Weapon_Get");
			yield return new WaitForSeconds(0.7f);
			
			// Search weapons in inventory, if active/in use, drop it
			foreach (Weapon _weapon in inventory)
			{
				if(_weapon.Active)
				{
					_weapon.WeaponDrop();			// drop current weapon attributes
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
		Debug.Log("INVENTORY");
		// Foreach weapon in inventory debug its name
		foreach (Weapon _weapon in inventory)
		{
			Debug.Log (	_weapon.weapon.ToString());
			yield return null;
		}
		
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
					Debug.Log(_weapon.weapon.ToString());
				
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
			Debug.Log("SWAP");
			
			// Arm animation to pickup, wait .5 seconds so the animation is at
			// its lowest point, hand is down off screen picking up weapon
			// this hides the fact that the renderers are changing
			RightArm.animation.Play("Weapon_Get");
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
	
	// Attack with current weapon
	IEnumerator Weapon_Attack()
	{
		canAttack = false;
		
		//search through inventory
		foreach (Weapon _weapon in inventory)
		{
			// if weapon is active/in use  attack
			if (_weapon.Active)
			{
				RightArm.animation.Play(_weapon.wAnimation);	// use attack animation for current weapon
				
			}
			
			yield return null;
		}
		yield return null;
		
		// timer delay so attack button is not mashed
		float timer = 1.0f;
		while (timer > 0)
		{
			timer -= Time.deltaTime;
			yield return null;
		}
		
		canAttack = true;
	}
	#endregion
	
}