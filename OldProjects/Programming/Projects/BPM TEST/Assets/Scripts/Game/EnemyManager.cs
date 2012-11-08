using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class EnemyManager : MonoBehaviour 
{
	[SerializeField] TextAsset EnemyStats;
	[SerializeField] GameObject[] SpawnLocation;
	[SerializeField] GameObject[] EnemyType;
	public bool EnemyManager_Loaded;
	public bool StartLoad;
	
	public List<Enemy> EnemyList = new List<Enemy>();
	int enemyId = 0;
	#region Enemies
	public int NumberOfEnemies;
	
	public string[] eName;
	public int[]    eHP;
	public int[]    eAttack;
	public float[]  eSpeed;
	#endregion
	
	void Awake()
	{
		StartCoroutine(LoadData());
	}
	
	IEnumerator LoadData()
	{
		while(true)
		{
			if (StartLoad)
			{
				break;	
			}
			yield return null;	
		}
		
		GetStats();
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			AddEnemy();	
		}
	}
	
	
	#region Load Data
	
	public void GetStats()
	{
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(EnemyStats.text);
		
		StartCoroutine(LoadEnemyStats(xmlDoc));		
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
							eHP[counter] = int.Parse(enemy.InnerText);
							break;
						}
						case "Attack":
						{
							eAttack[counter] = int.Parse(enemy.InnerText);
							break;
						}
						case "Speed":
						{
							eSpeed[counter] = float.Parse(enemy.InnerText);
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
		EnemyManager_Loaded = true;
	}
	
	
	private void CountEnemies(int num)
	{
		eName = new string[num];
		eHP = new int[num];
		eAttack = new int[num];
		eSpeed = new float[num];
	}
	#endregion
		
		
	IEnumerator CreateEnemy(int EnemyNumber)
	{	
		GameObject _enemy = Instantiate(EnemyType[EnemyNumber]) as GameObject;
		
		_enemy.GetComponent<Enemy>().Position = spawnPoint();
		_enemy.GetComponent<Enemy>().Name = eName[EnemyNumber];
		_enemy.GetComponent<Enemy>().Health = eHP[EnemyNumber];
		_enemy.GetComponent<Enemy>().Attack = eAttack[EnemyNumber];
		_enemy.GetComponent<Enemy>().Speed = eSpeed[EnemyNumber];
		_enemy.GetComponent<Enemy>().Id = enemyId;
		
		EnemyList.Add(_enemy.GetComponent<Enemy>());
		
		Debug.Log("!!!!!!!!!!!!!!!");
		
		enemyId++;	
		
		yield return null;
	}
	
	public void AddEnemy()
	{
		int rand = Random.Range(0, NumberOfEnemies);
		Debug.Log(rand);
		StartCoroutine(CreateEnemy(rand));
	}
	
	private Vector3 spawnPoint()
	{
		Vector3 spawnPoint;
		
		// Get random node to spawn in
		int randNode = Random.Range(0, SpawnLocation.Length);
		
		// Get center of it
		Vector3 pos = SpawnLocation[randNode].transform.position;
		
		// Vector3 pos
		spawnPoint = new Vector3(pos.x , 1.5f, pos.z);
		
		return spawnPoint;
	}
}

