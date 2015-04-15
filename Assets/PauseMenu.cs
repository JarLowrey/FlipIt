using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {


//resume game, maybe we need to disable movement and camera and reanble accordingly?
public void resume () {
		//GetComponent<Level2> ().unPause ();
		/*GameObject canv = GameObject.Find ("PauseCanvas");
		canv.SetActive (false);
		Time.timeScale = 1;
		Debug.Log ("here");*/
		Debug.Log ("here");

	}

	public void MainMenu(){
		Time.timeScale = 1;
		Application.LoadLevel ("Startup");

	}


	/*if(
		Application.LoadLevel("startup");
		}
		if(GUILayout.Button("Restart")){
		Application.LoadLevel(Application.loadedLevelName); //open this one
	}
	if(GUILayout.Button("Quit")){
		Application.Quit();
	}*/
	}
