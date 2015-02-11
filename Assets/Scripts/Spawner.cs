using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject[] balls;
	public static bool spawnNext;
	private int spawnInt;

	// Use this for initialization
	void Start () {
		// spawns the first ball of the game
		spawnInt = SpawnGenerator();
		Instantiate(balls[spawnInt], transform.position, transform.rotation);
		spawnNext = false;
	}
	
	// Update is called once per frame
	void Update () {
		// if okay to spawn, spawns the next ball
		if (spawnNext){
			spawnInt = SpawnGenerator();
			Instantiate(balls[spawnInt], transform.position, transform.rotation);
			spawnNext = false;
		}	
		// removes the ball from the launcher group
		if (Input.GetButtonDown ("Fire1")) 
			transform.DetachChildren ();			
	}

	private int SpawnGenerator()
	{
		// Random Index
		int i = Random.Range(0, balls.Length);
		
		return i;
	}
}
