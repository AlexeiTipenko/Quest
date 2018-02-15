using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print ("Board manager started");
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("r")) {
			BoardManagerMediator.getInstance ().cheat ("rankUp");
		} else if (Input.GetKeyUp ("s")) {
			BoardManagerMediator.getInstance ().cheat ("shieldsUp");
		} else if (Input.GetKeyUp ("p")) {
			BoardManagerMediator.getInstance ().cheat ("prosperity");
		} else if (Input.GetKeyUp ("c")) {
			BoardManagerMediator.getInstance ().cheat ("chivalrous");
		}
	}

}
