using UnityEngine;
using System.Collections;

public class Player_Controls : MonoBehaviour 
{
	[HideInInspector]
	public int pNumber;
	
	public int PlayersNumber(string PlayerName)
	{
		int num = 1;
		
		if (PlayerName == "Player_2")
			return 2;
		if (PlayerName == "Player_3")
			return 3;
		if (PlayerName == "Player_4")
			return 4;
		
		return num;	
	}
	
	#if UNITY_STANDALONE_WIN
	////////////////////////////////////////////////////////////////
	#region PC
	////////////////////////////////////////////////////////////////
	#region ABXY
	////////////////////////////////////////////////////////////////
	public KeyCode Get_A(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button0;
		
		if (n == 2)
			return KeyCode.Joystick2Button0;
		if (n == 3)
			return KeyCode.Joystick3Button0;
		if (n == 4)
			return KeyCode.Joystick4Button0;
		
		return c;		
	}
	
	public KeyCode Get_B(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button1;
		
		if (n == 2)
			return KeyCode.Joystick2Button1;
		if (n == 3)
			return KeyCode.Joystick3Button1;
		if (n == 4)
			return KeyCode.Joystick4Button1;
		
		return c;		
	}	
	
	public KeyCode Get_X(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button2;
		
		if (n == 2)
			return KeyCode.Joystick2Button2;
		if (n == 3)
			return KeyCode.Joystick3Button2;
		if (n == 4)
			return KeyCode.Joystick4Button2;
		
		return c;		
	}
	
	public KeyCode Get_Y(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button3;
		
		if (n == 2)
			return KeyCode.Joystick2Button3;
		if (n == 3)
			return KeyCode.Joystick3Button3;
		if (n == 4)
			return KeyCode.Joystick4Button3;
		
		return c;		
	}	
	////////////////////////////////////////////////////////////////
	#endregion
	#region D PAD
	////////////////////////////////////////////////////////////////
	public string Get_D_PAD_HORIZONTAL(int PlayerNumber)
	{
		int    n = PlayerNumber;
		string s = n.ToString() + "_D_PAD_Horizontal_PC";

		return s; 	
	}
	
	public string Get_D_PAD_VERTTICAL(int PlayerNumber)
	{
		int    n = PlayerNumber;
		string s = n.ToString() + "_D_PAD_Vertical_PC";

		return s;	
	}	
	////////////////////////////////////////////////////////////////
	#endregion
	#region RB/LB
	////////////////////////////////////////////////////////////////
	public KeyCode Get_LB(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button4;
		
		if (n == 2)
			return KeyCode.Joystick2Button4;
		if (n == 3)
			return KeyCode.Joystick3Button4;
		if (n == 4)
			return KeyCode.Joystick4Button4;
		
		return c;		
	}
	
	public KeyCode Get_RB(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button5;
		
		if (n == 2)
			return KeyCode.Joystick2Button5;
		if (n == 3)
			return KeyCode.Joystick3Button5;
		if (n == 4)
			return KeyCode.Joystick4Button5;
		
		return c;		
	}
	////////////////////////////////////////////////////////////////
	#endregion
	#region Other
	////////////////////////////////////////////////////////////////
	public KeyCode Get_LEFT_THUMB_CLICK(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button8;
		
		if (n == 2)
			return KeyCode.Joystick2Button8;
		if (n == 3)
			return KeyCode.Joystick3Button8;
		if (n == 4)
			return KeyCode.Joystick4Button8;
		
		return c;		
	}
	
	public KeyCode Get_RIGHT_THUMB_CLICK(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button9;
		
		if (n == 2)
			return KeyCode.Joystick2Button9;
		if (n == 3)
			return KeyCode.Joystick3Button9;
		if (n == 4)
			return KeyCode.Joystick4Button9;
		
		return c;		
	}	
	
	public KeyCode Get_START(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button7;
		
		if (n == 2)
			return KeyCode.Joystick2Button7;
		if (n == 3)
			return KeyCode.Joystick3Button7;
		if (n == 4)
			return KeyCode.Joystick4Button7;
		
		return c;		
	}
	
	public KeyCode Get_SELECT(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button6;
		
		if (n == 2)
			return KeyCode.Joystick2Button6;
		if (n == 3)
			return KeyCode.Joystick3Button6;
		if (n == 4)
			return KeyCode.Joystick4Button6;
		
		return c;		
	}
	
	public KeyCode Get_XBOX_BUTTON(int PlayerNumber)
	{
		KeyCode c = KeyCode.Joystick1Button10;
		
		Debug.LogError("Error-> XBOX_BUTTON is not a button for PC (MAC ONLY) -> using default null button 10");
		
		return c;		
	}
	////////////////////////////////////////////////////////////////
	#endregion
	#region Triggers
	////////////////////////////////////////////////////////////////
	public string Get_TRIGGER(int PlayerNumber)
	{
		int    n = PlayerNumber;
		string s = n.ToString() + "_Trigger_PC";

		return s;		
	}	
	////////////////////////////////////////////////////////////////
	#endregion
	#region Analog Sticks
	////////////////////////////////////////////////////////////////
	public string Get_ANALOG_RIGHT_HORIZONTAL(int PlayerNumber)
	{
		int    n = PlayerNumber;
		string s = n.ToString() + "_Move_Horizontal_PC";

		return s;		
	}	
	
	public string Get_ANALOG_RIGHT_VERTICAL(int PlayerNumber)
	{
		int    n = PlayerNumber;
		string s = n.ToString() + "_Move_Vertical_PC";

		return s;		
	}	
	
	public string Get_ANALOG_LEFT_HORIZONTAL(int PlayerNumber)
	{
		int    n = PlayerNumber;
		string s = n.ToString() + "_Look_Horizontal_PC";

		return s;		
	}	
	
	public string Get_ANALOG_LEFT_VERTICAL(int PlayerNumber)
	{
		int    n = PlayerNumber;
		string s = n.ToString() + "_Look_Vertical_PC";

		return s;		
	}	
	////////////////////////////////////////////////////////////////
	#endregion
	
	////////////////////////////////////////////////////////////////
	#endregion
	////////////////////////////////////////////////////////////////
	#endif
		
	#if UNITY_STANDALONE_OSX
	////////////////////////////////////////////////////////////////
	#region MAC
	////////////////////////////////////////////////////////////////
	#region ABXY
	////////////////////////////////////////////////////////////////
	public KeyCode Get_A(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button16;
		
		if (n == 2)
			return KeyCode.Joystick2Button16;
		if (n == 3)
			return KeyCode.Joystick3Button16;
		if (n == 4)
			return KeyCode.Joystick4Button16;
		
		return c;		
	}
	
	public KeyCode Get_B(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button17;
		
		if (n == 2)
			return KeyCode.Joystick2Button17;
		if (n == 3)
			return KeyCode.Joystick3Button17;
		if (n == 4)
			return KeyCode.Joystick4Button17;
		
		return c;		
	}	
	
	public KeyCode Get_X(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button18;
		
		if (n == 2)
			return KeyCode.Joystick2Button18;
		if (n == 3)
			return KeyCode.Joystick3Button18;
		if (n == 4)
			return KeyCode.Joystick4Button18;
		
		return c;		
	}
	
	public KeyCode Get_Y(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button19;
		
		if (n == 2)
			return KeyCode.Joystick2Button19;
		if (n == 3)
			return KeyCode.Joystick3Button19;
		if (n == 4)
			return KeyCode.Joystick4Button19;
		
		return c;		
	}	
	////////////////////////////////////////////////////////////////
	#endregion
	#region D PAD
	////////////////////////////////////////////////////////////////
	public KeyCode Get_D_UP(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button5;
		
		if (n == 2)
			return KeyCode.Joystick2Button5;
		if (n == 3)
			return KeyCode.Joystick3Button5;
		if (n == 4)
			return KeyCode.Joystick4Button5;
		
		return c;		
	}
	
	public KeyCode Get_D_DOWN(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button6;
		
		if (n == 2)
			return KeyCode.Joystick2Button6;
		if (n == 3)
			return KeyCode.Joystick3Button6;
		if (n == 4)
			return KeyCode.Joystick4Button6;
		
		return c;		
	}	
	
	public KeyCode Get_D_LEFT(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button7;
		
		if (n == 2)
			return KeyCode.Joystick2Button7;
		if (n == 3)
			return KeyCode.Joystick3Button7;
		if (n == 4)
			return KeyCode.Joystick4Button7;
		
		return c;		
	}
	
	public KeyCode Get_D_RIGHT(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button8;
		
		if (n == 2)
			return KeyCode.Joystick2Button8;
		if (n == 3)
			return KeyCode.Joystick3Button8;
		if (n == 4)
			return KeyCode.Joystick4Button8;
		
		return c;		
	}
	////////////////////////////////////////////////////////////////
	#endregion
	#region RB/LB
	////////////////////////////////////////////////////////////////
	public KeyCode Get_LB(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button13;
		
		if (n == 2)
			return KeyCode.Joystick2Button13;
		if (n == 3)
			return KeyCode.Joystick3Button13;
		if (n == 4)
			return KeyCode.Joystick4Button13;
		
		return c;		
	}
	
	public KeyCode Get_RB(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button14;
		
		if (n == 2)
			return KeyCode.Joystick2Button14;
		if (n == 3)
			return KeyCode.Joystick3Button14;
		if (n == 4)
			return KeyCode.Joystick4Button14;
		
		return c;		
	}
	////////////////////////////////////////////////////////////////
	#endregion
	#region Other
	////////////////////////////////////////////////////////////////
	public KeyCode Get_LEFT_THUMB_CLICK(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button11;
		
		if (n == 2)
			return KeyCode.Joystick2Button11;
		if (n == 3)
			return KeyCode.Joystick3Button11;
		if (n == 4)
			return KeyCode.Joystick4Button11;
		
		return c;		
	}
	
	public KeyCode Get_RIGHT_THUMB_CLICK(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button12;
		
		if (n == 2)
			return KeyCode.Joystick2Button12;
		if (n == 3)
			return KeyCode.Joystick3Button12;
		if (n == 4)
			return KeyCode.Joystick4Button12;
		
		return c;		
	}	
	
	public KeyCode Get_START(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button9;
		
		if (n == 2)
			return KeyCode.Joystick2Button9;
		if (n == 3)
			return KeyCode.Joystick3Button9;
		if (n == 4)
			return KeyCode.Joystick4Button9;
		
		return c;		
	}
	
	public KeyCode Get_SELECT(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button10;
		
		if (n == 2)
			return KeyCode.Joystick2Button10;
		if (n == 3)
			return KeyCode.Joystick3Button10;
		if (n == 4)
			return KeyCode.Joystick4Button10;
		
		return c;		
	}
	
	public KeyCode Get_XBOX_BUTTON(int PlayerNumber)
	{
		int    n = PlayerNumber;
		KeyCode c = KeyCode.Joystick1Button15;
		
		if (n == 2)
			return KeyCode.Joystick2Button15;
		if (n == 3)
			return KeyCode.Joystick3Button15;
		if (n == 4)
			return KeyCode.Joystick4Button15;
		
		return c;		
	}
	////////////////////////////////////////////////////////////////
	#endregion
	#region Triggers
	////////////////////////////////////////////////////////////////
	public string Get_TRIGGER_RIGHT(int PlayerNumber)
	{
		int    n = PlayerNumber;
		string s = n.ToString() + "_RightTrigger_MAC";

		return s;		
	}	
	
	public string Get_TRIGGER_LEFT(int PlayerNumber)
	{
		int    n = PlayerNumber;
		string s = n.ToString() + "_LeftTrigger_MAC";

		return s;			
	}
	////////////////////////////////////////////////////////////////
	#endregion
	#region Analog Sticks
	////////////////////////////////////////////////////////////////
	public string Get_ANALOG_RIGHT_HORIZONTAL(int PlayerNumber)
	{
		int    n = PlayerNumber;
		string s = n.ToString() + "_Move_Horizontal_MAC";

		return s;		
	}	
	
	public string Get_ANALOG_RIGHT_VERTICAL(int PlayerNumber)
	{
		int    n = PlayerNumber;
		string s = n.ToString() + "_Move_Vertical_MAC";

		return s;		
	}	
	
	public string Get_ANALOG_LEFT_HORIZONTAL(int PlayerNumber)
	{
		int    n = PlayerNumber;
		string s = n.ToString() + "_Look_Horizontal_MAC";

		return s;		
	}	
	
	public string Get_ANALOG_LEFT_VERTICAL(int PlayerNumber)
	{
		int    n = PlayerNumber;
		string s = n.ToString() + "_Look_Vertical_MAC";

		return s;		
	}	
	////////////////////////////////////////////////////////////////
	#endregion
	////////////////////////////////////////////////////////////////
	#endregion
	////////////////////////////////////////////////////////////////
	#endif
}
