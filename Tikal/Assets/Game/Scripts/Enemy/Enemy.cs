using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Seeker))]
public class Enemy: AIPath 
{	
	#region Declaration
	[SerializeField] Game        c_Game;
	[SerializeField] Enemy_Sight c_Sight;
	
	/** Minimum velocity for moving */
	public float sleepVelocity = 0.4F;
	
	/** Effect which will be instantiated when end of path is reached.
	 * \see OnTargetReached */
	public GameObject endOfPathEffect;
	/** Point for the last spawn of #endOfPathEffect */
	protected Vector3 lastTarget;
	
	#endregion
		
	#region Initializer;

	public new void Start () 
	{
		canMove = false;
		c_Sight.Initialize();
		//Call Start in base script (AIPath)
		base.Start ();
	}
	
	
	#endregion
	
	protected new void FixedUpdate () 
	{
		if (c_Game.StartGame)
		{
			c_Sight.UpdateFunction();
			CanEnemyMove();
			
			AttachTarget();
		}
		
		MoveTowardsTarget();
	}
	
	void CanEnemyMove()
	{
		if (c_Sight.CanEnemyMove())
			canMove = true;
		else
			canMove = false;
	}
	
	/**
	 * Called when the end of path has been reached.
	 * An effect (#endOfPathEffect) is spawned when this function is called
	 * However, since paths are recalculated quite often, we only spawn the effect
	 * when the current position is some distance away from the previous spawn-point
	*/
	public override void OnTargetReached () 
	{
		if (endOfPathEffect != null && Vector3.Distance (tr.position, lastTarget) > 1) 
		{
			GameObject.Instantiate (endOfPathEffect,tr.position,tr.rotation);
			lastTarget = tr.position;
		}
	}
	
	public override Vector3 GetFeetPosition ()
	{	
		return tr.position;
	}
	
	void MoveTowardsTarget()
	{
		//Get velocity in world-space
		Vector3 velocity;
		
		if (canMove) 
		{
			//Calculate desired velocity
			Vector3 dir = CalculateVelocity (GetFeetPosition());
			
			//Rotate towards targetDirection (filled in by CalculateVelocity)
			if (targetDirection != Vector3.zero) 
			{
				RotateTowards (targetDirection);
			}
			
			if (dir.sqrMagnitude > sleepVelocity*sleepVelocity) 
			{
				//If the velocity is large enough, move
			} 
			else 
			{
				//Otherwise, just stand still (this ensures gravity is applied)
				dir = Vector3.zero;
			}
			
			if (navController != null)
				navController.SimpleMove (GetFeetPosition(), dir);
			else if (controller != null)
				controller.SimpleMove (dir);
			else
				Debug.LogWarning ("No NavmeshController or CharacterController attached to GameObject");
			
			velocity = controller.velocity;
		} 
		else 
		{
			velocity = Vector3.zero;
		}		
	}
	
	protected void AttachTarget ()
	{
		if (c_Sight.TargetPlayer != null)
			target = c_Sight.TargetPlayer.transform;
	}
}
