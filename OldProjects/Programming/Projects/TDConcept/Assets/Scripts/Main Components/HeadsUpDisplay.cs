using UnityEngine;
using System.Collections;

public class HeadsUpDisplay : MonoBehaviour 
{
	[SerializeField] AssetManager assetManager;
	//[SerializeField] Tower tower;
	[SerializeField] Player player;
	
	
	bool HUDOn = true;
	
	void Update()
	{
		if (assetManager.LoadingComplete)
		{
			HUD();
		}
	}
	
	void HUD()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			if (HUDOn)
				HUDOn = false;
			else
				HUDOn = true;
		}
	}
	
	void OnGUI()
	{
		if (!HUDOn) return;
		
		GUI.Label(new Rect(10,60,300,20), "Tower Health: " + player.TowerHealth);
		GUI.Label(new Rect(10,80,300,20), "Player Health: " + player.PlayerHealth);
		GUI.Label(new Rect(10,100,300,20), "Player Gold: " + player.PlayerGold);
	}
}
