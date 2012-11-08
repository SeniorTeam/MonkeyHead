using UnityEngine;
using System.Collections;

public class AssetManager : MonoBehaviour 
{

	[SerializeField] GameManager  gameManager;
	[SerializeField] LevelManager levelManager;
	[SerializeField] EnemyManager enemyManager;
	[SerializeField] WeaponManager weaponManager;
	[SerializeField] WeaponSlot weaponSlot;
	
	public bool LoadingComplete;
	
	void Awake()
	{
		StartCoroutine(LoadData());
	}
	
	IEnumerator LoadData()
	{
		while(true)
		{
			if (gameManager.GameDataLoaded &&
				levelManager.LevelsLoaded  &&
				enemyManager.EnemiesLoaded &&
			    weaponManager.WeaponsLoaded &&
			    weaponSlot.WeaponSlotsLoaded)
			{
				break;	
			}
			yield return null;	
		}
		
		LoadingComplete = true;
		
	}
	void TestData()
	{
		if(Input.GetKeyDown(KeyCode.T))
		{
			for(int i=0; i <10; i++)
			{
				Debug.Log("Time " + (i+1) + ":  " + gameManager.TimeLow[i] + "-" + gameManager.TimeHigh[i]);	
			}
		}
		
		if(Input.GetKeyDown(KeyCode.L))
		{
			for(int i=0; i <levelManager.NumberOfLevels; i++)
			{
				Debug.Log("Level " + (i+1) + ":  " + levelManager.lName[i]);	
			}
		}
		
		if(Input.GetKeyDown(KeyCode.E))
		{
			for(int i=0; i <enemyManager.NumberOfEnemies; i++)
			{
				Debug.Log(enemyManager.eName[i] + " " +
						  enemyManager.eHP[i] + " " +
						  enemyManager.eSpeed[i] + " " +
					      enemyManager.eAttack[i] + " " +
						  enemyManager.eSize[i] + " " +
				 		  enemyManager.eSpawnTime[i]);
			}
		}
		
		if(Input.GetKeyDown(KeyCode.B))
		{
			for(int i=0; i <enemyManager.NumberOfBosses; i++)
			{
				Debug.Log(enemyManager.bName[i] + " " +
						  enemyManager.bHP[i] + " " +
						  enemyManager.bSpeed[i] + " " +
					      enemyManager.bAttack[i]);
			}
		}
	}

	void Update()
	{
		// DebugLogs Data
		//TestData();
	}
}
