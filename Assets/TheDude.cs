using UnityEngine;
using System.Collections;

public class TheDude : MonoBehaviour {

	Animator animateTheDude;
	bool isFlipping,isOnRoof;
	// Use this for initialization
	void Start () {
		animateTheDude = GetComponent<Animator> ();
		isFlipping = false;
		isOnRoof = false;
	}
	
	// Update is called once per frame
	void Update () {
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
		if(Input.GetKeyDown(KeyCode.G))
		{
			isFlipping = true;
			isOnRoof = !isOnRoof;
			rotateCharacter();
			Physics.gravity *= -1;
		}

		//if character is touching ground then isFlipping = false;
	}
	
	//need a function to smoothly rotate character. Should probably change speed of rotation depending on distance from walls.
	//Must ensure that character has flipped by the time he touches next wall
	//this might be useful http://codereview.stackexchange.com/questions/68217/rotating-a-character-upside-down-and-vice-versa
	private void rotateCharacter(){ 
		Vector3 rot = this.rigidbody.rotation.eulerAngles; 
		rot = new Vector3(rot.x,rot.y+180,rot.z);
		this.rigidbody.rotation = Quaternion.Euler(rot);
	}
}
