using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{	
	public enum WeaponType
	{ Pipe = 0, Bottle, Gun }
	
	public WeaponType weapon;
	
	public string wAnimation;
	public bool Equipped;
	public bool inPlayerRadius;
	public bool Active;
	float distanceToPickUp = 2.5f;
	string pickup;
	
	GameObject Player;
	GameObject Inventory;
	
	void Awake()
	{
		Player = GameObject.Find("Player");
		Inventory = GameObject.Find("Inventory");
		pickup = "Pick Up " + weapon.ToString();
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
	

	void Update()
	{
		if (Equipped)
		{
			WeaponUpdate();
		}
	}

	void WeaponUpdate ()
	{
		Vector3 pos = Inventory.transform.position;
		Vector3 rot = transform.eulerAngles;
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
	
}
