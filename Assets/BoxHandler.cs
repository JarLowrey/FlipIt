using UnityEngine;
using System.Collections;

public class BoxHandler : MonoBehaviour {
	AudioSource boxSource;
	GameObject box;

	// Use this for initialization
	void Start () {
		box = GameObject.Find ("CubeBlockingDoor");
		boxSource = box.GetComponent<AudioSource> ();
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	//automatically does collision for the dude (bc attached to this script)
	//these funcitons are not called
	public void OnCollisionEnter(Collision collision){
		//collision and you are actually moving it
		//all box collision goes here
		if (collision.gameObject.name == "Dude") {
			if (Mathf.Abs (box.rigidbody.velocity.x) > 0) {
				if (!boxSource.isPlaying)
					boxSource.Play (); //continuous playing as long as loop is checked in box audio source componenet	\
			}
		}
	}

	public void OnCollisionExit(Collision collision)
	{
		//collision exit of cube block door
			boxSource.Stop (); //stop collision noise
	}
}
