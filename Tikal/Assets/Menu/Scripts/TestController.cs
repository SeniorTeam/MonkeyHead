using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour 
{
	void Update()
	{
		PlayerOne_ButtonControls();
		PlayerOne_InputControls();
	}
	
	void PlayerOne_ButtonControls ()
	{
		if (Input.GetKeyDown(KeyCode.Joystick1Button1))
		{
			Debug.Log("Joystick1Button1");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button2))
		{
			Debug.Log("Joystick1Button2");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button3))
		{
			Debug.Log("Joystick1Button3");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button4))
		{
			Debug.Log("Joystick1Button4");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button5))
		{
			Debug.Log("Joystick1Button5");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button6))
		{
			Debug.Log("Joystick1Button6");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button7))
		{
			Debug.Log("Joystick1Button7");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button8))
		{
			Debug.Log("Joystick1Button8");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button9))
		{
			Debug.Log("Joystick1Button9");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button10))
		{
			Debug.Log("Joystick1Button10");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button11))
		{
			Debug.Log("Joystick1Button11");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button12))
		{
			Debug.Log("Joystick1Button12");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button13))
		{
			Debug.Log("Joystick1Button13");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button14))
		{
			Debug.Log("Joystick1Button14");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button15))
		{
			Debug.Log("Joystick1Button15");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button16))
		{
			Debug.Log("Joystick1Button16");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button17))
		{
			Debug.Log("Joystick1Button17");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button18))
		{
			Debug.Log("Joystick1Button18");
		}
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button19))
		{
			Debug.Log("Joystick1Button19");
		}
	}

	
	void PlayerOne_InputControls ()
	{
		
		float _1_1 = Input.GetAxis("1_1");
		float _1_2 = Input.GetAxis("1_2");
		float _1_3 = Input.GetAxis("1_3");
		float _1_4 = Input.GetAxis("1_4");
		float _1_5 = Input.GetAxis("1_5");
		float _1_6 = Input.GetAxis("1_6");
		float _1_7 = Input.GetAxis("1_7");
		float _1_8 = Input.GetAxis("1_8");
		float _1_9 = Input.GetAxis("1_9");
		float _1_10 = Input.GetAxis("1_10");
		
		
		if (_1_1 != 0)
			Debug.Log(" 1_1 :  " + _1_1); 
		if (_1_2 > 0.1f)
			Debug.Log(" 1_2 :  " + _1_2); 
		if (_1_3 != 0)
			Debug.Log(" 1_3 :  " + _1_3); 
		if (_1_4 != 0)
			Debug.Log(" 1_4 :  " + _1_4); 
		if (_1_5 != 0)
			Debug.Log(" 1_5 :  " + _1_5); 
		if (_1_6 != 0)
			Debug.Log(" 1_6 :  " + _1_6); 
		if (_1_7 != 0)
			Debug.Log(" 1_7 :  " + _1_7); 
		if (_1_8 != 0)
			Debug.Log(" 1_8 :  " + _1_8); 
		if (_1_9 != 0)
			Debug.Log(" 1_9 :  " + _1_9); 
		if (_1_10 != 0)
			Debug.Log(" 1_10 :  " + _1_10); 
		
		
	}
}
