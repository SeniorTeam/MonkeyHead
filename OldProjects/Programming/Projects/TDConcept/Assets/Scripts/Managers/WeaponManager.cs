using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class WeaponManager : MonoBehaviour 
{
	[SerializeField] TextAsset WeaponStats;
	
	#region Weapon Data
	public int NumberOfWeapons;
	
	public string[] wName;
	public float[]  wDamage;
	public float[]  wAmmo;
	public float[]  wFireRate;
	public float[]  wRange;
	public float[]  wSpray;
	public float[]  wSplash;
	
	public bool WeaponsLoaded;
	#endregion
	
	
	void Awake()
	{
		GetData();
	}
	
	public void GetData()
	{
			
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(WeaponStats.text);
		
		StartCoroutine(LoadWeaponStats(xmlDoc));	
	}
	
	IEnumerator LoadWeaponStats(XmlDocument xmlDoc)
	{
		XmlNodeList weaponList = xmlDoc.GetElementsByTagName("gun");
	
		NumberOfWeapons = weaponList.Count;
		CountWeapons(NumberOfWeapons);
		int counter = 0;
		
		foreach (XmlNode weaponInfo in weaponList)
		{
			XmlNodeList weaponContent = weaponInfo.ChildNodes;
			
			foreach (XmlNode weapon in weaponContent)
			{
				if(weapon.Name == "object")
				{
					switch(weapon.Attributes["name"].Value)
					{
						case "Name": 
						{
							wName[counter] = weapon.InnerText;
							break;
						}
						case "Damage": 
						{
							wDamage[counter] = float.Parse(weapon.InnerText);
							break;
						}
						case "Ammo": 
						{
							wAmmo[counter] = float.Parse(weapon.InnerText);
							break;
						}
						case "Fire_Rate": 
						{
							wFireRate[counter] = float.Parse(weapon.InnerText);
							break;
						}
						case "Range": 
						{
							wRange[counter] = float.Parse(weapon.InnerText);
							break;
						}
						case "Spray": 
						{
							wSpray[counter] = float.Parse(weapon.InnerText);
							break;
						}
						case "Splash": 
						{
							wSplash[counter] = float.Parse(weapon.InnerText);
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
		WeaponsLoaded = true;
		//Test();
	}
	
	private void CountWeapons(int num)
	{
		wName = new string[num];
	 	wDamage = new float[num];
	 	wAmmo = new float[num];
	 	wFireRate = new float[num];
	 	wRange = new float[num];
		wSpray = new float[num];
		wSplash = new float[num];
	}
	
	void Test()
	{
		for (int i =0; i <NumberOfWeapons; i++)
		{
			Debug.Log("\n");
			Debug.Log("Name   : " + wName[i]);
	 		Debug.Log("Damage : " + wDamage[i]);
	 		Debug.Log("Ammo   : " + wAmmo[i]);
	 		Debug.Log("FR     : " + wFireRate[i]);
	 		Debug.Log("Range  : " + wRange[i]);
			Debug.Log("Spray  : " + wSpray[i]);
			Debug.Log("Splash : " + wSplash[i]);
			Debug.Log("\n");
		}
	}
	
}
