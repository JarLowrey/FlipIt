using UnityEngine;
using System.Collections;

public class MovePlatform : MonoBehaviour {
	private float currentDistanceMoved, elapsedDelay;
	private float numberOfMovementCycles = 0;
	private bool movePlayerWithThisPlatform, delayFinished, waitForDelay;
	public float distanceUntilChangeDirection, speed,SecondsOfDelayBetweenMovementCycle;
	public Vector3 direction;
	
	// Use this for initialization
	void Start () {
		if (direction == null) {
			Debug.Log ("Direction of platform is null!");
		} 
		if (Mathf.Approximately (distanceUntilChangeDirection, 0)) {
			Debug.Log ("Distance to move platform is zero!");
		}
		if (Mathf.Approximately (speed, 0)) {
			Debug.Log ("Platform movement speed is zero!");
		}
		
		direction.Normalize ();//ensure that direction does not impact the speed
		movePlayerWithThisPlatform = false;
		waitForDelay = true; //need to set to true for delay to apply on the first movement cycle
	}
	
	// Update is called once per frame
	void Update () {
		MovePlatformAndCheckForDelay ();

		//move main player along with the moving platform
		if (movePlayerWithThisPlatform) {
			GameObject.Find("Dude").transform.position += getMovementDirection ();
		}
	}

	private void MovePlatformAndCheckForDelay(){
		if (waitForDelay) {
			elapsedDelay += Time.deltaTime;
			if(elapsedDelay > SecondsOfDelayBetweenMovementCycle){
				waitForDelay = false;
				elapsedDelay = 0;
				movePlatform();
			}
		} else {
			movePlatform ();
		}
	}

	private void movePlatform(){
		Vector3 movement  = getMovementDirection ();

		if (currentDistanceMoved < distanceUntilChangeDirection) {
			transform.position += movement;
			currentDistanceMoved += movement.magnitude;
		} else {
			currentDistanceMoved = 0;
			direction *= -1;

			numberOfMovementCycles++;//increment every time platform changes direction (ie reached end of its leash)
			if(numberOfMovementCycles % 2 ==0){//platform has gone there and back
				waitForDelay = true;
			}
		}
	}
	
	//don't want object to move through other objects
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag != "Player"){//change direction if hit random object (don't go through it)
			currentDistanceMoved = 0;
			direction *= -1;
		}
	}

	private Vector3 getMovementDirection(){
		if (waitForDelay) {
			return new Vector3(0,0,0);
		} else {
			//delta time changes every frame, and thus must be recalculated every frame
			float speed_in_time = speed * Time.deltaTime;//make speed dependent on time instead of framerate
			return direction * speed_in_time;
		}
	}

	//trigger methods used in the box collider that is a trigger.
	//These methods to move player with the platform
	void OnTriggerEnter ( Collider other) {
		movePlayerWithThisPlatform = true;
	}
	void OnTriggerExit(Collider other){
		movePlayerWithThisPlatform = false;
	}
}
