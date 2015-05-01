using UnityEngine;
using System.Collections;

public class ParticleHandler : MonoBehaviour {

	GameObject water;
	ParticleAnimator anim;
	GravityHandler gravScript;
	MainLevelHandler player;
	Animator animateTheDude;
	bool won;
	// Use this for initialization
	void Start () {
		water = GameObject.Find("Water Fountain");
		anim = GetComponent<ParticleAnimator>();
		GameObject theDude = GameObject.Find ("Dude");
		gravScript = theDude.GetComponent<GravityHandler> ();
		GameObject dude = GameObject.Find ("Dude");
		animateTheDude = dude.GetComponent<Animator> (); 
		player = dude.GetComponent<MainLevelHandler> ();
		won = player.won;

	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 grav = new Vector3 (0, -9.81f, 0);
		Vector3 reverseGrav = new Vector3 (0, 9.81f, 0);
		if (gravScript.currentlyFlipped)
			anim.force = reverseGrav;
		else
			anim.force = grav;

		if (won)
			water.SetActive (false); //turn off if you won so it doesn't hit you anymore


	

	
	}

	public void OnParticleCollision (GameObject other)
	{
		
		if (other.name == "Dude") {
			animateTheDude.SetTrigger("die");
			player.ShowDeathGui();
		}

	}
}
