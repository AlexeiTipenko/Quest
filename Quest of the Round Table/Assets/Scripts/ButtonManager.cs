using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour 
{
	
	public static ButtonManager instance = null;

	public static string[] names = new string[4];
	public static bool[] hoomans = new bool[4];

	//public InputField player1Name, player2Name, player3Name, player4Name;
	//public Dropdown player1AI, player2AI, player3AI, player4AI;
	public List<Player> playerList = new List<Player> ();

	public void SwitchScene(string playerSelection){
		SceneManager.LoadScene(playerSelection);
	}

	public void SwitchSceneWithPlayerList(string playerSelection) {
		/*if (player1Name.text != "") {
			string playerName = player1Name.text;
			string isAIString = player1AI.captionText.text;
			bool isAI = (isAIString == "Computer" ? true : false);
			Player player = new Player (playerName, isAI);
			playerList.Add (player);
		}
		if (player2Name.text != "") {
			string playerName = player2Name.text;
			string isAIString = player2AI.captionText.text;
			bool isAI = (isAIString == "Computer" ? true : false);
			Player player = new Player (playerName, isAI);
			playerList.Add (player);
		}
		if (player3Name.text != "") {
			string playerName = player3Name.text;
			string isAIString = player3AI.captionText.text;
			bool isAI = (isAIString == "Computer" ? true : false);
			Player player = new Player (playerName, isAI);
			playerList.Add (player);
		}
		if (player4Name.text != "") {
			string playerName = player4Name.text;
			string isAIString = player4AI.captionText.text;
			bool isAI = (isAIString == "Computer" ? true : false);
			Player player = new Player (playerName, isAI);
			playerList.Add (player);
		}
		foreach (Player player in playerList) {
			print (player.toString ());
		}*/
		for (int i = 0; i < 4; i++) {
			GameObject inputFieldGo = GameObject.Find("Scene Elements/Canvas/Player" + i);
			InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
			names [i] = inputFieldCo.text;
			GameObject dropDownGo = GameObject.Find ("Scene Elements/Canvas/Dropdown" + i);
			Dropdown dropDownCo = dropDownGo.GetComponent<Dropdown> ();
			hoomans[i] = (dropDownCo.options[dropDownCo.value].text == "Computer" ? true : false);
		}
		SceneManager.LoadScene(playerSelection);
	}

	public void ExitGame(){
		Application.Quit();
	}

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}
}
