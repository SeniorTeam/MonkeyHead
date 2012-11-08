using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
	[SerializeField] EnemyManager enemyManager;
	[SerializeField] DissovleLoadingScreen screen;
	
	public bool LoadingComplete;
	
	public AudioClip Song;
	
	void Awake()
	{
		StartCoroutine(LoadData());
	}
	
	IEnumerator LoadData()
	{
		while(true)
		{
			if (enemyManager.EnemyManager_Loaded)
			{
				break;	
			}
			yield return null;	
		}
		
		LoadingComplete = true;
		
		// Note Duration
		audio.clip = Song;
		InvokeRepeating("SpawnEnemies",2, 2 - (screen.Duration*2));
		yield return new WaitForSeconds(2);
		audio.Play();
		
	}
	
	void Update()
	{
		if (LoadingComplete)
		{
			
		}
	}

	void SpawnEnemies ()
	{
		Debug.Log("Add");
		enemyManager.AddEnemy();
	}
}
