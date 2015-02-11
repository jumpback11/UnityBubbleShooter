using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

	public GameObject[] balls;
	private int spawnInt;
	public int ballCount;
	Vector3 v;

	// Use this for initialization
	void Start () {
		Grid.ClearGrid ();
		Debug.Log (ballCount);
		SpawnMap ();

	}
	
	private int SpawnGenerator()
	{
		// Random Index
		int i = Random.Range(0, ballCount);
		
		return i;
	}

	/*
	 * generates a map of balls including the requested number of colors
	 */
	private void SpawnMap()
	{
		for (float i = 9; i <= 17; i++){
			for (float j = 0; j <= 12; j++){
				Debug.Log(i + ", " + j);
				if (i % 2 == 0){
					if (j != 12){
						float k = j + 0.5f;
						v = new Vector3(k, 0.0f, i);
						spawnInt = SpawnGenerator();
						Instantiate(balls[spawnInt], v, Quaternion.Euler(Random.insideUnitSphere));
					}else{}
				}else{
					v = new Vector3(j, 0.0f, i);
					spawnInt = SpawnGenerator();
					Instantiate(balls[spawnInt], v, Quaternion.Euler(Random.insideUnitSphere));
				}
			}
		}
	}
}
