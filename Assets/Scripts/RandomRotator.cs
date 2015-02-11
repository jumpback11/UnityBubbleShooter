using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {

	public float tumble;

	// Use this for initialization
	void Start () {
		transform.rotation = Quaternion.Euler(Random.insideUnitSphere * tumble);
	}

}
