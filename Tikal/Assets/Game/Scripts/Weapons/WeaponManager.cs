using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;


public class WeaponManager : MonoBehaviour 
{
	
	public struct Stat
	{
		public string _Name;
		public string _Type;
		
		public float  _Ammo;
		public float  _Clips;
		public float  _Damage;
		public float  _FireRate;
		public float  _Range;
		public float  _Splash;
		public float  _Spray;
	}

	[SerializeField] TextAsset WeaponStats;
	
	#region Weapon Data
	public int NumberOfWeapons;
	
	public List<Stat> wStats = new List<Stat>();
	Stat newWeapon;
	
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
		XmlNodeList weaponList = xmlDoc.GetElementsByTagName("weapon");
	
		NumberOfWeapons = weaponList.Count;
		foreach (XmlNode weaponInfo in weaponList)
		{
			XmlNodeList weaponContent = weaponInfo.ChildNodes;
			newWeapon = new Stat();
			
			foreach (XmlNode weapon in weaponContent)
			{
				if(weapon.Name == "object")
				{
					
					switch(weapon.Attributes["name"].Value)
					{
						case "Name": 
						{
							newWeapon._Name = weapon.InnerText;
							break;
						}
						case "Type": 
						{
							newWeapon._Type = weapon.InnerText;
							break;
						}
						case "Ammo": 
						{
							newWeapon._Ammo = float.Parse(weapon.InnerText);
							break;
						}
						case "Clips": 
						{
							newWeapon._Clips = float.Parse(weapon.InnerText);
							break;
						}
						case "Damage": 
						{
							newWeapon._Damage = float.Parse(weapon.InnerText);
							break;
						}
						case "Fire_Rate": 
						{
							newWeapon._FireRate = float.Parse(weapon.InnerText);
							break;
						}
						case "Range": 
						{
							newWeapon._Range = float.Parse(weapon.InnerText);
							break;
						}
						case "Splash": 
						{
							newWeapon._Splash = float.Parse(weapon.InnerText);
							break;
						}
						case "Spray": 
						{
							newWeapon._Spray = float.Parse(weapon.InnerText);		
							break;
						}
					}
					
				}
				
				yield return null;
			}
			
			wStats.Add(newWeapon);
			yield return null;
		}
		yield return null;
		WeaponsLoaded = true;
		//Test();
	}
	
	void Test()
	{
		foreach (Stat _stat in wStats)
		{
			Debug.Log("\n");
			Debug.Log("Name   : " + _stat._Name);
			Debug.Log("Type   : " + _stat._Type);
	 		Debug.Log("Ammo   : " + _stat._Ammo);
			Debug.Log("Clips  : " + _stat._Clips);
			Debug.Log("Damage : " + _stat._Damage);
	 		Debug.Log("Fire R : " + _stat._FireRate);
	 		Debug.Log("Range  : " + _stat._Range);
			Debug.Log("Splash : " + _stat._Splash);
			Debug.Log("Spray  : " + _stat._Spray);
			Debug.Log("\n");
		}
	}
	
}
