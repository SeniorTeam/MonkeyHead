using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplitScreen_Cameras : MonoBehaviour 
{
	#region Camera Types
	///////// 1 Player
	Rect c_1_1 = new Rect(0,0,1,1);
	///////// 2 Players
	Rect c_2_1 = new Rect(0,.5f,1,.5f);
	Rect c_2_2 = new Rect(0,0,1,.5f);
	///////// 3 Players
	Rect c_3_1 = new Rect(0,.5f,1,.5f);
	Rect c_3_2 = new Rect(0,0,.5f,.5f);
	Rect c_3_3 = new Rect(.5f,0,.5f,.5f);
	///////// 4 Players
	Rect c_4_1 = new Rect(0,.5f,.5f,.5f);
	Rect c_4_2 = new Rect(.5f,.5f,.5f,.5f);
	Rect c_4_3 = new Rect(0,0,.5f,.5f);
	Rect c_4_4 = new Rect(.5f,0,.5f,.5f);
	///////// JUNK
	Rect c_null = new Rect(0,0,0,0);
	
	#endregion
	
	#region HudCamera Types
	///////// 1 Player
	Vector3 h_1_1 = new Vector3(.5f,.5f,0);
	
	///////// 2 Players
	Vector3 h_2_1 = new Vector3(.5f,.75f,0);
	Vector3 h_2_2 = new Vector3(.5f,.25f,0);
	///////// 3 Players
	Vector3 h_3_1 = new Vector3(.5f,.75f,0);
	Vector3 h_3_2 = new Vector3(.25f,.25f,0);
	Vector3 h_3_3 = new Vector3(.75f,.25f,0);
	///////// 4 Players
	Vector3 h_4_1 = new Vector3(.25f,.75f,0);
	Vector3 h_4_2 = new Vector3(.75f,.75f,0);
	Vector3 h_4_3 = new Vector3(.25f,.25f,0);
	Vector3 h_4_4 = new Vector3(.75f,.25f,0);
	
	///////// JUNK
	Vector3 h_null = new Vector3(0,0,0);
	#endregion
	
	#region Camera Types
	///////// 1 Player
	Vector2 ch_1_1 = new Vector2(30,30);
	///////// 2 Players
	Vector2 ch_2_1 = new Vector2(30,30);
	Vector2 ch_2_2 = new Vector2(30,30);
	///////// 3 Players
	Vector2 ch_3_1 = new Vector2(30,30);
	Vector2 ch_3_2 = new Vector2(15,15);
	Vector2 ch_3_3 = new Vector2(15,15);
	///////// 4 Players
	Vector2 ch_4_1 = new Vector2(15,15);
	Vector2 ch_4_2 = new Vector2(15,15);
	Vector2 ch_4_3 = new Vector2(15,15);
	Vector2 ch_4_4 = new Vector2(15,15);
	///////// JUNK
	Vector2 ch_null = new Vector2(0,0);
	
	#endregion
	
	public Dictionary<string, List<Rect>> Player_Camera = new Dictionary<string, List<Rect>>();
	public Dictionary<string, List<Vector3>> Player_HudCamera = new Dictionary<string, List<Vector3>>();
	public Dictionary<string, List<Vector2>> Player_CrossHair = new Dictionary<string, List<Vector2>>();
	
	[HideInInspector]
	public string[] PlayerCamera_String = new string[4];
	[HideInInspector]
	public string[] Player_HudCamera_String = new string[4];
	[HideInInspector]
	public string[] Player_CrossHair_String = new string[4];
	
	List<Rect> CameraView_1 = new List<Rect>();
	List<Rect> CameraView_2 = new List<Rect>();
	List<Rect> CameraView_3 = new List<Rect>();
	List<Rect> CameraView_4 = new List<Rect>();
	
	List<Vector3> HudCameraView_1 = new List<Vector3>();
	List<Vector3> HudCameraView_2 = new List<Vector3>();
	List<Vector3> HudCameraView_3 = new List<Vector3>();
	List<Vector3> HudCameraView_4 = new List<Vector3>();
	
	List<Vector2> CrossHairCoords_1 = new List<Vector2>();
	List<Vector2> CrossHairCoords_2 = new List<Vector2>();		
	List<Vector2> CrossHairCoords_3 = new List<Vector2>();
	List<Vector2> CrossHairCoords_4 = new List<Vector2>();
	
	public void Initialize()
	{
		// Camera View
		CameraView_1.Add(c_1_1);
		CameraView_1.Add(c_2_1);
		CameraView_1.Add(c_3_1);
		CameraView_1.Add(c_4_1);
		Player_Camera.Add("Player_Camera_1", CameraView_1);

		CameraView_2.Add(c_null);
		CameraView_2.Add(c_2_2);
		CameraView_2.Add(c_3_2);
		CameraView_2.Add(c_4_2);
		Player_Camera.Add("Player_Camera_2", CameraView_2);
		
		CameraView_3.Add(c_null);
		CameraView_3.Add(c_null);
		CameraView_3.Add(c_3_3);
		CameraView_3.Add(c_4_3);
		Player_Camera.Add("Player_Camera_3", CameraView_3);
		
		CameraView_4.Add(c_null);
		CameraView_4.Add(c_null);
		CameraView_4.Add(c_null);
		CameraView_4.Add(c_4_4);
		Player_Camera.Add("Player_Camera_4", CameraView_4);
		
		PlayerCamera_String[0] = "Player_Camera_1";
		PlayerCamera_String[1] = "Player_Camera_2";
		PlayerCamera_String[2] = "Player_Camera_3";
		PlayerCamera_String[3] = "Player_Camera_4";
		
		
		// Hud Camera View
		HudCameraView_1.Add(h_1_1);
		HudCameraView_1.Add(h_2_1);
		HudCameraView_1.Add(h_3_1);
		HudCameraView_1.Add(h_4_1);
		Player_HudCamera.Add("Player_HudCamera_1", HudCameraView_1);
		
		HudCameraView_2.Add(h_null);
		HudCameraView_2.Add(h_2_2);
		HudCameraView_2.Add(h_3_2);
		HudCameraView_2.Add(h_4_2);
		Player_HudCamera.Add("Player_HudCamera_2", HudCameraView_2);
		
		HudCameraView_3.Add(h_null);
		HudCameraView_3.Add(h_null);
		HudCameraView_3.Add(h_3_3);
		HudCameraView_3.Add(h_4_3);
		Player_HudCamera.Add("Player_HudCamera_3", HudCameraView_3);
		
		HudCameraView_4.Add(h_null);
		HudCameraView_4.Add(h_null);
		HudCameraView_4.Add(h_null);
		HudCameraView_4.Add(h_4_4);
		Player_HudCamera.Add("Player_HudCamera_4", HudCameraView_4);
		
		
		Player_HudCamera_String[0] = "Player_HudCamera_1";
		Player_HudCamera_String[1] = "Player_HudCamera_2";
		Player_HudCamera_String[2] = "Player_HudCamera_3";
		Player_HudCamera_String[3] = "Player_HudCamera_4";
		
		// Crosshair Camera View
		CrossHairCoords_1.Add(ch_1_1);
		CrossHairCoords_1.Add(ch_2_1);
		CrossHairCoords_1.Add(ch_3_1);
		CrossHairCoords_1.Add(ch_4_1);
		Player_CrossHair.Add("Player_CrossHair_1", CrossHairCoords_1);
		
		CrossHairCoords_2.Add(ch_null);
		CrossHairCoords_2.Add(ch_2_2);
		CrossHairCoords_2.Add(ch_3_2);
		CrossHairCoords_2.Add(ch_4_2);
		Player_CrossHair.Add("Player_CrossHair_2", CrossHairCoords_2);
		
		CrossHairCoords_3.Add(ch_null);		
		CrossHairCoords_3.Add(ch_null);
		CrossHairCoords_3.Add(ch_3_3);
		CrossHairCoords_3.Add(ch_4_3);
		Player_CrossHair.Add("Player_CrossHair_3", CrossHairCoords_3);
		
		CrossHairCoords_4.Add(ch_1_1);		
		CrossHairCoords_4.Add(ch_1_1);
		CrossHairCoords_4.Add(ch_1_1);
		CrossHairCoords_4.Add(ch_4_4);
		Player_CrossHair.Add("Player_CrossHair_4", CrossHairCoords_4);
		
		Player_CrossHair_String[0] = "Player_CrossHair_1";
		Player_CrossHair_String[1] = "Player_CrossHair_2";
		Player_CrossHair_String[2] = "Player_CrossHair_3";
		Player_CrossHair_String[3] = "Player_CrossHair_4";
	}
	
}
