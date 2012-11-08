using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class GameManager : MonoBehaviour 
{
	[SerializeField] TextAsset GameStats;
	
	public int[] TimeLow;
	public int[] TimeHigh;
	public bool GameDataLoaded;
	
	void Awake()
	{
		GetStats();
	}
	
	
	public void GetStats()
	{
		XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
		xmlDoc.LoadXml(GameStats.text); // load the file.
		
		StartCoroutine(LoadSpawnTimes(xmlDoc));
	}
	
	IEnumerator LoadSpawnTimes(XmlDocument xmlDoc)
	{
		XmlNodeList spawnList = xmlDoc.GetElementsByTagName("spawn");
		
		TimeLow = new int[10];
		TimeHigh = new int[10];
		int counter = 0;
		
		foreach (XmlNode spawnInfo in spawnList)
		{
			XmlNodeList spawnContent = spawnInfo.ChildNodes;
			
			foreach (XmlNode spawn in spawnContent)
			{
				if(spawn.Name == "object")
				{
					switch(spawn.Attributes["name"].Value)
					{
						case "Low": 
						{
							TimeLow[counter] = int.Parse(spawn.InnerText);
							break;
						}
						case "High":
						{
							TimeHigh[counter] = int.Parse(spawn.InnerText);
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
		GameDataLoaded = true;
	}
	
}
