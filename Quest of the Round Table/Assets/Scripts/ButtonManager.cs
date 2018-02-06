using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour 
{
	public void SwitchScene(string playerSelection){
		SceneManager.LoadScene(playerSelection);
	}
	public void ExitGame(){
		Application.Quit();
	}
}
