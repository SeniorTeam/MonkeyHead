using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Note : MonoBehaviour 
{
	#region Declarations
	public enum NoteType
	{
		WHOLE_NOTE = 0,
		HALF_NOTE,
		QUATER_NOTE,
		EIGHTH_NOTE,
		SIXTEENTH_NOTE
	}
	
	public NoteType note;
	public bool isActive;
	public string KeyType;
	
	List<Enemy> enemyList = new List<Enemy>();
	#endregion	
	
	
	#region Trigger Enter/Exit
	void OnTriggerEnter(Collider hit)
	{
		enemyList.Add(hit.gameObject.GetComponent<Enemy>());	
	}
	
	void OnTriggerExit(Collider hit)
	{
		enemyList.Remove(hit.gameObject.GetComponent<Enemy>());	
	}
	#endregion
}


/* GET KEY INPUT
	if (KeyType == key)
		{
			Debug.Log("ffffff");	
		}
		
		if (Input.anyKeyDown)
		{
			key = Input.inputString;
		}
		
/////////////

				
		void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			foreach (Test test in testList)
			{
				Debug.Log(test.name);
			}	
		}
	}
	
	
	void OnTriggerEnter(Collider hit)
	{
		if (hit.gameObject.name == "Cube")
		{
			Debug.Log("hit cube");	
		}
		Debug.Log("Enter");
		testList.Add(hit.gameObject.GetComponent<Test>());	
	}
	
	void OnTriggerExit(Collider hit)
	{
		Debug.Log("Exit");
		testList.Remove(hit.gameObject.GetComponent<Test>());	
	}
	
	
	////////////
	
		void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			foreach (Enemy _enemy in enemyList)
			{
				Debug.Log(_enemy.name);
				Debug.Log(_enemy.Health);
				_enemy.Health -= 1;
				Debug.Log(_enemy.Health);
			}	
		}
		
	}
	void OnTriggerEnter(Collider hit)
	{
		if (hit.gameObject.name == "Cube")
		{
			Debug.Log("hit cube");	
		}
		Debug.Log("Enter");
		enemyList.Add(hit.gameObject.GetComponent<Enemy>());	
	}
 */