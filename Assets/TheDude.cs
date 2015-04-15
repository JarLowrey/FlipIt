using UnityEngine;
using System.Collections;

public class TheDude : MonoBehaviour {

	Animator animateTheDude;
	bool characterRotating;
	float rotationRate = 2f;
	float roofHeight;
	float groundHeight;
	bool currentlyFlipped = false; //based on original oritenation flipped is flipped from beginning
	bool isPaused;
	Rect pauseMenu;
	AudioSource boxSource;
	GameObject box;
	GameObject theGuy;
	string nextLevel = "level_2"; //the next level for loading
	
	
	// Use this for initialization
	void Start () {
		animateTheDude = GetComponent<Animator> ();
		characterRotating = false;
		roofHeight = GameObject.Find ("Roof").transform.position.y; //roof height
		groundHeight = 0; 
		//pauseCanvas = GameObject.Find ("PauseCanvas");
		//pauseCanvas.SetActive (false);
		isPaused = false;
		pauseMenu = new Rect (0, 0, Screen.width, Screen.height);
		box = GameObject.Find ("CubeBlockingDoor");
		boxSource = box.GetComponent<AudioSource> ();
		theGuy = GameObject.Find ("Dude");
		
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
		if (Input.GetKeyDown (KeyCode.Escape)) {
			isPaused = !isPaused; //flip now, first hit makes this true, is it now paused? is what this does
			if(isPaused)
				pause (); //run pause
			else
				unPause(); //handles multiple cases and inuts
		}



		//Gravity controller
		if( !characterRotating && Input.GetKeyDown(KeyCode.G) ) //cannot rotate while in air, only near planes
		{
			//set speed of rotation based on how far away wall is (if possible)
			characterRotating = true;
			Physics.gravity *= -1;
			currentlyFlipped = !currentlyFlipped; //flip this variable
		}

		rotate ();

		if (checkForFallout ())
			respawn ();

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
			if(Mathf.Abs(rot.z) < Mathf.Abs(rotationRate)){//rightside up
				//smooth rotation
				rot = new Vector3(rot.x,rot.y,0);
				this.transform.rotation = Quaternion.Euler(rot);
				//signal rotation has ended
				characterRotating = false;
			}else if( Mathf.Abs(rot.z-180) < Mathf.Abs(rotationRate) ){//upside down
				//smooth rotation
				rot = new Vector3(rot.x,rot.y,180);
				this.transform.rotation = Quaternion.Euler(rot);
				//signal rotation has ended
				characterRotating = false;
			}
		}
	}

	//helper funciton to check and see if player is closeEnough to a plane (roof or floor)
	//in future iterations, this can include moving platforms and other areas that will be acceptable for rotation
	//true if close enough, false o.w
	private bool isOnPlane(){
		float curYpos = this.transform.position.y;
		if (curYpos + 1 >= roofHeight)
			return true;
		if (curYpos - 1 <= groundHeight)
			return true;
		return false; //otherwise
	}

	//check if fall through roof (or through ground to be added later)
	private bool checkForFallout()
	{
		float curYpos = this.transform.position.y;
		if (curYpos > roofHeight + 5)
			return true;
		return false;

	}


	private void respawn()
	{

		GameObject respawnPoint = GameObject.Find ("Respawn Point");
		Vector3 respawnPos = respawnPoint.transform.position;
		this.transform.position = respawnPos;
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
		if (isPaused) {
			GUI.Box (pauseMenu, "GAME PAUSED");
			
			// Make the Quit button.
			//params: LEFT TOP WIDTH HEIGHT
			if (GUI.Button (new Rect (Screen.width/2-50, Screen.height/2-100, 100, 50), "Resume")) {
				isPaused = false; //flips
				Time.timeScale = 1;
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
		if (collision.gameObject.name == "CubeBlockingDoor" && Mathf.Abs (box.rigidbody.velocity.x) > 0) {
			if (!boxSource.isPlaying)
				boxSource.Play(); //continuous playing as long as loop is checked in box audio source componenet
			
		}

	}
	
	public void OnCollisionExit(Collision collision)
	{
		//collision exit of cube block door
		if (collision.gameObject.name == "CubeBlockingDoor") {
			boxSource.Stop (); //stop collision noise
		}
		if (collision.gameObject.name == "Door") {
			goToNextLevel (nextLevel);
			//play winning sound?
		}
		
	}

	private void goToNextLevel(string name){
		if (currentlyFlipped) {
			Physics.gravity *= - 1;
			currentlyFlipped = false;
		}
		Application.LoadLevel (name);
	}

}
