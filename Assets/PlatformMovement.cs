using UnityEngine;
using System.Collections;

public class PlatformMovement : MonoBehaviour {
	private float currentDistanceMoved;
	public float distanceUntilChangeDirection, speed;
	public Vector3 direction;

	// Use this for initialization
	void Start () {
		if (direction == null) {
			Debug.Log ("Direction of platform is null!");
		} 

		direction.Normalize ();//ensure that direction does not impact the speed

		if (Mathf.Approximately (distanceUntilChangeDirection, 0)) {
			Debug.Log ("Distance to move platform is zero!");
		}
		
		speed *= Time.deltaTime;//make speed dependent on time instead of framerate

		if (Mathf.Approximately (speed, 0)) {
			Debug.Log ("Platform movement speed is zero!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 movementDist = direction * speed;

		if (currentDistanceMoved < distanceUntilChangeDirection) {
			transform.position += movementDist;
			currentDistanceMoved += movementDist.magnitude;
		} else {
			currentDistanceMoved = 0;
			direction *= -1;
		}
	}

	//don't want object to move through other objects
	void OnCollisionEnter(){
		currentDistanceMoved = 0;
		direction *= -1;
	}
}
