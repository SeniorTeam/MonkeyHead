using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class EnemyManager : MonoBehaviour 
{
	#region Declarartions
	[SerializeField] GameManager _game;
	[SerializeField] TextAsset EnemyStats;
	[SerializeField] GameObject SpawnLocation;
	[SerializeField] LevelManager levelManager;
	
	public bool EnemiesLoaded;
	
	#region Enemies
	public int NumberOfEnemies;
	public string[] eName;
	public float[] eHP;
	public float[] eSpeed;
	public float[] eAttack;
	public float[] eSize;
	public int[] eSpawnTime;
	
	private bool enemiesLoaded;
	#endregion
	#region Bosses
	public int NumberOfBosses;
	public string[] bName;
	public float[] bHP;
	public float[] bSpeed;
	public float[] bAttack;
	
	private bool bossesLoaded;
	#endregion
	#endregion
	

	void Awake()
	{
		GetStats();
		StartCoroutine(DataLoaded());
	}
	
	#region Load Data
	/*////////////////////////////////////
	//////////////////////////////////////	 
	*////////////////////////////////////
	IEnumerator DataLoaded()
	{
		while(true)
		{
		
			if (enemiesLoaded && bossesLoaded)
			{
				EnemiesLoaded = true;
				levelManager.GetData();
				break;
			}
			
			yield return null;
		}
	}

	public void GetStats()
	{
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(EnemyStats.text);
		
		StartCoroutine(LoadEnemyStats(xmlDoc));
		StartCoroutine(LoadBossStats(xmlDoc));
		
	}
	
	IEnumerator LoadEnemyStats(XmlDocument xmlDoc)
	{
		XmlNodeList enemiesList = xmlDoc.GetElementsByTagName("enemy"); // array of the level nodes.
	
		NumberOfEnemies = enemiesList.Count;
		CountEnemies(NumberOfEnemies);
		int counter = 0;
		
		foreach (XmlNode enemyInfo in enemiesList)
		{
			XmlNodeList enemyContent = enemyInfo.ChildNodes;
			
			foreach (XmlNode enemy in enemyContent)
			{
				if(enemy.Name == "object")
				{
					switch(enemy.Attributes["name"].Value)
					{
						case "Name": 
						{
							eName[counter] = enemy.InnerText;
							break;
						}
						case "HP": 
						{
							eHP[counter] = float.Parse(enemy.InnerText);
							break;
						}
						case "Speed":
						{
							eSpeed[counter] = float.Parse(enemy.InnerText);
							break;
						}
						case "Attack":
						{
							eAttack[counter] = float.Parse(enemy.InnerText);
							break;
						}
						case "Size": 
						{
							eSize[counter] = float.Parse(enemy.InnerText);
							counter++;
							break;
						}
					}
					yield return null;
				}
				yield return null;
			}
			yield return null;
		}
		yield return null;
		enemiesLoaded = true;
	}
	
	IEnumerator LoadBossStats(XmlDocument xmlDoc)
	{
		XmlNodeList bossList = xmlDoc.GetElementsByTagName("boss"); // array of the level nodes.
	
		NumberOfBosses = bossList.Count;
		CountBosses(NumberOfBosses);
		int counter = 0;
		
		foreach (XmlNode bossInfo in bossList)
		{
			XmlNodeList bossContent = bossInfo.ChildNodes;
			
			foreach (XmlNode boss in bossContent)
			{
				if(boss.Name == "object")
				{
					switch(boss.Attributes["name"].Value)
					{
						case "Name": 
						{
							bName[counter] = boss.InnerText;
							break;
						}
						case "HP": 
						{
							bHP[counter] = float.Parse(boss.InnerText);
							break;
						}
						case "Speed":
						{
							bSpeed[counter] = float.Parse(boss.InnerText);
							break;
						}
						case "Attack":
						{
							bAttack[counter] = float.Parse(boss.InnerText);
							counter++;
							break;
						}
					}
					yield return null;
				}
				yield return null;
			}
			yield return null;
		}
		yield return null;
		bossesLoaded = true;
	}
	
	private void CountEnemies(int num)
	{
		eName = new string[num];
		eHP = new float[num];
		eSpeed = new float[num];
		eAttack = new float[num];
		eSize = new float[num];
		eSpawnTime = new int[num];
	}
	private void CountBosses(int num)
	{
		bName = new string[num];
		bHP = new float[num];
		bSpeed = new float[num];
		bAttack = new float[num];
	}
	
	/*////////////////////////////////////
	//////////////////////////////////////	 
	*////////////////////////////////////
	#endregion
		
		
	public IEnumerator AddEnemy(int amountOfEnemies, int EnemyNumber)
	{
		
		float time = GetTime(eSpawnTime[EnemyNumber]);
		
		
		
		while (time > 0)
		{
			time -= Time.deltaTime;
			yield return null;	
		}
		
		
		
		GameObject _enemy = Instantiate(Resources.Load("Prefabs/Enemies/" + eName[EnemyNumber])) as GameObject;
		
		_enemy.GetComponent<Enemy>().Position = spawnPoint();
		_enemy.GetComponent<Enemy>().Name = eName[EnemyNumber];
		_enemy.GetComponent<Enemy>().Health = eHP[EnemyNumber];
		_enemy.GetComponent<Enemy>().Speed = SpeedMultiplier(eSpeed[EnemyNumber]);
		_enemy.GetComponent<Enemy>().Attack = eAttack[EnemyNumber];
		_enemy.GetComponent<Enemy>().Size = eSize[EnemyNumber];
		
		
		if (amountOfEnemies > 0)
		{
			amountOfEnemies--;
			//Debug.Log("E: " + EnemyNumber + "   L: " + amountOfEnemies);
			StartCoroutine(AddEnemy(amountOfEnemies, EnemyNumber));
		}
		yield return null;
	}
	
	private Vector3 spawnPoint()
	{
		Vector3 spawnPoint;
		
		float diameter = (SpawnLocation.transform.localScale.z)/2;
		float rndPoint = Random.Range((float)-diameter, (float)diameter);
		spawnPoint = new Vector3(SpawnLocation.transform.position.x, 0, rndPoint);
		
		return spawnPoint;
	}
	
	private float SpeedMultiplier(float speed)
	{
		speed = speed/100;	
		
		int rnd = Random.Range(1,4);
		
		speed = speed * rnd;
		
		return speed;
		
	}
	
	public void AddBoss(int EnemyNumber)
	{
		
	}
	public void AddSpecialEenemy(string Prefab, float HP, float Speed, float Attack, float Size)
	{
		
	}
		
	private float GetTime(int num)
	{
		float time = Random.Range((float)_game.TimeLow[num], (float)_game.TimeHigh[num] + 1);
		return time;
	}
}
