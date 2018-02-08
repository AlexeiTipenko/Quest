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

	public static List<Player> playerList = new List<Player> ();

	public void SwitchScene(string playerSelection){
		SceneManager.LoadScene(playerSelection);
	}

	public void SwitchSceneWithPlayerList(string playerSelection) {
		for (int i = 0; i < 4; i++) {
			GameObject inputFieldGo = GameObject.Find("Scene Elements/Canvas/Player" + i);
			InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
			names [i] = inputFieldCo.text;
			GameObject dropDownGo = GameObject.Find ("Scene Elements/Canvas/Dropdown" + i);
			Dropdown dropDownCo = dropDownGo.GetComponent<Dropdown> ();
			hoomans[i] = (dropDownCo.options[dropDownCo.value].text == "Computer" ? true : false);
			Player player = new Player (names [i], hoomans [i]);
			playerList.Add (player);
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
