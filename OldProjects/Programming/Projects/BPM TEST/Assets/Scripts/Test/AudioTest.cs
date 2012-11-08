using UnityEngine;
using System.Collections;
using System.Linq;

public class AudioTest : MonoBehaviour 
{
	[SerializeField] GameObject[] AudioPorts; 
	bool[] muted;
	GameObject currentObject;
	
	void Awake()
	{
		muted = new bool[5];
		
		currentObject = AudioPorts[0];
		
		for (int i=0; i< 5; i++)
			muted[i] = false;
	}
	
	
	void Update()
	{
		GetUsersInput();
	}
	
	void GetUsersInput ()
	{
		CurrentPort();
		MovePort();
		MutePort();
	}

	void CurrentPort ()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
			currentObject = AudioPorts[0];
		
		if (Input.GetKeyDown(KeyCode.Alpha2))
			currentObject = AudioPorts[1];
		
		if (Input.GetKeyDown(KeyCode.Alpha3))
			currentObject = AudioPorts[2];
		
		if (Input.GetKeyDown(KeyCode.Alpha4))
			currentObject = AudioPorts[3];
		
		if (Input.GetKeyDown(KeyCode.Alpha5))
			currentObject = AudioPorts[4];
		
	}

	void MovePort ()
	{
		Vector3 pos = currentObject.transform.position;
		
		if (Input.GetKey(KeyCode.Equals))
			pos.z += .1f;
			
		if (Input.GetKey(KeyCode.Minus))
			pos.z -= .1f;
		
		currentObject.transform.position =  pos; 	
	}

	void MutePort ()
	{
		if (Input.GetKeyDown(KeyCode.N))
			Debug.Log(currentObject.audio.clip.name);
		
		if (Input.GetKeyDown(KeyCode.M))
		{	
			for (int i=0; i< 5; i++)
			{
				if (currentObject.name == AudioPorts[i].name)
				{
					if (muted[i])
					{
						AudioPorts[i].audio.volume = 1;
						muted[i] = false;
					}
					else
					{
						AudioPorts[i].audio.volume = 0;
						muted[i] = true;
					}
				}
			}
		}
	}
	
	void OnGUI()
	{
		GUI.Label( new Rect (10, 60, 300, 20), "Selected: " + currentObject.name );
		
		GUI.Label( new Rect (10, 100, 300, 20), AudioPorts[0].name + ": " + AudioPorts[0].transform.position.z);
		GUI.Label( new Rect (10, 120, 300, 20), AudioPorts[1].name + ": " + AudioPorts[1].transform.position.z);
		GUI.Label( new Rect (10, 140, 300, 20), AudioPorts[2].name + ": " + AudioPorts[2].transform.position.z);
		GUI.Label( new Rect (10, 160, 300, 20), AudioPorts[3].name + ": " + AudioPorts[3].transform.position.z);
		GUI.Label( new Rect (10, 180, 300, 20), AudioPorts[4].name + ": " + AudioPorts[4].transform.position.z);
	}
}
