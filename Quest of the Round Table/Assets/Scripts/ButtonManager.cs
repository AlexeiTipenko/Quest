using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour 
{
	
	public static ButtonManager instance;

	public static bool validSetup;

	public static string[] names = new string[4];

	public static List<Player> playerList = new List<Player> ();

    public static string scenario = null;

	public void SwitchScene(string scene){
		SceneManager.LoadScene(scene);
	}

	public void SwitchSceneWithPlayerList(string scene) {
		PreparePlayers ();
        if (validSetup && playerList.Count > 1) {
			SwitchScene (scene);
        }
	}

	public void RiggedScenario(string scene, int scenarioNum) {
		scenario = "scenario" + scenarioNum;
		SwitchSceneWithPlayerList (scene);
	}

    public void RiggedScenario1(string scene) {
		RiggedScenario (scene, 1);
    }

    public void RiggedScenario2(string scene) {
		RiggedScenario (scene, 2);
    }

	private void PreparePlayers() {
		validSetup = false;
		playerList = new List<Player>();
		for (int i = 0; i < 4; i++)
		{
			GameObject inputFieldGo = GameObject.Find("Scene Elements/Canvas/Player" + i);
			InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
			names[i] = inputFieldCo.text;
			if (names[i] != "")
			{
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
	}

	public void ExitGame(){
		Application.Quit();
	}

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}
}
