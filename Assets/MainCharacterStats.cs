using UnityEngine;
using System.Collections;

public class MainCharacterStats : MonoBehaviour {

	Animator animateTheDude;
	MainLevelHandler mainScript;
	bool isDead;
	GravityHandler gravScript;

	// Use this for initialization
	void Start () {
		animateTheDude = GetComponent<Animator> (); 
		mainScript = GetComponent<MainLevelHandler>();
		gravScript = GetComponent<GravityHandler> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//collision with guy as pertains to health/stats etc.
	public void OnCollisionEnter(Collision collision){
		//collision and you are actually moving it
		//all box collision goes here
		if (collision.gameObject.tag == "notDude") {
			handleCrushCollision (collision.gameObject, this.gameObject); //whatever this script is attached to and the box
		}
	}
	
	public void OnCollisionExit(Collision collision)
	{
		
		
	}

	//handle collison between two objects
	//this funciton checks to see if something is crushed
	//obj1 should be checked to see if it is crushing obj2
	//assume that crusher can crush beingCrushed
	private void handleCrushCollision(GameObject crusher, GameObject beingCrushed)
	{
		RaycastHit hit;
		float velocityRequiredToKill = 3f;
		bool onPlane = gravScript.isOnPlane ();
		
		bool crush = Physics.Raycast (beingCrushed.transform.position, Vector3.up, out hit,4); //true if the beingCrushed looks up and is being hit, 3 is hardcoded for now
		Debug.Log (onPlane);
		Debug.Log (crusher.rigidbody.velocity.y);
		//options, play with mass, make a ratio of mass/velocity? extension ideas
		if (crush && Mathf.Abs (crusher.rigidbody.velocity.y) >= velocityRequiredToKill) {
			//GameObject.Destroy (beingCrushed);
			//newLoad (Application.loadedLevelName);
			animateTheDude.SetTrigger ("die");
			mainScript.ShowDeathGui();
		}
	}
	

}
