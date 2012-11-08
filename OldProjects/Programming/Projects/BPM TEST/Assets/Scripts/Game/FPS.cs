using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour 
{
	[SerializeField] GUIText FpsText;
	int lastFrame = 0;
	bool debugOn = true;
	
	void Awake()
	{
		FpsText.enabled = true;
		InvokeRepeating("CountFps",0,1);	
	}
	
	void CountFps()
	{
		int frame = Time.frameCount;
		int diff = frame - lastFrame;
		lastFrame = frame;
		FpsText.text = diff.ToString();
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.D))
		{
			if (debugOn)
			{
				FpsText.enabled = false;
				debugOn = false;
			}
			else
			{
				FpsText.enabled = true;
				debugOn = true;
			}
		}
	}
}
