using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEditor;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{

    Logger logger;

    void Start()
    {
		Logger.getInstance().info("Board manager started");
        print("Board manager started");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("r"))
        {
            BoardManagerMediator.getInstance().cheat("rankUp");
        }
        else if (Input.GetKeyUp("s"))
        {
            BoardManagerMediator.getInstance().cheat("shieldsUp");
        }
        else if (Input.GetKeyUp("p"))
        {
            BoardManagerMediator.getInstance().cheat("prosperity");
        }
        else if (Input.GetKeyUp("c"))
        {
            BoardManagerMediator.getInstance().cheat("chivalrous");
        }
        else if (Input.GetKeyUp("n"))
        {
            BoardManagerMediator.getInstance().cheat("nextPlayer");
        }
    }

    public static void SetInteractionText(String text)
    {
        GameObject interactionText = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionText");
        interactionText.GetComponent<Text>().text = text;
    }

    public static void SetInteractionButtons(String text1, String text2, Action func1, Action func2)
    {
        GameObject button1 = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionButton1");
        GameObject button2 = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionButton2");
        GameObject buttonText1 = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionButton1/Text");
        GameObject buttonText2 = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionButton2/Text");

        buttonText1.GetComponent<Text>().text = text1;
        buttonText2.GetComponent<Text>().text = text2;


        if (func1 != null) {
            button1.SetActive(true);
            button1.GetComponent<Button>().onClick.AddListener(ClearInteractions);
            button1.GetComponent<Button>().onClick.AddListener(new UnityAction(func1));
        }

        if (func2 != null) {
            button2.SetActive(true);
            button2.GetComponent<Button>().onClick.AddListener(ClearInteractions);
            button2.GetComponent<Button>().onClick.AddListener(new UnityAction(func2));
        }
    }

    /*
    public static void WaitUntilButtonClick(bool buttonClicked){

        //coroutine = Coroutine(buttonClicked);
        //StartCoroutine(coroutine);

        //yield return StartCoroutine(Coroutine(buttonClicked));
        StartCoroutine("Coroutine");
    }


    IEnumerator Coroutine()
    {
        while (true)
        {
            //yield return new WaitForSeconds(waitTime);
            yield return null;
        }
    }
    */


    public static void ClearInteractions() {
        GameObject interactionText = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionText");
        GameObject button1 = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionButton1");
        GameObject button2 = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionButton2");

        button1.GetComponent<Button>().onClick.RemoveAllListeners();
        button2.GetComponent<Button>().onClick.RemoveAllListeners();

        interactionText.GetComponent<Text>().text = "";
        button1.SetActive(false);
        button2.SetActive(false);
    }

    public static void DrawCards(Player player) {
        DestroyCards();
        DrawHand(player);
        DrawRank(player);
        DrawCardInPlay();

        //TODO: draw cards in play area
    }

    public static List<string> GetSelectedCardNames()
    {
        GameObject playArea = GameObject.Find("Canvas/TabletopImage/PlayerPlayArea");
        List<string> cardNames = new List<string>();

        foreach (Transform child in playArea.transform)
        {
            cardNames.Add(child.gameObject.name);
        }
        return cardNames;
    }

    public static List<string> GetSelectedDiscardNames()
    {
        GameObject boardArea = GameObject.Find("Canvas/TabletopImage/DiscardArea");
        List<string> cardNames = new List<string>();

        foreach (Transform child in boardArea.transform)
        {
            cardNames.Add(child.gameObject.name);
        }
        return cardNames;
    }

    public static void ReturnCardsToPlayer()
    {
        GameObject playArea = GameObject.Find("Canvas/TabletopImage/PlayerPlayArea");
        GameObject handArea = GameObject.Find("Canvas/TabletopImage/HandArea");

        foreach (Transform child in playArea.transform)
        {
            child.gameObject.transform.SetParent(handArea.transform, false);
        }
    }

    public static void DrawHand(Player player)
    {
        DestroyHand();
        foreach (Card card in player.getHand())
        {
            GameObject handArea = GameObject.Find("Canvas/TabletopImage/HandArea");
            GameObject instance = Instantiate(Resources.Load("CardPrefab", typeof(GameObject))) as GameObject;
            instance.name = card.getCardName();
            Image cardImg = instance.GetComponent<Image>();
            cardImg.sprite = Resources.Load<Sprite>("cards/" + card.cardImageName);
            instance.tag = "HandCard";
            instance.transform.SetParent(handArea.transform, false);
        }
    }

    public static void DrawRank(Player player)
    {
        DestroyRank();
        GameObject rankArea = GameObject.Find("Canvas/TabletopImage/RankArea");
        GameObject noDragInstance = Instantiate(Resources.Load("NoDragCardPrefab", typeof(GameObject))) as GameObject;
        Image cardImg = noDragInstance.GetComponent<Image>();
        noDragInstance.name = player.getRank().getCardName();
        cardImg.sprite = Resources.Load<Sprite>("cards/ranks/" + player.getRank().getCardName());
        noDragInstance.tag = "RankCard";
        noDragInstance.transform.SetParent(rankArea.transform, false);
    }

    public static void DestroyHand()
    {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("HandCard");
        foreach (GameObject gameObj in cardObjs)
        {
            Destroy(gameObj);
        }
    }

    public static void DestroyRank()
    {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("RankCard");
        foreach (GameObject gameObj in cardObjs)
        {
            Destroy(gameObj);
        }
    }

    public static void DestroyCardInPlay()
    {
        //print("Destroying card in play");
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("CardInPlay");
        foreach (GameObject gameObj in cardObjs)
        {
            Destroy(gameObj);
        }
    }

    public static void DestroyStage(int stages)
    {
        for (int i = 0; i < stages; i++){
            GameObject boardAreaFoe = GameObject.Find("Canvas/TabletopImage/StageAreaFoe" + i);
            Destroy(boardAreaFoe);
        }
    }

    public static void DestroyDiscardArea()
    {

        GameObject discardArea = GameObject.Find("Canvas/TabletopImage/DiscardArea");
        Destroy(discardArea);

    }

    public static void DrawCardInPlay()
    {
        DestroyCardInPlay();
        GameObject cardInPlayArea = GameObject.Find("Canvas/TabletopImage/CardInPlayArea");
        GameObject noDragInstance = Instantiate(Resources.Load("NoDragCardPrefab", typeof(GameObject))) as GameObject;
        Card cardInPlay = BoardManagerMediator.getInstance().getCardInPlay();
        Image cardImg = noDragInstance.GetComponent<Image>();
        noDragInstance.name = cardInPlay.getCardName();
        cardImg.sprite = Resources.Load<Sprite>("cards/" + cardInPlay.cardImageName);
        noDragInstance.tag = "CardInPlay";
        noDragInstance.transform.SetParent(cardInPlayArea.transform, false);
    }

    public static void DestroyCards()
    {
        DestroyHand();
        DestroyRank();
        DestroyCardInPlay();

        //TODO: destroy what's on the table
    }

    public static void SetupQuestPanels(int numStages){
        GameObject board = GameObject.Find("Canvas/TabletopImage");
        Debug.Log("Num stages is: " + numStages);
        float position = -465;
        for (int i = 0; i < numStages; i++){
            GameObject BoardAreaFoe = Instantiate(Resources.Load("StageAreaPrefab", typeof(GameObject))) as GameObject;
            Debug.Log("Position is: " + position);

            BoardAreaFoe.name = "StageAreaFoe" + i;
            BoardAreaFoe.transform.position = new Vector3(position, BoardAreaFoe.transform.position.y, BoardAreaFoe.transform.position.z);
            BoardAreaFoe.transform.SetParent(board.transform, false);

            position += 155;
        }
    }

    public static void SetupDiscardPanel()
    {
        GameObject board = GameObject.Find("Canvas/TabletopImage");

        //float position = -465;

        GameObject DiscardArea = Instantiate(Resources.Load("DiscardArea", typeof(GameObject))) as GameObject;
        //Debug.Log("Position is: " + position);

        DiscardArea.name = "DiscardArea";
        //DiscardArea.transform.position = new Vector3(position, DiscardArea.transform.position.y, DiscardArea.transform.position.z);
        DiscardArea.transform.SetParent(board.transform, false);

        //position += 155;

    }
}
