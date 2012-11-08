using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]

public class MouseLook : MonoBehaviour 
{
	[SerializeField] Game c_Game;
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;
	
	[SerializeField] Camera EyeLevel;
	float sightDistance = 0.8f;
	[SerializeField] string[] ActiveTags;

	void Update ()
	{
		if (c_Game.StartGame)
		{
			CameraKeyBoard();
			ActiveObject();
		}
	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
	
	void CameraController()
	{
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Camera_Horizontal") * sensitivityX;
			
			rotationY += Input.GetAxis("Camera_Vertical") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Camera_Horizontal") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Camera_Vertical") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}
	
	void CameraKeyBoard()
	{
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}
	
	void ActiveObject()
	{
		Vector3 forward = EyeLevel.transform.TransformDirection(Vector3.forward) * sightDistance;
		Ray ray = new Ray(EyeLevel.transform.position, forward);
		RaycastHit hit;
		Transform found = null;
		bool active = false;
		
		if ( Physics.Raycast(ray, out hit, sightDistance) )
		{
		   found = hit.transform;
			
			// If hit target
			if (found != null)
			{
				foreach (string _tag in ActiveTags)
				{
					if (found.tag == _tag)
						active = true;
				}
				
				if (active)
				{
					Vector3 pos = found.position;
					
					if ( isPlayerInRange(pos.y, DistanceForm(pos.x, pos.z), sightDistance) )
					{
						Debug.Log (hit.transform.name + " is ACTIVE");
					}
					else
					{
						Debug.Log (hit.transform.name + " is not in range");
					}
				}
				
			}
			
		}
		// Debug Players eyesight
		Debug.DrawRay (EyeLevel.transform.position, forward, Color.red);
		
	}
	
	bool isPlayerInRange(float y, float xz, float distance)
	{
		bool inRange = true;
		
		if ( y > (Mathf.Abs(transform.position.y) + distance) )
			inRange = false;
		if (xz > distance)
			inRange = false;
					
		return inRange;					
	}
				
	private float DistanceForm (float player_x, float player_z)
	{
		float x1 = player_x;
		float x2 = transform.position.x;
		float y1 = player_z;
		float y2 = transform.position.z;
		
		float distance;
		
		float xFactor = Mathf.Pow( (x2 - x1), 2);
		float yFactor = Mathf.Pow( (y2 - y1), 2);
		
		distance = Mathf.Sqrt( (xFactor + yFactor) );
		distance = Mathf.Abs(distance);
		
		return distance;
	}
}