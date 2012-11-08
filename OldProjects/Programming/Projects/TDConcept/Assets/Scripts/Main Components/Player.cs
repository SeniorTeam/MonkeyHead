using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	[SerializeField] AssetManager assetManager;
	[SerializeField] GameObject PlayersGunPos;
	
	public float TowerHealth;
	public float PlayerHealth;
	public int PlayerGold;
	
	public Vector3 StartPosition = new Vector3(-48, .25f,0);
	
	float pSpeed = 0.08f;
	float pAttack = 50.0f;
	
	
	bool nearTower = false;
	bool inTower = false;
	
	void Awake()
	{
		TowerHealth = 10000.0f;	
		PlayerHealth = 100;
		PlayerGold = 0;
		
		transform.position = StartPosition;
	}
	
	void Update()
	{
		if (assetManager.LoadingComplete)
		{
			UsersControls();
			
		}
	}
	
	#region Controls
	void UsersControls()
	{
		//Teleport in and out of tower	
		TowerTeleport();
		PlayerMovement();
	}
	
	void PlayerMovement()
	{
		Vector3 pos = transform.position;
		
		//Move Forward
		if (Input.GetKey(KeyCode.RightArrow))
		{
			pos.x += pSpeed;
		}
		
		//Move Backwards/ turn around
		else if(Input.GetKey(KeyCode.LeftArrow))
		{
			pos.x -= pSpeed;
		}
		
		//Move Left (Away from camera)
		if (Input.GetKey(KeyCode.UpArrow))
		{
			pos.z += pSpeed;
		}
		
		//Move Right (towards from camera)
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			pos.z -= pSpeed;
			
		}
		
		transform.position = pos;
	}

	
	void TowerTeleport()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			if (nearTower)
			{
				if (inTower)
				{
					//Teleport out of tower
					inTower = false;
				}
				else
				{
					// Teleport in tower
					inTower = true;	
				}
			}
		}
	}
	#endregion
	
	
	#region Main Components
	public void AttackTower(float reduction)
	{
		TowerHealth -= reduction;	
	}
	
	public void InflictDamage( float reduction)
	{
		PlayerHealth -= reduction;	
	}
	
	public void AddHealth( float amount)
	{
		PlayerHealth += amount;
	}
	
	public void AddGold( int amount)
	{
		PlayerGold += amount;	
	}
	#endregion
}
