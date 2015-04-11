using UnityEngine;
using System.Collections;

public class TheDude : MonoBehaviour {

	Animator animateTheDude;
	bool characterRotating;
	float rotationRate = 2f;

	// Use this for initialization
	void Start () {
		animateTheDude = GetComponent<Animator> ();
		characterRotating = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Animation controllers
		float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");
		animateTheDude.SetFloat("speed", vertical );
		animateTheDude.SetFloat ("Direction", horizontal, 0.25f, Time.deltaTime);
		if( Input.GetKeyDown(KeyCode.Space) ){
			animateTheDude.SetTrigger ("jump");
		}
		if( Input.GetKeyDown(KeyCode.Alpha1) ){
			animateTheDude.SetTrigger ("die");
		}
		if( Input.GetKeyDown(KeyCode.Alpha2) ){
			animateTheDude.SetTrigger ("revive");
		}
		if( Input.GetKeyDown(KeyCode.Alpha3) ){
			animateTheDude.SetTrigger ("wave");
		}

		//Gravity controller
		if( !characterRotating && Input.GetKeyDown(KeyCode.G) ) //cannot rotate while in air - how to check that?
		{
			//set speed of rotation based on how far away wall is (if possible)
			characterRotating = true;
			Physics.gravity *= -1;
		}

		rotate ();
		//if character is touching ground then isFlipping = false;
	}
	
	//need a function to smoothly rotate character. Should probably change speed of rotation depending on distance from walls.
	//Must ensure that character has flipped by the time he touches next wall
	//this might be useful http://codereview.stackexchange.com/questions/68217/rotating-a-character-upside-down-and-vice-versa
	private void rotateCharacter(){ 
		//var distance = Vector3.Distance(object1.transform.position, object2.transform.position);//calc distance between 2 objects
		Vector3 rot = this.transform.rotation.eulerAngles; 
		rot = new Vector3(rot.x,rot.y,rot.z+180);
		this.transform.rotation = Quaternion.Euler(rot);
	}
	/*
	private void rotate(){
		float totalRotation = 0;
		while (characterRotating) {
			if(totalRotation >= 180){
				characterRotating = false;
			}else{
				Vector3 rot = this.transform.rotation.eulerAngles;
				transform.Rotate(0, 0, rotationRate * Time.deltaTime);
				totalRotation += rotationRate * Time.deltaTime;
			}
		}

	}*/

	private void rotate(){
		if (characterRotating) {
			transform.Rotate(0,0,rotationRate);
			
			Vector3 rot = this.transform.rotation.eulerAngles; 
			//check if character is done rotating
			if(Mathf.Abs(rot.z)<1){//rightside up
				//smooth rotation
				rot = new Vector3(rot.x,rot.y,0);
				this.transform.rotation = Quaternion.Euler(rot);
				//signal rotation has ended
				characterRotating = false;
			}else if( Mathf.Abs(rot.z-180)<1){//upside down
				//smooth rotation
				rot = new Vector3(rot.x,rot.y,180);
				this.transform.rotation = Quaternion.Euler(rot);
				//signal rotation has ended
				characterRotating = false;
			}
		}
	}
}
