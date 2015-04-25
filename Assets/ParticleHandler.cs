using UnityEngine;
using System.Collections;

public class ParticleHandler : MonoBehaviour {

	GameObject water;
	ParticleAnimator anim;
	GravityHandler gravScript;
	// Use this for initialization
	void Start () {
		water = GameObject.Find("Water Fountain");
		anim = GetComponent<ParticleAnimator>();
		GameObject theDude = GameObject.Find ("Dude");
		gravScript = theDude.GetComponent<GravityHandler> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 grav = new Vector3 (0, -9.81f, 0);
		Vector3 reverseGrav = new Vector3 (0, 9.81f, 0);
		if (gravScript.currentlyFlipped)
			anim.force = reverseGrav;
		else
			anim.force = grav;
	
	}
}
