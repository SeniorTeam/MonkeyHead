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
	Game c_Game;
	GameObject _Game;
	CrossHair c_CrossHair;
	GameObject _CrossHair;
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;
	
	float sightDistance = 25.0f; // overall view (dont want too far so we dont have to make as many calc)
	float enemySight = 20.0f;
	float allySight = 20.0f;
	float objectSight = 20.0f;
	
	bool _active;
	
	[SerializeField] Camera EyeLevel;
	[SerializeField] string[] ActiveTags;
	[SerializeField] string[] AllyTags;
	[SerializeField] string[] EnemyTags;
	
	
	public string Horizontal;
	public string Vertical;
	
	bool initComplete;
	
	
	
	void Awake()
	{
		_Game = GameObject.Find("Game");
		c_Game = _Game.GetComponentInChildren<Game>();
	
		StartCoroutine(Init());
	}
	
	IEnumerator Init()
	{
		string hud;
		yield return new WaitForSeconds(0.1f);
		hud = transform.GetComponentInChildren<Player_Info>().PlayerHUD.ToString();
		
		_CrossHair = GameObject.Find(hud);
		
		foreach (Transform obj in _CrossHair.transform)
		{
			if (obj.tag == "CrossHair")
				c_CrossHair = _CrossHair.GetComponentInChildren<CrossHair>();
			yield return null;
		}
		
		initComplete = true;
		yield return null;
	}

	void Update ()
	{
		if (c_Game.StartGame && initComplete)
		{
			if (gameObject.GetComponent<Player_Info>().ControllerUse)
			{
				CameraController();
				CrossHair();
			}
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
			float rotationX = transform.localEulerAngles.y + Input.GetAxis(Horizontal) * sensitivityX;
			
			rotationY += Input.GetAxis(Vertical) * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis(Horizontal) * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis(Vertical) * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}
	void CrossHair()
	{
		Vector3 forward = EyeLevel.transform.TransformDirection(Vector3.forward) * sightDistance;
		Ray ray = new Ray(EyeLevel.transform.position, forward);
		RaycastHit hit;
		Transform found = null;
		_active = false;
		
		c_CrossHair.ResetCrossHair();
		
		if ( Physics.Raycast(ray, out hit, enemySight) )
		{
		   found = hit.transform;
			
			// If hit target
			if (found != null)
			{
				
				if ( CrossHairCheck (found, EnemyTags) )
				{
					if ( isObjectClose(found.position, enemySight) )
					{
						c_CrossHair.ChangeCrossHair("enemy");
					}
					else
						c_CrossHair.ResetCrossHair();
		
				}
				else if ( CrossHairCheck (found, AllyTags) )
				{
					if ( isObjectClose(found.position, allySight) )
					{
						c_CrossHair.ChangeCrossHair("ally");
					}
					else
						c_CrossHair.ResetCrossHair();
				}
					
				else if( CrossHairCheck (found, ActiveTags) )
				{
					if ( isObjectClose(found.position, objectSight) )
					{
						c_CrossHair.ChangeCrossHair("object");
					}
					else
						c_CrossHair.ResetCrossHair();
				}
			
			}
			
		}
		// Debug Players eyesight
		Debug.DrawRay (EyeLevel.transform.position, forward, Color.red);
		
	}
	
	bool CrossHairCheck(Transform found, string[] Tags)
	{
		foreach (string _tag in Tags)
		{
			if (found.tag == _tag)
				_active = true;
		}
		
		if (_active)
			return true;
		else
			return false;
	}
	bool isObjectClose(Vector3 pos, float sight)
	{
		if ( isPlayerInRange(pos.y, DistanceForm(pos.x, pos.z), sight) )
			return true;
		else
			return false;
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