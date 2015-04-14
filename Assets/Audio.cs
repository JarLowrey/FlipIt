using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour {

	AudioSource boxMoving;

	// Use this for initialization
	void Start () {
		boxMoving = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		//boxMoving.Play ();
	}
}
