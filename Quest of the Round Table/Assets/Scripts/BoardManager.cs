using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {

	void Start () {
		print ("Board manager started");

		//Adding prefab to hand area
		/*
		print ("Loading prefab...");
		GameObject cardPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Allies/Merlin.prefab", typeof(GameObject)) as GameObject;

		var handArea = GameObject.Find("HandArea").transform;
		if (handArea != null) {
			var card = GameObject.Instantiate (cardPrefab);

			//Add Image Component to it(This will add RectTransform as-well)
			card.AddComponent<Image>();

			//Center Image to screen
			card.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

			card.transform.SetParent (handArea.transform, false);
			print ("Prefab should be in hand.");
		}
		else
			print ("Prefab has not been found.");
			*/
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
