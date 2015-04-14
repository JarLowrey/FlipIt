using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {
	
	public GameObject loadingImage;
	
	public void LoadScene(string name)
	{
		//loadingImage.SetActive(true);
		Application.LoadLevel(name);
	}
}