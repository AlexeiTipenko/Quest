using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private Player player1;
	private List<Card> cardList;
	public Text text;

	// Use this for initialization
	void Start () {
		
		player1 = new Player("Joey");
		cardList.Add(new At_Camelot(player1, "At Camelot", 3));
		cardList.Add(new At_Camelot(player1, "At Camelot", 3));
		cardList.Add(new At_Camelot(player1, "At Camelot", 3));

		player1.givePlayerCards(cardList);

		//player1.displayCards();
		//Console.WriteLine(player1.displayCards());

		print(player1.displayCards());

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
