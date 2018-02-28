using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    private static GameObject coverCanvas = null;
    private static GameObject coverInteractionText = null;
    private static GameObject coverInteractionButton = null;
    private static GameObject coverInteractionButtonText = null;
    private static Player previousPlayer = null;
    private static bool isFreshTurn = true;

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


        if (func1 != null)
        {
            button1.SetActive(true);
            button1.GetComponent<Button>().onClick.AddListener(ClearInteractions);
            button1.GetComponent<Button>().onClick.AddListener(new UnityAction(func1));
        }

        if (func2 != null)
        {
            button2.SetActive(true);
            button2.GetComponent<Button>().onClick.AddListener(ClearInteractions);
            button2.GetComponent<Button>().onClick.AddListener(new UnityAction(func2));
        }
    }

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
        DrawCover(player);
        DrawHand(player);
        DrawRank(player);
        DrawCardInPlay();
        DrawStageAreaCards(player);
        DrawPlayArea(player);
        DestroyPlayerInfo();
        DisplayPlayers();
        previousPlayer = player;
        Debug.Log(player.getName() + "'s cards drawn to GUI");
        Logger.getInstance().info(player.getName() + "'s cards drawn to GUI");
    }

    public static void setIsFreshTurn(bool isFreshTurn) {
        BoardManager.isFreshTurn = isFreshTurn;
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
            child.tag = "DiscardedCard";
            cardNames.Add(child.gameObject.name);
        }
        DestroyDiscardArea();

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

    public static void DrawCover(Player player) {
        HideCover();
        Debug.Log("Current player: " + player.getName());
        Debug.Log("Previous player: " + player.getName());
        if (player != previousPlayer || isFreshTurn) {
            isFreshTurn = false;
            coverInteractionText.GetComponent<Text>().text = "NEXT PLAYER: " + player.getName().ToUpper() + "\nPress continue when you are ready.";
            coverInteractionButton.GetComponent<Button>().onClick.AddListener(new UnityAction(HideCover));
            coverInteractionButtonText.GetComponent<Text>().text = "Continue";
            coverCanvas.SetActive(true);
        }
    }

    public static void HideCover() {
        if (coverCanvas == null) {
            coverCanvas = GameObject.Find("CoverCanvas");
            coverInteractionText = GameObject.Find("CoverCanvas/CoverInteractionPanel/CoverInteractionText");
            coverInteractionButton = GameObject.Find("CoverCanvas/CoverInteractionPanel/CoverInteractionButton");
            coverInteractionButtonText = GameObject.Find("CoverCanvas/CoverInteractionPanel/CoverInteractionButton/Text");
        }
        coverCanvas.SetActive(false);
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

    public static void DrawPlayArea(Player player) {
        DestroyPlayArea();
        foreach (Card card in player.getPlayArea().getCards())
        {
            GameObject playArea = GameObject.Find("Canvas/TabletopImage/PlayerPlayArea");
            GameObject instance = Instantiate(Resources.Load("NoDragCardPrefab", typeof(GameObject))) as GameObject;
            instance.name = card.getCardName();
            Image cardImg = instance.GetComponent<Image>();
            cardImg.sprite = Resources.Load<Sprite>("cards/" + card.cardImageName);
            instance.tag = "PlayAreaCard";
            instance.transform.SetParent(playArea.transform, false);
        }
    }

    public static void DestroyPlayArea() {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("PlayAreaCard");
        foreach (GameObject gameObj in cardObjs)
        {
            Destroy(gameObj);
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

    public static void DrawStageAreaCards(Player player) {
        DestroyStageAreaCards();
        if (BoardManagerMediator.getInstance().getCardInPlay().GetType().IsSubclassOf(typeof(Quest))) {
            Quest questInPlay = (Quest)BoardManagerMediator.getInstance().getCardInPlay();
            for (int i = 0; i < questInPlay.getNumStages(); i++)
            {
                GameObject boardAreaFoe = GameObject.Find("Canvas/TabletopImage/StageAreaFoe" + i);
                Stage currentStage = questInPlay.getStage(i);
                if (currentStage != null) {
                    if (questInPlay.getSponsor() == player 
                        || i < questInPlay.getCurrentStage().getStageNum() 
                        || (i == questInPlay.getCurrentStage().getStageNum()
                            && questInPlay.getStage(i).getStageCard().GetType().IsSubclassOf(typeof(Test)))) {
                        foreach (Card card in currentStage.getCards()) {
                            GameObject noDragInstance = Instantiate(Resources.Load("NoDragCardPrefab", typeof(GameObject))) as GameObject;
                            Image cardImg = noDragInstance.GetComponent<Image>();
                            noDragInstance.name = card.getCardName();
                            cardImg.sprite = Resources.Load<Sprite>("cards/" + card.cardImageName);
                            noDragInstance.tag = "StageCard";
                            noDragInstance.transform.SetParent(boardAreaFoe.transform, false);
                        }
                    } else {
                        GameObject noDragInstance = Instantiate(Resources.Load("NoDragCardPrefab", typeof(GameObject))) as GameObject;
                        Image cardImg = noDragInstance.GetComponent<Image>();
                        noDragInstance.name = "HiddenCard";
                        cardImg.sprite = Resources.Load<Sprite>("cards/facedown/adventure");
                        noDragInstance.tag = "StageCard";
                        noDragInstance.transform.SetParent(boardAreaFoe.transform, false);
                    }
                }
            }
        }
    }

    public static void DestroyHand()
    {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("HandCard");
        foreach (GameObject gameObj in cardObjs) {
            Destroy(gameObj);
        }
    }

    public static void DestroyRank()
    {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("RankCard");
        foreach (GameObject gameObj in cardObjs) {
            Destroy(gameObj);
        }
    }

    public static void DestroyStageAreaCards() {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("StageCard");
        foreach (GameObject gameObj in cardObjs) {
            Destroy(gameObj);
        }
    }

    public static void DestroyCardInPlay()
    {
        //print("Destroying card in play");
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("CardInPlay");
        foreach (GameObject gameObj in cardObjs) {
            Destroy(gameObj);
        }
    }

    public static void DestroyStages()
    {
        if (BoardManagerMediator.getInstance().getCardInPlay().GetType().IsSubclassOf(typeof(Quest)))
        {
            Quest questInPlay = (Quest)BoardManagerMediator.getInstance().getCardInPlay();
            for (int i = 0; i < questInPlay.getNumStages(); i++) {
                GameObject boardAreaFoe = GameObject.Find("Canvas/TabletopImage/StageAreaFoe" + i);
                Destroy(boardAreaFoe);
            }
        }
    }

    public static void DestroyDiscardArea()
    {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("DiscardedCard");
        foreach (GameObject gameObj in cardObjs)
        {
            Destroy(gameObj);
        }
        GameObject discardArea = GameObject.Find("Canvas/TabletopImage/DiscardArea");
        Destroy(discardArea);

    }

    public static void DestroyPlayerInfo() {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("PlayerInfo");
        foreach (GameObject gameObj in cardObjs)
        {
            Destroy(gameObj);
        }
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
        DestroyStageAreaCards();

        //TODO: destroy what's on the table
    }

    public static void SetupQuestPanels(int numStages){
        GameObject board = GameObject.Find("Canvas/TabletopImage");
        Debug.Log("Num stages is: " + numStages);
        float position = -462;
        for (int i = 0; i < numStages; i++){
            GameObject BoardAreaFoe = Instantiate(Resources.Load("StageAreaPrefab", typeof(GameObject))) as GameObject;

            BoardAreaFoe.name = "StageAreaFoe" + i;
            BoardAreaFoe.transform.position = new Vector3(position, BoardAreaFoe.transform.position.y, BoardAreaFoe.transform.position.z);
            BoardAreaFoe.transform.SetParent(board.transform, false);

            position += 160;
        }
    }

    public static bool QuestPanelsExist() {
        GameObject panels = GameObject.Find("Canvas/TabletopImage/StageAreaFoe0");
        return (!(panels == null));
    }

    public static List<Stage> CollectStageCards() {
        List<Stage> stages = new List<Stage>();
        if (BoardManagerMediator.getInstance().getCardInPlay().GetType().IsSubclassOf(typeof(Quest)))
        {
            Quest questInPlay = (Quest)BoardManagerMediator.getInstance().getCardInPlay();
            for (int i = 0; i < questInPlay.getNumStages(); i++)
            {
                GameObject boardAreaFoe = GameObject.Find("Canvas/TabletopImage/StageAreaFoe" + i);
                Adventure stageCard = null;
                List<Weapon> weapons = new List<Weapon>();
                foreach (Transform child in boardAreaFoe.transform) {
                    Type genericType = Type.GetType(child.name.Replace(" ", ""), true);
                    Card card = (Card)Activator.CreateInstance(genericType);
                    card.cardImageName = child.name.Replace(" ", "");
                    if (genericType.IsSubclassOf(typeof(Weapon))) {
                        weapons.Add((Weapon)card);
                    } else {
                        stageCard = (Adventure)card;
                    }
                }
                stages.Add(new Stage(stageCard, weapons, i));
            }
        }
        return stages;
    }

	public static void GetPlayArea(Player player) {
		GameObject PlayArea = GameObject.Find ("Canvas/TabletopImage/PlayerPlayArea");
		foreach (Transform child in PlayArea.transform) {
            Debug.Log("VisualCardName: " + child.name);
            foreach(Card card in player.getHand()) {
                Type cardType = card.GetType();
                if(child.name == card.getCardName()) {
                    Debug.Log("Found a match: " + card.getCardName());
                    bool amourExistsInPlayArea = false;
                    foreach (Card playAreaCard in player.getPlayArea().getCards()) {
                        Debug.Log("Play area card: " + playAreaCard.getCardName());
                        if (playAreaCard.GetType() == typeof(Amour)) {
                            amourExistsInPlayArea = true;
                            break;
                        }
                    }
                    if (!amourExistsInPlayArea) {
                        Debug.Log("Moving card");
                        player.getPlayArea().addCard(card);
                        player.RemoveCard(card);
                        break;
                    }
                }
            }
		}
	}

    public static int GetCardsNumHandArea(Player player)
    {
        int count = 0;
        GameObject HandArea = GameObject.Find("Canvas/TabletopImage/HandArea");
        foreach (Transform child in HandArea.transform)
        {
            count++;
        }

        Debug.Log("Cards in hand (UI): " + count);
        return count;
    }



    public static void SetupDiscardPanel()
    {
        Debug.Log("In setupdiscardpanel");
        DestroyDiscardArea();
        GameObject discardArea = GameObject.Find("Canvas/TabletopImage/DiscardArea");
        GameObject board = GameObject.Find("Canvas/TabletopImage");
        discardArea = Instantiate(Resources.Load("DiscardArea", typeof(GameObject))) as GameObject;
        discardArea.name = "DiscardArea";
        discardArea.transform.SetParent(board.transform, false);
    }

    public static void DisplayPlayers(){
        List<Player> players = BoardManagerMediator.getInstance().getPlayers();
        GameObject PlayersInfo = GameObject.Find("Canvas/TabletopImage/PlayersInfo");
        float position = -320;
        foreach(Player currPlayer in players){
            GameObject CurrentPlayerInfo = Instantiate(Resources.Load("PlayerInfo", typeof(GameObject))) as GameObject;
            CurrentPlayerInfo.name = "PlayerInfo" + currPlayer.getName();
            CurrentPlayerInfo.tag = "PlayerInfo";
            CurrentPlayerInfo.transform.position = new Vector3(position, CurrentPlayerInfo.transform.position.y, CurrentPlayerInfo.transform.position.z);
            Text[] texts = CurrentPlayerInfo.transform.GetComponentsInChildren<Text>();
            texts[0].text = "Player Name: " + currPlayer.getName();
            texts[1].text = "Player Rank: " + currPlayer.getRank().ToString();
            texts[2].text = "Player: " + currPlayer.getName() + " has " + currPlayer.getHand().Count.ToString() + " cards";
            CurrentPlayerInfo.transform.SetParent(PlayersInfo.transform, false);
            position += 150;
        }

    }
}
