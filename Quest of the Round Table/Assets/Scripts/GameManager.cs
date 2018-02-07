using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public BoardManager boardManager = null;
	public ButtonManager buttonManager = null;

	// Use this for initialization
	void Awake () {

		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		print ("Passing playerList to BoardManager");
		boardManager = GetComponent<BoardManager> ();
		buttonManager = GetComponent<ButtonManager> ();

		boardManager.initGame (buttonManager.playerList);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
