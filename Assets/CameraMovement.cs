using UnityEngine;

public class CameraMovement : MonoBehaviour {
	public float rotateSpeed ;//set in component window

	void Update() {	
		float movementX = Input.GetAxis ("Mouse X");
		transform.RotateAround (transform.parent.position, Vector3.up, rotateSpeed * movementX * Time.deltaTime);

		float movementY = Input.GetAxis ("Mouse Y");
		transform.Rotate (Vector3.left, rotateSpeed * movementY * Time.deltaTime);
		
		//transform.FindChild (0)//the camera!
	}
}
