using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public BoardManager boardManager = null;
	//public ButtonManager buttonManager = null;

	// Use this for initialization
	void Awake () {

		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		print ("Passing playerList to BoardManager");

		GameObject brd = GameObject.Find ("LocalGameBtnManager");
		//BoardManager boardManager = GetComponent<BoardManager> ();
		ButtonManager buttonManager = brd.GetComponent<ButtonManager> ();

		Debug.Log ("Player name should be " + ButtonManager.names[0]);
		Debug.Log ("Player name should be " + ButtonManager.names[1]);
		Debug.Log ("Player name should be " + ButtonManager.names[2]);
		Debug.Log ("Player name should be " + ButtonManager.names[3]);

		Debug.Log ("Player computer should be " + ButtonManager.hoomans[0]);
		Debug.Log ("Player computer should be " + ButtonManager.hoomans[1]);
		Debug.Log ("Player computer should be " + ButtonManager.hoomans[2]);
		Debug.Log ("Player computer should be " + ButtonManager.hoomans[3]);



		//boardManager.initGame (buttonManager.playerList);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
