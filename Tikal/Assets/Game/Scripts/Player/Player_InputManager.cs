using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]

public class Player_InputManager : MonoBehaviour 
{
	// Require a character controller to be attached to the same game object
	private CharacterMotor motor;
	Game c_Game;
	GameObject _Game;
	
	public string Horizontal;
	public string Vertical;
	
	
	// Use this for initialization
	void Awake () 
	{
		_Game = GameObject.Find("Game");
		c_Game = _Game.GetComponentInChildren<Game>();
		
		motor = GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (c_Game.StartGame)
		{
			if (GetComponent<Player_Info>().ControllerUse)
			{
				// Get the input vector from kayboard or analog stick
				PlayerMovement_Controller();
			}
			else
			{
				motor.inputMoveDirection = new Vector3(0,0,0);
			}
		}
	}

	void PlayerMovement_Controller ()
	{
		Vector3 directionVector = new Vector3(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical));
		
		if (directionVector != Vector3.zero) 
		{
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			float directionLength = directionVector.magnitude;
			directionVector = directionVector / directionLength;
			
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min(1, directionLength);
			
			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
			
			// Multiply the normalized direction vector by the modified length
			directionVector = directionVector * directionLength;
		}
		
		// Apply the direction to the CharacterMotor
		motor.inputMoveDirection = transform.rotation * directionVector;
	}
}

