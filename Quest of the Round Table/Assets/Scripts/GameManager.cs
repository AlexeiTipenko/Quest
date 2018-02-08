using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public BoardManager boardManager = null;

	void Awake () {

		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		print ("Passing playerList to BoardManager");

		BoardManager boardManager = GetComponent<BoardManager> ();

		foreach (Player player in ButtonManager.playerList) {
			print (player.toString ());
		}

		boardManager.initGame (ButtonManager.playerList);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
