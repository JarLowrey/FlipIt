using UnityEngine;
using System.Collections;

//only put into here what is specfic to this level
public class TheDude : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MainLevelHandler script = GetComponent<MainLevelHandler> ();
		script.nextLevel = "level_2";

	}
	
	// Update is called once per frame
	void Update () {
	}


}
