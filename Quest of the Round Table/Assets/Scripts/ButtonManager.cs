using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour 
{
	
	public static ButtonManager instance;

	public static string[] names = new string[4];

	public static List<Player> playerList = new List<Player> ();

	public void SwitchScene(string playerSelection){
		SceneManager.LoadScene(playerSelection);
	}

	public void SwitchSceneWithPlayerList(string playerSelection) {
        bool validSetup = false;
        playerList = new List<Player>();
		for (int i = 0; i < 4; i++) {
			GameObject inputFieldGo = GameObject.Find("Scene Elements/Canvas/Player" + i);
			InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
			names [i] = inputFieldCo.text;
            if (names[i] != "") {
                GameObject dropDownGo = GameObject.Find("Scene Elements/Canvas/Dropdown" + i);
                Dropdown dropDownCo = dropDownGo.GetComponent<Dropdown>();
                Player player;
                if (dropDownCo.options[dropDownCo.value].text == "AI 1")
                {
                    player = new AIPlayer(names[i], new Strategy1());
                }
                else if (dropDownCo.options[dropDownCo.value].text == "AI 2")
                {
                    player = new AIPlayer(names[i], new Strategy2());
                }
                else
                {
                    player = new HumanPlayer(names[i]);
                    validSetup = true;
                }
                playerList.Add(player);
            }
		}
        if (validSetup && playerList.Count > 1) {
            SceneManager.LoadScene(playerSelection);
        }
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
