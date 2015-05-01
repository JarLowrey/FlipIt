using UnityEngine;
using System.Collections;

public class MainLevelHandler : MonoBehaviour {
	
	Animator animateTheDude;
	//bool characterRotating;
	//float rotationRate = 2f;
	public float roofHeight,groundHeight,timeToWin;
	private  bool isPaused,isDead,currentlyFlipped;//based on original oritenation flipped is flipped from beginning, found in GravityHandler
	public bool won;
	Rect pauseMenu;
	//AudioSource boxSource;
	//GameObject box;
	GameObject theGuy;
	public string nextLevel; //the next level for loading, changed by specific level
	GravityHandler gravScript;
	GameObject[]allObjsNotDude;   

	
	
	// Use this for initialization
	void Start () {
		allObjsNotDude = GameObject.FindGameObjectsWithTag("notDude"); 
		gravScript = GetComponent<GravityHandler> ();
		animateTheDude = GetComponent<Animator> ();
		isPaused = false;
		isDead = false;
		pauseMenu = new Rect (0, 0, Screen.width, Screen.height);
		//box = GameObject.Find ("CubeBlockingDoor");
		//boxSource = box.GetComponent<AudioSource> ();
		theGuy = GameObject.Find ("Dude");
		won = false;
		Time.timeScale = 1; //always reset time
	}
	
	// Update is called once per frame
	void Update () {
		//grab variables from grav handler
		currentlyFlipped = gravScript.currentlyFlipped; //grab the currently flipped variable from the gravity handler

		//Animation controllers
		float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");
		animateTheDude.SetFloat("runningSpeed", vertical );

		//animateTheDude.SetFloat ("speed", vertical);
		animateTheDude.SetFloat ("Direction", horizontal, 0.25f, Time.deltaTime);
		if (Input.GetKeyDown (KeyCode.Space)) {
			animateTheDude.SetTrigger ("jump");
		}
		/*
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			animateTheDude.SetTrigger ("die");
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			animateTheDude.SetTrigger ("revive");
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			animateTheDude.SetTrigger ("wave");
		}
		*/
		if (Input.GetKeyDown (KeyCode.Escape) && !won) {
			isPaused = !isPaused; //flip now, first hit makes this true, is it now paused? is what this does
			if(isPaused)
				pause (); //run pause
			else
				unPause(); //handles multiple cases and inuts
		}
		
		if (checkForFallout (this.gameObject) && !isDead && !won)
			gravScript.respawn ();
	else {
			foreach (GameObject notDude in allObjsNotDude)
				if(checkForFallout(notDude))
					respawnObject (notDude); //check to see if anything has fallen out, and if so destroy it because its just extra computation
		}
		
		//if character is touching ground then isFlipping = false;


	}
	
	private void fixRotation()
	{
		Vector3 cur = this.transform.rotation.eulerAngles;
		if (currentlyFlipped) {
			//rotation should be 180
			//this.transform.rotation.z = 180;
		}
		else
		{
			//not flipped
			this.transform.Rotate(new Vector3(cur.x,cur.y,0));
		}
	}
	

	
	//check if fall through roof (or through ground to be added later)
	private bool checkForFallout(GameObject obj)
	{
		float curYpos = obj.transform.position.y;
		if (curYpos > roofHeight + 5 || curYpos<groundHeight - 5)
			return true;
		return false;
		
	}
	

	//different from respawn the dude, which adjusts gravity etc.
	private void respawnObject(GameObject obj)
	{
		GameObject respawnPoint = GameObject.Find ("Respawn Point");
		Vector3 respawnPos = respawnPoint.transform.position;
		obj.transform.position = respawnPos;
		Debug.Log (respawnPos);
	}
	
	
	//enter this if game is to be paused now
	//seprated to allow for seprate music etc.
	private void pause()
	{
		//needs to be paused
		Time.timeScale = 0;
		//gets checked every frame, so as long as it is paused this GUI menu gets polled WIHTOUT explicilty calling ONGUI
	}
	
	//enter this if game is to be unPaused via hitting escape
	private void unPause()
	{
		//just unpause
		Time.timeScale = 1;
	}
	
	public void newLoad(string name)
	{
		//these things happen regardless of what you are loading
		Time.timeScale = 1; //re allow game back in
		isPaused = false; //not paused
		//pauseCanvas.SetActive (false);
		if (currentlyFlipped) {
			Physics.gravity *= - 1;
			currentlyFlipped = false;
		}
		Application.LoadLevel (name);
	}
	
	
	public void OnGUI(){
		if ((isPaused || isDead) && !won) {
			if(isDead)
				GUI.Box (pauseMenu, "You Died!");
			else if(isPaused)
				GUI.Box (pauseMenu,"GAME PAUSED");
			
			// Make the Quit button.
			//params: LEFT TOP WIDTH HEIGHT
			if(!isDead){
				if (GUI.Button (new Rect (Screen.width/2-50, Screen.height/2-100, 100, 50), "Resume")) {
					isPaused = false; //flips
					Time.timeScale = 1;
					//GetComponent (MouseLook).enabled = true; //resumes game, as isPaused is false
				}
			}
			if (GUI.Button (new Rect (Screen.width/2-50, Screen.height/2-50, 100, 50), "Main Menu"))
			{
				newLoad ("Startup");
			}
			if (GUI.Button (new Rect (Screen.width/2-50, Screen.height/2, 100, 50), "Retry"))
			{
				newLoad (Application.loadedLevelName);
			}
			if (GUI.Button (new Rect (Screen.width/2-50, Screen.height/2+50, 100, 50), "Quit")) {
				Application.Quit(); //IN EDITOR, THERE IS NO FUNCTION OF THIS
			}
		}

		//for winning only
		if (won && !isPaused && !isDead) {
			Time.timeScale = 0; //stop time
				string message = "You WON in " + timeToWin + " seconds";
				GUI.Box (pauseMenu, message);

				if (GUI.Button (new Rect (Screen.width/2-50, Screen.height/2-100, 100, 50), "Next")) {
					isPaused = false; //flips
					Time.timeScale = 1;
					goToNextLevel (nextLevel);
					//GetComponent (MouseLook).enabled = true; //resumes game, as isPaused is false
				}

			if (GUI.Button (new Rect (Screen.width/2-50, Screen.height/2-50, 100, 50), "Main Menu"))
			{
				newLoad ("Startup");
			}
			if (GUI.Button (new Rect (Screen.width/2-50, Screen.height/2, 100, 50), "Retry"))
			{
				newLoad (Application.loadedLevelName);
			}
			if (GUI.Button (new Rect (Screen.width/2-50, Screen.height/2+50, 100, 50), "Quit")) {
				Application.Quit(); //IN EDITOR, THERE IS NO FUNCTION OF THIS
			}
		}
	}

	public void ShowDeathGui(){
		isDead = true;
	}
	/*private bool boxIsMovingOnPlane()
	{
		//only returns true if box is moving on a plan i.e. only in x direction
		if (Mathf.Abs (box.rigidbody.velocity.x) > 0 && Mathf.Abs (box.rigidbody.velocity.y) == 0 && Mathf.Abs (box.rigidbody.velocity.z) > 0)
			return true;
		return false;
	}*/
	
	//automatically does collision for the dude (bc attached to this script)
	//these funcitons are not called
	public void OnCollisionEnter(Collision collision){
		//collision and you are actually moving it
		//all box collision goes here
		if (collision.gameObject.tag == "Goal") {
			//goToNextLevel (nextLevel);
			won = true;
			timeToWin = Time.timeSinceLevelLoad;
			//play winning sound?
		}

	}
	
	public void OnCollisionExit(Collision collision)
	{

		
	}


	

	private void goToNextLevel(string name){
		if (currentlyFlipped) {
			Physics.gravity *= - 1;
			currentlyFlipped = false;
		}
		Application.LoadLevel (name);
	}
	
}
