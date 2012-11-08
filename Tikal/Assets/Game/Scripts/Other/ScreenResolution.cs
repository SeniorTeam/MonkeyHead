using UnityEngine;
using System.Collections;

public class ScreenResolution : MonoBehaviour 
{
	void Awake()
	{
		Resolution[] resolution = Screen.resolutions;
		
		foreach ( Resolution res in resolution)
		{
			Debug.Log(res.width + "x" + res.height);
		}
		Screen.SetResolution(resolution[0].width, resolution[0].height, true);
	}
	
}
