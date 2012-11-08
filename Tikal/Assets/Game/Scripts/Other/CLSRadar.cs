using UnityEngine;
using System.Collections;

public class CLSRadar : MonoBehaviour
{
	Game c_Game;
	GameObject _Game;	
	
	public string Player;
    public Texture blip;
    public Texture radarBG;
    public GameObject centerObject;
    public float mapScale = 0.5f;
    public Vector2 mapCenter;

    public string tagFilter = "Player";
    public float maxDist = 50;
	
	bool CanDraw = false;
	
	void Awake()
	{
		_Game = GameObject.Find("Game");
		c_Game = _Game.GetComponentInChildren<Game>();
		StartCoroutine(Wait());
	}
	
	IEnumerator Wait()
	{
		while (!c_Game.StartGame)
		{
			yield return null;	
		}
		
		yield return new WaitForSeconds(0.1f);
		
		centerObject = GameObject.Find( Player );
		Debug.Log(Player);
		CanDraw = true;
	}

	public void OnGUI()
    {
		if (CanDraw)
		{
	       // Draw player blip (centerObject)      
	       // GUI.DrawTexture( new Rect( 10, (Screen.height - radarBG.height), radarBG.width/2, radarBG.height/2), radarBG );
	        mapCenter = new Vector2( radarBG.width / 2, Screen.height - ( radarBG.height / 2 ) );
	    
			DrawBlipsFor( tagFilter );
		}
    }
 
    private void DrawBlipsFor( string tagName )
    {
		// Find all game objects with tag 
		GameObject[] gos = GameObject.FindGameObjectsWithTag( tagName );
		
		// Iterate through them
		foreach ( GameObject go in gos )
			drawBlip( go, blip );
		
    }
 
    private void drawBlip( GameObject go, Texture aTexture )
    {	
	    Vector3 centerPos = centerObject.transform.position;
	    Vector3 extPos = go.transform.position;
	 
	    // first we need to get the distance of the enemy from the player
	    float dist = Vector3.Distance( centerPos, extPos );
	 
	    float dx = centerPos.x - extPos.x; // how far to the side of the player is the enemy?
		float dz = centerPos.z - extPos.z; // how far in front or behind the player is the enemy?
	 
	    // what's the angle to turn to face the enemy - compensating for the player's turning?
	    float deltay = Mathf.Atan2( dx, dz ) * Mathf.Rad2Deg - 270 - centerObject.transform.eulerAngles.y;
	 
	    // just basic trigonometry to find the point x,y (enemy's location) given the angle deltay
	    float bX = dist * Mathf.Cos( deltay * Mathf.Deg2Rad );
	    float bY = dist * Mathf.Sin( deltay * Mathf.Deg2Rad );
	 
	    bX = bX * mapScale; // scales down the x-coordinate by half so that the plot stays within our radar
	    bY = bY * mapScale; // scales down the y-coordinate by half so that the plot stays within our radar
	 
     	if ( dist <= maxDist )
	    {
			// this is the diameter of our largest radar circle
			GUI.DrawTexture( new Rect( mapCenter.x + bX, mapCenter.y + bY, 2, 2 ), aTexture );
	    }
		
    }
	
}