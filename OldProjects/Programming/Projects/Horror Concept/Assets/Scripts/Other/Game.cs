using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
	public bool StartGame;
	
	void Awake () 
	{
		StartGame = true;
		Screen.showCursor = false;	
	}
	
	void Update () 
	{
	}
	
	void OnGUI()
	{
	
	}
}
