using UnityEngine;
using System.Collections;

public class GeneratedBallBehavior : MonoBehaviour {

	// this call is the same as the active ball class, excluding all calls regarding moving
	private int gridX, gridZ;
	private Color color;
	private bool isAnchored, onGrid;
	
	// Use this for initialization
	void Start () 
	{
		Initialize ();
	}
	
	void FixedUpdate()
	{
				
		// pops the ball if it is flagged
		if ( onGrid && Grid.colorGrid [gridX, gridZ] == Color.clear)
			kill ();
		// sets the bool of anchored property from the grid
		if ( onGrid)
			isAnchored = Grid.anchoredGrid[gridX, gridZ];
		// if the ball is not anchored, pop it
		if ( onGrid && !isAnchored && Grid.checkAnchorDone)
			kill ();
	}
	
	void Initialize()
	{
		// tag the ball
		gameObject.tag = "Ball";
		// updates position variables
		gridX = (int)transform.position.x;
		gridZ = (int)transform.position.z;
		// captures the color of the current ball in the color variable
		color = transform.renderer.material.color;
		// updates the values of the grid to represent this ball 
		Grid.colorGrid [gridX, gridZ] = color;
		Grid.grid [gridX, gridZ] = transform;
		Grid.anchoredGrid[gridX, gridZ] = true;
		// sets this balls values for being on grid and anchored
		onGrid = true;
		isAnchored = true;

	}
	
	/*
	 * Method that calls all other functions that have to do with killing this bubble
	 */
	public void kill(){
		// stop all coroutines
		StopAllCoroutines();
		// start the kill routine
		StartCoroutine(ScaleTo(0.15f));
	}
	
	/*
	 * coroutine that scales the ball down to 0 then destroys the gameobject
	 * @param {float} speed the scale is changed
	 */
	IEnumerator ScaleTo(float duration) {
		
		float timeThrough = 0.0f;
		Vector3 scale = new Vector3 (0, 0, 0);
		Vector3 initialScale = transform.localScale;
		
		while (transform.localScale.x >= 0.1){
			timeThrough += Time.deltaTime;
			Vector3 target = Vector3.Lerp(initialScale, scale, timeThrough / duration);
			transform.localScale = target;
			yield return null;
		}
		if (transform.localScale.x <= 0.1){			
			Destroy(transform.rigidbody);
			Destroy(transform.collider);
			Grid.grid [gridX, gridZ] = null;
			Grid.anchoredGrid [gridX, gridZ] = false;
			Grid.colorGrid [gridX, gridZ] = Color.clear;
			Grid.CAStarter();
			Destroy (this.gameObject);
		}
	}
}
