using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
	[SerializeField] AssetManager assetManager;
	[SerializeField] EnemyManager enemyManager;
	[SerializeField] LevelManager levelManager;
	
	bool canSpawnWaves = true;
	
	int LEVEL = 1;
	
	void Awake()
	{

	}
	
	void Update()
	{
		if (assetManager.LoadingComplete)
		{
			if (canSpawnWaves)
			{
				//Debug.Log("START THE GAME ALREADY");
				canSpawnWaves = false;
				StartCoroutine(StartWaveSpawns(LEVEL-1));
			}
		}
	}
	
	IEnumerator StartWaveSpawns(int level)
	{
		// Get Number of Enemies
		// Get type of enemies that spawn each wave
		
		for (int i=0; i < enemyManager.NumberOfEnemies; i++)
		{
			if (levelManager.enemySpawn[level,i] == 1)
			{
				int numEnemies = RandomWaveCount();
				//Debug.Log("E: " + (i+1) + "    A: " + numEnemies);
				enemyManager.StartCoroutine(enemyManager.AddEnemy(numEnemies, i));
			}
			yield return null;
		}
	}
	
	int RandomWaveCount()
	{
		int numberOfEnemies = 0;
		numberOfEnemies = Random.Range(10, 15);
		
		return numberOfEnemies;
	}
}
