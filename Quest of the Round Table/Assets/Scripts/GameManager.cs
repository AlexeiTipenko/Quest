using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private Player player1;
	private List<Card> cardList;
	public Text text;

	public static GameManager instance = null;

	// Use this for initialization
	void Awake () {

		print ("fengei");

		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		print ("fenguu");
			
		player1 = new Player("Joey");
		cardList = new List<Card>();
		cardList.Add(new AtCamelot(player1, "At Camelot"));
		cardList.Add(new AtCamelot(player1, "At Camelot"));
		cardList.Add(new AtCamelot(player1, "At Camelot"));

		print ("fengoiii");

		player1.givePlayerCards(cardList);

		//player1.displayCards();
		//Console.WriteLine(player1.displayCards());

		Debug.Log(player1.displayCards());

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
