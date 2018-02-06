using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour 
{
	public void LocalGame(string playerSelection){
		SceneManager.LoadScene(playerSelection);
	}
	public void ExitGame(){
		Application.Quit();
	}
	public void MenuGame(string menu){
		SceneManager.LoadScene (menu);
	}
}
