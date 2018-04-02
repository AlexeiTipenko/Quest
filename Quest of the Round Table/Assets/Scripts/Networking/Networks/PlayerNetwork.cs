using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork Instance;
    public string PlayerName { get; private set; }
    void Awake() {
        Instance = this;
        PlayerName = "P#" + Random.Range(1, 100);
    }

}