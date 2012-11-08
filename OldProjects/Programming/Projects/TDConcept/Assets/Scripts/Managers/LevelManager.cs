using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class LevelManager : MonoBehaviour 
{
	[SerializeField] TextAsset LevelStats;
	[SerializeField] EnemyManager enemyManager;
	
	#region Level Data
	public int NumberOfLevels;
	public string[] lName;
	
	public int[,] enemySpawn;
	
	public bool LevelsLoaded;
	#endregion
	
	
	public void GetData()
	{
			
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(LevelStats.text);
		
		StartCoroutine(LoadLevelStats(xmlDoc));	
	}
	
	IEnumerator LoadLevelStats(XmlDocument xmlDoc)
	{
		XmlNodeList levelList = xmlDoc.GetElementsByTagName("level"); // array of the level nodes.
	
		NumberOfLevels = levelList.Count;
		CountLevels(NumberOfLevels);
		int counter = 0;
		
		foreach (XmlNode levelInfo in levelList)
		{
			XmlNodeList levelContent = levelInfo.ChildNodes;
			
			foreach (XmlNode level in levelContent)
			{
				if(level.Name == "object")
				{
					switch(level.Attributes["name"].Value)
					{
						case "Level_Name": 
						{
							lName[counter] = level.InnerText;
							break;
						}
						
					}
				}
				
				if(level.Name == "wave")
				{
					switch(level.Attributes["name"].Value)
					{
						case "enemy1": 
						{
							enemySpawn[counter, 0] = int.Parse(level.InnerText);
							break;
						}
						case "enemy2": 
						{
							enemySpawn[counter, 1] = int.Parse(level.InnerText);
							break;
						}
						case "enemy3": 
						{
							enemySpawn[counter ,2] = int.Parse(level.InnerText);
							break;
						}
						case "enemy4": 
						{
							enemySpawn[counter, 3] = int.Parse(level.InnerText);
							break;
						}
						case "enemy5": 
						{
							enemySpawn[counter, 4] = int.Parse(level.InnerText);
							counter++;
							break;
						}
						
					}
				}
				yield return null;
			}
			yield return null;
		}
		yield return null;
		LevelsLoaded = true;
	}
	
	private void CountLevels(int num)
	{
		lName = new string[num];
		enemySpawn = new int [num, enemyManager.NumberOfEnemies];
	}
	
}
