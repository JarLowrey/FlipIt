using UnityEngine;
using System.Collections;

public class MovePlatform : MonoBehaviour {
	private float currentDistanceMoved;
	private bool movePlayerWithPlatform;
	public float distanceUntilChangeDirection, speed;
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
		speed *= Time.deltaTime;//make speed dependent on time instead of framerate
		direction *= speed;
		movePlayerWithPlatform = false;
	}
	
	// Update is called once per frame
	void Update () {
		movePlatform ();

		//move main player along with the moving platform
		if (movePlayerWithPlatform) {
			GameObject.Find("Dude").transform.position +=(direction);
		}
	}

	private void movePlatform(){
		if (currentDistanceMoved < distanceUntilChangeDirection) {
			transform.position += direction;
			currentDistanceMoved += direction.magnitude;
		} else {
			currentDistanceMoved = 0;
			direction *= -1;
		}
	}
	
	//don't want object to move through other objects
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag != "Player"){//change direction if hit random object (don't go through it)
			currentDistanceMoved = 0;
			direction *= -1;
		}
	}

	//trigger methods used in the box collider that is a trigger.
	//These methods to move player with the platform
	void OnTriggerEnter ( Collider other) {
		movePlayerWithPlatform = true;
	}
	void OnTriggerExit(Collider other){
		movePlayerWithPlatform = false;
	}
}
