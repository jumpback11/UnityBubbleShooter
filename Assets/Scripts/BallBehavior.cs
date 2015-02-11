using UnityEngine;
using System.Collections;

public class BallBehavior : MonoBehaviour {

	private Transform launcher, player;
	private SphereCollider myCollider;
	private Rigidbody myRigid;
	private Vector3 previousPosition, currentPosition;
	private int gridX, gridZ;
	private Color color;
	private bool onGrid, isAnchored, isMoving;

	// Use this for initialization
	void Start () 
	{
		Initialize ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// sends the command to move the ball forward
		if ( Input.GetButtonDown ("Fire1") && !isMoving && !GameController.gameOver) {
			// ball moving
			isMoving = true;
			// move the ball
			Move ();
			// enable collider
			myCollider.enabled = true;
			//resets the grid check to elminate issues
			Grid.ResetChecked();		
		}



	}

	void FixedUpdate()
	{
		//updates current position
		UpdatePosition (transform.position);
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

		// Starts Ball as not moving
		isMoving = false;
		// sets up ball to be child of the launcher
		launcher = (GameObject.Find("Spawner")).transform;
		transform.parent = launcher;
		// get player for shooting angle
		player = (GameObject.Find ("Player")).transform;
		//create variable to hold the collider instance
		myCollider = GetComponent<SphereCollider>();
		//disable collider
		myCollider.enabled = false;
		// create variable to hold rigidbody instance
		myRigid = GetComponent<Rigidbody> ();
		// captures the color of the current ball in the color variable
		color = transform.renderer.material.color;
		//sets the ball as unanchored
		isAnchored = false;
		// checks ball color against grid, and destroys if color is not on grid.
		Changeball ();
	}

	/*
	 * If ball color is not on this level estroy and generate another ball.
	 */
	void Changeball()
	{
		if ( !Grid.colorArray.Contains (color)){
			// call next spawn
			Spawner.spawnNext = true;
			// destroy this ball
			Destroy(gameObject);
		}
	}

	/*
	 * Sets the ball in motion in the direction the shooter is facing.
	 */
	void Move ()
	{
		rigidbody.AddRelativeForce (player.position * 10);

	}

	/*
	 * checks if the ball has been shot and then updates the current position if there is no collisions
	 */
	void UpdatePosition(Vector3 v)
	{
		if (isMoving){
			// if ball is still moving update previous position
			previousPosition = currentPosition;
			
			// update new current position
			if (v.x >= 11.5f){
				if(v.z % 2 !=0)
					currentPosition.x = 12;
				else
					currentPosition.x = 11.5f;
				
				currentPosition.z = v.z;
			}else if (v.x <= 0.5f){
				if(v.z % 2 !=0)
					currentPosition.x = 0;
				else
					currentPosition.x = 0.5f;
				
				currentPosition.z = v.z;
			}else{
				currentPosition = v;
			}
			
		}
		
	}

	/*
	 * collision detection metthod
	 */
	void OnTriggerEnter (Collider collider)
	{
		//stick if collider is not side wall
		if (collider.tag != ("Boundary") && !onGrid){
			// set ball position on grid
			SetPosition();
			// make the balls physics inactive
			Destroy (rigidbody);
			// tag the ball
			gameObject.tag = "Ball";
			// activate trigger
			myCollider.isTrigger = true;
			// call next spawn
			Spawner.spawnNext = true;
			// gameover?
			if(transform.position.z < 1)
				GameController.gameOver = true;	

		}
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
	 * Method that updates the grid and establishes on grid and anchored
	 */
	void UpdateGrid () {

		// creates an offset for even rows
		if (gridZ % 2 == 0)
			gridX -= 1;
		// updates the values of the grid to represent this ball 
		Grid.grid[gridX, gridZ] = transform;
		Grid.colorGrid[gridX, gridZ] = color;
		Grid.anchoredGrid[gridX, gridZ] = true;
		// sets this balls values for being on grid and anchored
		onGrid = true;
		isAnchored = true;
		//starts the reaction to check similar colors attached to this ball
		Grid.ChainReaction (gridX, gridZ, color);

	}

	/*
	 * Method to set the location of the ball into a grid with an offset grid every even row
	 */
	void SetPosition() {
		// makes the previous position raounded for better positioning on the grid.
		Vector3 position = RoundVector(previousPosition);
		// if row is even offset the x position
		if (position.z % 2 == 0 ){
			// if row is even and x is greater than 11 so we dont have a ball in the wall
			if (position.x >= 11)
				position.x = 11;
			// math to get a rounded x.5 decimal
			position.x = (Mathf.FloorToInt(transform.position.x * 10)) / 5;
			position.x = (position.x * 5);

			if (position.x % 2 == 0){
				// if math is even add 0.5 to adjust for rounding
				position = new Vector3 (position.x / 10 + 0.5f, 0.0f, position.z);
				gridX = Mathf.CeilToInt(position.x);
				gridZ = (int)position.z;
			}else{
				// else we have a x.5 already
				position = new Vector3 (position.x / 10, 0.0f, position.z);
				gridX = Mathf.CeilToInt(position.x);
				gridZ = (int)position.z;
			}
			// if row is even and x is greater than 11.5 so we dont have a ball in the wall
			if (position.x >= 11.5f)
				position.x = 11.5f;
		}else{
			// these are the values if the row is not even
			gridX = (int)position.x;
			gridZ = (int)position.z;
		}
		// move ball into position 
		transform.position = position;
		// add bubble to grid
		UpdateGrid();

	}

	/*
	 * Function that rounds Vector3 to int
	 * @param {Vector3} Vector3 that you want to round
	 */
	Vector3 RoundVector(Vector3 v) {

		return new Vector3 (Mathf.RoundToInt(v.x), 0.0f, Mathf.RoundToInt (v.z));
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
