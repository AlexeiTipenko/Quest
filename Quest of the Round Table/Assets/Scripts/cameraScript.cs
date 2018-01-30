using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour {

	public float cameraWidth = 206f;
	public float cameraHeight = 189f;

	void Update () 
	{
		// Camera has fixed width and height on every screen solution
		float x = (100f - 100f / (Screen.width / cameraWidth)) / 100f;
		float y = (100f - 100f / (Screen.height / cameraHeight)) / 100f;
		GetComponent<Camera>().rect = new Rect(x, y, 1, 1);
	}
}
