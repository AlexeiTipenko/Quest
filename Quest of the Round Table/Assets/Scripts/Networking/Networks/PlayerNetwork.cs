using UnityEngine;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork Instance;
    public string PlayerName { get; private set; }

    void Awake() {
        Instance = this;
        PlayerName = "Kirk" + Random.Range(1000, 9999);
    }

}
