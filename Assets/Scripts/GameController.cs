using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static bool gameOver;
	// Use this for initialization
	void Start () {
		gameOver = false;
		Grid.ClearGrid ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.R) && gameOver) 
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}



}
