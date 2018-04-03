using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PunManager : Photon.MonoBehaviour {

    [PunRPC]
    public void SwitchScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
