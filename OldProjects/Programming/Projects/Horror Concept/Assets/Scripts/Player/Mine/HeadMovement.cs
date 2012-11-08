using UnityEngine;
using System.Collections;

public class HeadMovement : MonoBehaviour 
{
	void Update()
	{
		MouseInput();
		XboxInput();
	}

	void MouseInput ()
	{
		
	}

	void XboxInput ()
	{
		Vector3 pos = transform.position;
		//Vector3 rot = transform.eulerAngles;
		
		Debug.Log(Input.GetAxis("Camera_Horizontal") + "  " + Input.GetAxis("Camera_Verical"));
		
		// Player Movement
		pos.x += LeftRightSpeed;
		pos.z += ForwardBackwardSpeed;
		
		// Camera Movement
		//rot.x += UpDownCamera;
		//rot.y += LeftRightCamera;
		
		
		transform.position = pos;
		
		Vector3 mouseScreenPos = Input.mousePosition;
    	Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

    	mouseWorldPos.z = transform.position.z;

    	transform.LookAt(mouseWorldPos,Vector3.forward); 
		
		//transform.LookAt(rot,Vector3.forward);
		//transform.eulerAngles = rot;
	
		//Debug.Log(Input.GetAxis("Mouse/Left_X") + " :   " +pos);
	}
	
	
	#region Player Movement ->left/right/up/down
	[SerializeField] private float leftRightSpeed = 2.0f;
	public float LeftRightSpeed 
	{
		get { return this.leftRightSpeed * Input.GetAxis("Movement_Horizontal"); }
		set { this.leftRightSpeed = value; }
	}
	
	[SerializeField] private float forwardBackwardSpeed = 2.0f;
	public float ForwardBackwardSpeed 
	{
		get { return this.forwardBackwardSpeed * Input.GetAxis("Movement_Vertical"); }
		set { this.forwardBackwardSpeed = value; }
	}
	#endregion
	
	#region Camera Movement ->left/right/up/down
	[SerializeField] private float leftRightCamera = 2.0f;
	public float LeftRightCamera 
	{
		get { return this.leftRightCamera * Input.GetAxis("Camera_Horizontal"); }
		set { this.leftRightCamera = value; }
	}
	
	[SerializeField] private float upDownCamera = 2.0f;
	public float UpDownCamera
	{
		get { 
			
				float cameraAngle = this.upDownCamera * Input.GetAxis("Camera_Verical");
				
				float actualAngle = cameraAngle;
			
				return actualAngle; 
			}
		set { this.upDownCamera = value; }
	}
	#endregion
}
