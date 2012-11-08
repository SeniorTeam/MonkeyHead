using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Character/Character Motor")]


public class CharacterMotor : MonoBehaviour
{
	Game c_Game;
	GameObject _Game;
	
    // Does this script currently respond to input?
    public bool canControl = true;
    public bool useFixedUpdate = true;

    // For the next variables, [System.NonSerialized] tells Unity to not serialize the variable or show it in the inspector view.
    // Very handy for organization!

    // The current global direction we want the character to move in.
    [System.NonSerialized]
    public Vector3 inputMoveDirection = Vector3.zero;

    [System.Serializable]
    public class CharacterMotorMovement
    {
        // The maximum horizontal speed when moving
        public float maxForwardSpeed = 10.0f;
        public float maxSidewaysSpeed = 10.0f;
        public float maxBackwardsSpeed = 9.5f;

        // How fast does the character change speeds?  Higher is faster.
        public float maxGroundAcceleration = 20.0f;
        public float maxAirAcceleration = 11.0f;

        // The gravity for the character
        public float gravity = 10.0f;
        public float maxFallSpeed = 10.0f;

        // For the next variables, [System.NonSerialized] tells Unity to not serialize the variable or show it in the inspector view.
        // Very handy for organization!

        // The last collision flags returned from controller.Move
        [System.NonSerialized]
        public CollisionFlags collisionFlags;

        // We will keep track of the character's current velocity,
        [System.NonSerialized]
        public Vector3 velocity;

        // This keeps track of our current velocity while we're not grounded
        [System.NonSerialized]
        public Vector3 frameVelocity = Vector3.zero;

        [System.NonSerialized]
        public Vector3 hitPoint = Vector3.zero;

        [System.NonSerialized]
        public Vector3 lastHitPoint = new Vector3(Mathf.Infinity, 0, 0);
    }

    public CharacterMotorMovement movement = new CharacterMotorMovement();
	
    [System.NonSerialized]
    public bool grounded = true;

    [System.NonSerialized]
    public Vector3 groundNormal = Vector3.zero;

    private Vector3 lastGroundNormal = Vector3.zero;

    private Transform tr;

    private CharacterController controller;

    void Awake()
    {
		_Game = GameObject.Find("Game");
		c_Game = _Game.GetComponentInChildren<Game>();
		
        controller = GetComponent<CharacterController>();
        tr = transform;
		StartCoroutine(SetSpeedMultiplier());
    }

	IEnumerator SetSpeedMultiplier ()
	{
		while (!c_Game.StartGame)
		{
			yield return null;	
		}

		yield return null;
	}

    private void UpdateFunction()
    {
        // We copy the actual velocity into a temporary variable that we can manipulate.
        Vector3 velocity = movement.velocity;

        // Update velocity based on input
        velocity = ApplyInputVelocityChange(velocity);
		
		// Apply gravity and jumping force
		velocity = ApplyGravityAndJumping (velocity);

        // Save lastPosition for velocity calculation.
        Vector3 lastPosition = tr.position;

        // We always want the movement to be framerate independent.  Multiplying by Time.deltaTime does this.
        Vector3 currentMovementOffset = velocity * Time.deltaTime;

        // Find out how much we need to push towards the ground to avoid loosing grouning
        // when walking down a step or over a sharp change in slope.
        float pushDownOffset = Mathf.Max(controller.stepOffset, new Vector3(currentMovementOffset.x, 0, currentMovementOffset.z).magnitude);
        if(grounded)
            currentMovementOffset -= pushDownOffset * Vector3.up;

        groundNormal = Vector3.zero;

        // Move our character!
        movement.collisionFlags = controller.Move(currentMovementOffset);

        movement.lastHitPoint = movement.hitPoint;
        lastGroundNormal = groundNormal;

        // Calculate the velocity based on the current and previous position.  
        // This means our velocity will only be the amount the character actually moved as a result of collisions.
        Vector3 oldHVelocity = new Vector3(velocity.x, 0, velocity.z);
        movement.velocity = (tr.position - lastPosition) / Time.deltaTime;
        Vector3 newHVelocity = new Vector3(movement.velocity.x, 0, movement.velocity.z);

        // The CharacterController can be moved in unwanted directions when colliding with things.
        // We want to prevent this from influencing the recorded velocity.
        if(oldHVelocity == Vector3.zero)
        {
            movement.velocity = new Vector3(0, movement.velocity.y, 0);
        }
        else
        {
            float projectedNewVelocity = Vector3.Dot(newHVelocity, oldHVelocity) / oldHVelocity.sqrMagnitude;
            movement.velocity = oldHVelocity * Mathf.Clamp01(projectedNewVelocity) + movement.velocity.y * Vector3.up;
        }
		
		if (movement.velocity.y < velocity.y - 0.001f) 
		{
			if (movement.velocity.y < 0) 
			{
				// Something is forcing the CharacterController down faster than it should.
				// Ignore this
				movement.velocity.y = velocity.y;
			}
		}
		
       // We were grounded but just loosed grounding
		if (grounded && !IsGroundedTest()) {
			grounded = false;
			
			tr.position += pushDownOffset * Vector3.up;
		}
		// We were not grounded but just landed on something
		else if (!grounded && IsGroundedTest()) {
			grounded = true;
			
			SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);
		}
    }

    void FixedUpdate()
    {
		if (c_Game.StartGame)
		{
	        if(useFixedUpdate)
	            UpdateFunction();
		}
    }

    void Update()
    {
		if (c_Game.StartGame)
		{
	        if(!useFixedUpdate)
	            UpdateFunction();
		}
    }

    private Vector3 ApplyInputVelocityChange(Vector3 velocity)
    {
        if(!canControl)
            inputMoveDirection = Vector3.zero;

        // Find desired velocity
        Vector3 desiredVelocity;
       
        desiredVelocity = GetDesiredHorizontalVelocity();

        if(grounded)
            desiredVelocity = AdjustGroundVelocityToNormal(desiredVelocity, groundNormal);
        else
            velocity.y = 0;

        // Enforce max velocity change
        float maxVelocityChange = GetMaxAcceleration(grounded) * Time.deltaTime;
        Vector3 velocityChangeVector = (desiredVelocity - velocity);
        if(velocityChangeVector.sqrMagnitude > maxVelocityChange * maxVelocityChange)
        {
            velocityChangeVector = velocityChangeVector.normalized * maxVelocityChange;
        }
        // ifwe're in the air and don't have control, don't apply any velocity change at all.
        // ifwe're on the ground and don't have control we do apply it - it will correspond to friction.
        if(grounded || canControl)
            velocity += velocityChangeVector;

        if(grounded)
        {
            // When going uphill, the CharacterController will automatically move up by the needed amount.
            // Not moving it upwards manually prevent risk of lifting off from the ground.
            // When going downhill, DO move down manually, as gravity is not enough on steep hills.
            velocity.y = Mathf.Min(velocity.y, 0);
        }

        return velocity;
    }
	
	private Vector3 ApplyGravityAndJumping( Vector3 velocity) 
	{
	
	
		if (grounded)
			velocity.y = Mathf.Min(0, velocity.y) - movement.gravity * Time.deltaTime;
		else
		{
			velocity.y = movement.velocity.y - movement.gravity * Time.deltaTime;
			
			// Make sure we don't fall any faster than maxFallSpeed. This gives our character a terminal velocity.
			velocity.y = Mathf.Max (velocity.y, -movement.maxFallSpeed);
		}
		
		return velocity;
	}
	
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.normal.y > 0 && hit.normal.y > groundNormal.y && hit.moveDirection.y < 0)
        {
            if((hit.point - movement.lastHitPoint).sqrMagnitude > 0.001f || lastGroundNormal == Vector3.zero)
                groundNormal = hit.normal;
            else
                groundNormal = lastGroundNormal;

            movement.hitPoint = hit.point;
            movement.frameVelocity = Vector3.zero;
        }
    }

    private Vector3 GetDesiredHorizontalVelocity()
    {
        // Find desired velocity
        Vector3 desiredLocalDirection = tr.InverseTransformDirection(inputMoveDirection);
        float maxSpeed = MaxSpeedInDirection(desiredLocalDirection);	
        return tr.TransformDirection(desiredLocalDirection * maxSpeed);
    }

    private Vector3 AdjustGroundVelocityToNormal(Vector3 hVelocity, Vector3 groundNormal)
    {
        Vector3 sideways = Vector3.Cross(Vector3.up, hVelocity);
        return Vector3.Cross(sideways, groundNormal).normalized * hVelocity.magnitude;
    }

    private bool IsGroundedTest()
    {
        return (groundNormal.y > 0.01f);
    }

    public float GetMaxAcceleration(bool grounded)
    {
        // Maximum acceleration on ground and in air
        if(grounded)
            return movement.maxGroundAcceleration;
        else
            return movement.maxAirAcceleration;
    }

    public bool IsTouchingCeiling()
    {
        return (movement.collisionFlags & CollisionFlags.CollidedAbove) != 0;
    }

    public bool IsGrounded()
    {
        return grounded;
    }
	
    public Vector3 GetDirection()
    {
        return inputMoveDirection;
    }

    void SetControllable(bool controllable)
    {
        canControl = controllable;
    }

    // Project a direction onto elliptical quater segments based on forward, sideways, and backwards speed.
    // The function returns the length of the resulting vector.
    public float MaxSpeedInDirection(Vector3 desiredMovementDirection)
    {
        if(desiredMovementDirection == Vector3.zero)
            return 0;
        else
        {
            float zAxisEllipseMultiplier = (desiredMovementDirection.z > 0 ? movement.maxForwardSpeed : movement.maxBackwardsSpeed) / movement.maxSidewaysSpeed;
            Vector3 temp = new Vector3(desiredMovementDirection.x, 0, desiredMovementDirection.z / zAxisEllipseMultiplier).normalized;
            float length = new Vector3(temp.x, 0, temp.z * zAxisEllipseMultiplier).magnitude * movement.maxSidewaysSpeed;
            return length;
        }
    }

    void SetVelocity(Vector3 velocity)
    {
        grounded = false;
        movement.velocity = velocity;
        movement.frameVelocity = Vector3.zero;
        SendMessage("OnExternalVelocity");
    }
}