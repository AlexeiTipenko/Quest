using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEditor;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {

	void Start () {
		print ("Board manager started");
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("r")) {
			BoardManagerMediator.getInstance ().cheat ("rankUp");
		} else if (Input.GetKeyUp ("s")) {
			BoardManagerMediator.getInstance ().cheat ("shieldsUp");
		} else if (Input.GetKeyUp ("p")) {
			BoardManagerMediator.getInstance ().cheat ("prosperity");
		} else if (Input.GetKeyUp ("c")) {
			BoardManagerMediator.getInstance ().cheat ("chivalrous");
        } else if (Input.GetKeyUp ("n")) {
            BoardManagerMediator.getInstance().cheat("nextPlayer");
        }
	}

    public static void SetInteractionText(String text) {
        GameObject interactionText = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionText");
        interactionText.GetComponent<Text>().text = text;
    }

    public static void SetInteractionButtons(String text1, String text2, Action func1, Action func2) {
        GameObject button1 = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionButton1");
        GameObject button2 = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionButton2");
        GameObject buttonText1 = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionButton1/Text");
        GameObject buttonText2 = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionButton2/Text");

        buttonText1.GetComponent<Text>().text = text1;
        buttonText2.GetComponent<Text>().text = text2;

        button1.GetComponent<Button>().onClick.RemoveAllListeners();
        button2.GetComponent<Button>().onClick.RemoveAllListeners();

        if (func1 != null) {
            button1.GetComponent<Button>().onClick.AddListener(new UnityAction(func1));
        }

        if (func2 != null) {
            button2.GetComponent<Button>().onClick.AddListener(new UnityAction(func2));
        }
    }

    public static void DrawCards(Player player) {
        DestroyCards();
        DrawHand(player);
        DrawRank(player);
        DrawCardInPlay();

        //TODO: draw cards in play area
    }

    public static void DrawHand(Player player) {
        DestroyHand();
        foreach (Card card in player.getHand()) {
            GameObject handArea = GameObject.Find("Canvas/TabletopImage/HandArea");
            GameObject instance = Instantiate(Resources.Load("CardPrefab", typeof(GameObject))) as GameObject;

            Image cardImg = instance.GetComponent<Image>();
            cardImg.sprite = Resources.Load<Sprite>("cards/" + card.cardImageName);
            GameObject cardObj = Instantiate(instance);
            cardObj.tag = "HandCard";
            cardObj.transform.SetParent(handArea.transform, false);
        }
    }

    public static void DrawRank(Player player) {
        DestroyRank();
        GameObject rankArea = GameObject.Find("Canvas/TabletopImage/RankArea");
        GameObject noDragInstance = Instantiate(Resources.Load("NoDragCardPrefab", typeof(GameObject))) as GameObject;

        Image cardImg = noDragInstance.GetComponent<Image>();
        cardImg.sprite = Resources.Load<Sprite>("cards/ranks/" + player.getRank().getCardName());
        GameObject cardObj = Instantiate(noDragInstance);
        cardObj.tag = "RankCard";
        cardObj.transform.SetParent(rankArea.transform, false);
    }

    public static void DestroyHand() {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("HandCard");
        foreach (GameObject gameObj in cardObjs) {
            Destroy(gameObj);
        }
    }

    public static void DestroyRank() {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("RankCard");
        foreach (GameObject gameObj in cardObjs) {
            Destroy(gameObj);
        }
    }

    public static void DestroyCardInPlay() {
        print("Destroying card in play");
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("CardInPlay");
        foreach (GameObject gameObj in cardObjs) {
            Destroy(gameObj);
        }
    }

    public static void DrawCardInPlay() {
        DestroyCardInPlay();
        GameObject cardInPlayArea = GameObject.Find("Canvas/TabletopImage/CardInPlayArea");
        GameObject noDragInstance = Instantiate(Resources.Load("NoDragCardPrefab", typeof(GameObject))) as GameObject;

        Card cardInPlay = BoardManagerMediator.getInstance().getCardInPlay();
        Image cardImg = noDragInstance.GetComponent<Image>();
        cardImg.sprite = Resources.Load<Sprite>("cards/" + cardInPlay.cardImageName);
        GameObject cardObj = Instantiate(noDragInstance);
        cardObj.tag = "CardInPlay";
        cardObj.transform.SetParent(cardInPlayArea.transform, false);
    }

    public static void DestroyCards() {
        DestroyHand();
        DestroyRank();
        DestroyCardInPlay();

        //TODO: destroy what's on the table
    }
}
