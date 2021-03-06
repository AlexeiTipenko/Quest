﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    static GameObject coverCanvas;
    static GameObject coverInteractionText;
    static GameObject coverInteractionButton;
    static GameObject coverInteractionButtonText;
    static GameObject playAreaCanvas;
    static GameObject mordredCanvas;
    static Player previousPlayer;
    static bool isFreshTurn = true;
    static bool isResolutionOfStage;

    void Start()
    {
		Logger.getInstance().info("Board manager started");
        print("Board manager started");

    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyUp ("r")) {
			BoardManagerMediator.getInstance ().cheat ("rankUp");
		} else if (Input.GetKeyUp ("s")) {
			BoardManagerMediator.getInstance ().cheat ("shieldsUp");
		} else if (Input.GetKeyUp ("p")) {
			BoardManagerMediator.getInstance ().cheat ("prosperity");
		} else if (Input.GetKeyUp ("c")) {
			BoardManagerMediator.getInstance ().cheat ("chivalrous");
		} else if (Input.GetKeyUp ("n")) {
			BoardManagerMediator.getInstance ().cheat ("nextPlayer");
		} else if (Input.GetKeyUp ("1")) {
			BoardManagerMediator.getInstance ().cheat ("scenario1");
		} else if (Input.GetKeyUp ("2")) {
			BoardManagerMediator.getInstance ().cheat ("scenario2");
		} else if (Input.GetKeyUp ("d")) {
			BoardManagerMediator.getInstance ().cheat ("discardArea");
		}
    }

    public static void SetInteractionText(String text)
    {
        Debug.Log("Setting interaction text: " + text);
        Logger.getInstance().info("Setting interaction text: " + text);
        GameObject interactionText = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionText");
        interactionText.GetComponent<Text>().text = text;
    }


    public static void SetNewInteractionText(String text)
    {
        GameObject interactionText2 = GameObject.Find("Canvas/TabletopImage/TempInteractionPanel(Clone)/InteractionText");
        interactionText2.GetComponent<Text>().text = text;
    }


    public static void SetInteractionBid(String text) {
        GameObject interactionBid = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionBid");
        GameObject interactionText = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionBid/Text");
        interactionBid.SetActive(true);
        interactionText.GetComponent<Text>().text = text;
    }

    public static void ClearInteractionBid() {
        GameObject interactionBid = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionBid");
        interactionBid.SetActive(false);
    }

    public static String GetInteractionBid() {
        GameObject interactionText = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionBid/Text");
        return interactionText.GetComponent<Text>().text;
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


    public static void SetNewInteractionButton(String text)
    {
        GameObject buttonText = GameObject.Find("Canvas/TabletopImage/TempInteractionPanel(Clone)/okButton/Text");
        GameObject okButton = GameObject.Find("Canvas/TabletopImage/TempInteractionPanel(Clone)/okButton");
        okButton.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonText.GetComponent<Text>().text = "Okay";

        okButton.SetActive(true);
        okButton.GetComponent<Button>().onClick.AddListener(new UnityAction(DestroyNewInteractionArea));
    }


    public static void ClearInteractions() {
		Debug.Log("Clearing interactions...");
        GameObject interactionText = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionText");
        GameObject button1 = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionButton1");
        GameObject button2 = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionButton2");
        GameObject interactionBid = GameObject.Find("Canvas/TabletopImage/InteractionPanel/InteractionBid");

        button1.GetComponent<Button>().onClick.RemoveAllListeners();
        button2.GetComponent<Button>().onClick.RemoveAllListeners();

        interactionText.GetComponent<Text>().text = "";
        button1.SetActive(false);
        button2.SetActive(false);
        interactionBid.SetActive(false);
		Debug.Log("Cleared interactions.");
    }

    public static void DrawCards(Player player) {
        HideStageCards();
        DestroyCards();
        DrawCover(player);
        DrawHand(player);
        DrawRank(player);
        DrawCardInPlay();
        DrawStageAreaCards(player);
        DrawPlayArea(player);
        DestroyPlayerInfo();
        DisplayPlayers();
        DisplayStageButton(BoardManagerMediator.getInstance().getPlayers());
        DisplayMordredButton(player, BoardManagerMediator.getInstance().getPlayers());
        previousPlayer = player;
        Debug.Log(player.getName() + "'s cards drawn to GUI");
        Logger.getInstance().info(player.getName() + "'s cards drawn to GUI");
    }


    public static void SetIsFreshTurn(bool isFreshTurn) {
        BoardManager.isFreshTurn = isFreshTurn;
    }

    public static void SetIsResolutionOfStage(bool isResolutionOfStage) {
        BoardManager.isResolutionOfStage = isResolutionOfStage;
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
        if (boardArea != null) {
            if (boardArea.transform.childCount != 0)
            {
                foreach (Transform child in boardArea.transform)
                {
                    child.tag = "DiscardedCard";
                    cardNames.Add(child.gameObject.name);
                }
                DestroyDiscardArea();
            }
        }

        return cardNames;
    }



    public static void ValidateSelectedAlly()
    {
        bool success = false;
        string interactionText;
        GameObject discardArea = GameObject.Find("mordredCanvas/MordredDiscardArea");

        if (discardArea != null)
        {
            if (discardArea.transform.childCount == 0)
            {
                Debug.Log("No allies were selected.");
                Logger.getInstance().warn("No allies were selected.");
                interactionText = "No allies were selected.";
            }

            else if (discardArea.transform.childCount > 1){
                Debug.Log("Too many allies were selected.");
                Logger.getInstance().warn("Too many allies were selected.");
                interactionText = "Too many allies were selected.";
            }

            else {
                interactionText = "Ally removed!";
                success = true;
            }

            if (success)
            {
                Debug.Log("Ally selection is valid.");
                Logger.getInstance().info("Ally selection is valid.");
                RemoveAlly(discardArea);
                BoardManagerMediator.getInstance().DiscardCard("Mordred", previousPlayer);
                DrawHand(previousPlayer);
                DisplayMordredButton(previousPlayer, BoardManagerMediator.getInstance().getPlayers());
            }

            InstantiateNewInteraction(interactionText);
        }
    }


    public static void RemoveAlly(GameObject discardArea) {
        string discardedAlly = discardArea.transform.GetChild(0).gameObject.name;
        Debug.Log("Discarding ally: " + discardedAlly);
        Logger.getInstance().info("Discardeding ally: " + discardedAlly);
        if (BoardManagerMediator.getInstance().IsOnlineGame())
        {
            BoardManagerMediator.getInstance().getPhotonView().RPC("DiscardChosenAlly", PhotonTargets.Others, discardedAlly);
        }
        BoardManagerMediator.getInstance().DiscardChosenAlly(discardedAlly);
    }


    public static void InstantiateNewInteraction(string text) {
        HideAllyCards();
        GameObject messagePanel = Instantiate(Resources.Load("TempInteractionPanel", typeof(GameObject))) as GameObject;
        GameObject canvasArea = GameObject.Find("Canvas/TabletopImage");
        messagePanel.transform.SetParent(canvasArea.transform, false);
        SetNewInteractionText(text);
        SetNewInteractionButton(text);
    }


    public static void DrawCover(Player player) {
        BoardManagerMediator board = BoardManagerMediator.getInstance();
        HideCover();
        if (player != previousPlayer || isFreshTurn || board.IsOnlineGame()) {
            isFreshTurn = false;
            if (board.IsOnlineGame()) {
                List<Player> players = board.getPlayers();
                int playerTurn = players.IndexOf(player) + 1;
                Logger.getInstance().info("Local player id: " + PhotonNetwork.player.ID);
                Logger.getInstance().info("Drawing for player (+ 1): " + playerTurn);
                Debug.Log("Player id: " + PhotonNetwork.player.ID);
                Debug.Log("player turn: " + playerTurn);
                if (playerTurn == PhotonNetwork.player.ID)
                {
                    Logger.getInstance().info("PLAYER PHOTON ID AND TURN IS MATCHING");
                    Debug.Log("Player photon id and turn is matching");
                    coverCanvas.SetActive(false);
                }
                else {
                    Logger.getInstance().info("Inside else statement");
                    coverCanvas.SetActive(true);
                }
            }
            else {
                Debug.Log("draw cover not online");
                coverInteractionText.GetComponent<Text>().text = "NEXT PLAYER: " + player.getName().ToUpper() + "\nPress continue when you are ready.";
                coverInteractionButton.GetComponent<Button>().onClick.AddListener(new UnityAction(HideCover));
                coverInteractionButtonText.GetComponent<Text>().text = "Continue";
                coverCanvas.SetActive(true);
            }

        }
    }

    public static void HideCover() {
        if (coverCanvas == null) {
            Debug.Log("Instantiating cover canvas...");
            coverCanvas = GameObject.Find("CoverCanvas");
            BoardManagerMediator board = BoardManagerMediator.getInstance();
            if (!board.IsOnlineGame()){
                coverInteractionText = GameObject.Find("CoverCanvas/CoverInteractionPanel/CoverInteractionText");
                coverInteractionButton = GameObject.Find("CoverCanvas/CoverInteractionPanel/CoverInteractionButton");
                coverInteractionButtonText = GameObject.Find("CoverCanvas/CoverInteractionPanel/CoverInteractionButton/Text");
            }
        }
        coverCanvas.SetActive(false);
    }

    public static void DrawHand(Player player)
    {
        DestroyHand();
        foreach (Card card in player.GetHand())
        {
            GameObject handArea = GameObject.Find("Canvas/TabletopImage/HandArea");
            GameObject instance = Instantiate(Resources.Load("CardPrefab", typeof(GameObject))) as GameObject;
            instance.name = card.GetCardName();
            Image cardImg = instance.GetComponent<Image>();
            cardImg.sprite = Resources.Load<Sprite>("cards/" + card.cardImageName);
            instance.tag = "HandCard";
            instance.transform.SetParent(handArea.transform, false);
        }
    }

    public static void DrawPlayArea(Player player) {
        DestroyPlayArea();
        foreach (Adventure card in player.getPlayArea().getCards())
        {
            Logger.getInstance().info("Player name: " + player.getName() + ", playarea card is: " + card.GetCardName() + " battlepoints are : " + card.getBattlePoints() + " bid points are: " + card.getBidPoints());
            GameObject playArea = GameObject.Find("Canvas/TabletopImage/PlayerPlayArea");
            GameObject instance = Instantiate(Resources.Load("NoDragCardPrefab", typeof(GameObject))) as GameObject;
            instance.name = card.GetCardName();
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


    public static void DestroyMordredButton()
    {
        GameObject mordredButton = GameObject.Find("Canvas/TabletopImage/MordredButton");
        Destroy(mordredButton);
    }


    public static void DestroyNewInteractionArea()
    {
        GameObject tempPanel = GameObject.Find("Canvas/TabletopImage/TempInteractionPanel(Clone)");
        Destroy(tempPanel);
    }



    public static void DrawRank(Player player)
    {
        DestroyRank();
        GameObject rankArea = GameObject.Find("Canvas/TabletopImage/RankArea");
        GameObject noDragInstance = Instantiate(Resources.Load("NoDragCardPrefab", typeof(GameObject))) as GameObject;
        Image cardImg = noDragInstance.GetComponent<Image>();
        noDragInstance.name = player.getRank().GetCardName();
        cardImg.sprite = Resources.Load<Sprite>("cards/ranks/" + player.getRank().GetCardName());
        noDragInstance.tag = "RankCard";
        noDragInstance.transform.SetParent(rankArea.transform, false);
    }

    public static void DrawStageAreaCards(Player player) {
        DestroyStageAreaCards();
		if (BoardManagerMediator.getInstance().getCardInPlay().IsQuest()) {
            Quest questInPlay = (Quest)BoardManagerMediator.getInstance().getCardInPlay();
            for (int i = 0; i < questInPlay.getNumStages(); i++)
            {
                GameObject boardAreaFoe = GameObject.Find("Canvas/TabletopImage/StageAreaFoe" + i);
                if (boardAreaFoe == null) {
                    SetupQuestPanels(questInPlay.getNumStages());
                    boardAreaFoe = GameObject.Find("Canvas/TabletopImage/StageAreaFoe" + i);
                }
                Stage currentStage = questInPlay.getStage(i);
                if (currentStage != null) {
                    if (questInPlay.getSponsor() == player 
                        || i < questInPlay.getCurrentStage().getStageNum() 
                        || (i == questInPlay.getCurrentStage().getStageNum() 
                            && isResolutionOfStage)
                        || (i == questInPlay.getCurrentStage().getStageNum() 
							&& questInPlay.getStage(i).getStageCard().IsTest() 
                            && questInPlay.getStage(i).IsInProgress())) {
                        foreach (Card card in currentStage.getCards()) {
                            Logger.getInstance().info("Stage " + i + ", card: " + card.GetCardName());
                            GameObject noDragInstance = Instantiate(Resources.Load("NoDragCardPrefab", typeof(GameObject))) as GameObject;
                            Image cardImg = noDragInstance.GetComponent<Image>();
                            noDragInstance.name = card.GetCardName();
                            cardImg.sprite = Resources.Load<Sprite>("cards/" + card.cardImageName);
                            noDragInstance.tag = "StageCard";
                            noDragInstance.transform.SetParent(boardAreaFoe.transform, false);
                        }
                        if (i == questInPlay.getCurrentStage().getStageNum()) {
                            isResolutionOfStage = false;
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
		if (BoardManagerMediator.getInstance().getCardInPlay().IsQuest())
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


    public static void DestroyMordredDiscardArea()
    {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("DiscardedCard");
        foreach (GameObject gameObj in cardObjs)
        {
            Destroy(gameObj);
        }
        GameObject discardArea = GameObject.Find("mordredCanvas/MordredDiscardArea");
        Destroy(discardArea);

    }


    public static void returnDicardedCardsToHand(){
        
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("DiscardedCard");
        GameObject handArea = GameObject.Find("Canvas/TabletopImage/HandArea");

        foreach (GameObject gameObj in cardObjs)
        {
            gameObj.transform.SetParent(handArea.transform, false);
            gameObj.tag = "Untagged";
        }
    }

    public static void DestroyPlayerInfo() {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("PlayerInfo");
        foreach (GameObject gameObj in cardObjs)
        {
            Destroy(gameObj);
        }
    }

    public static void DestroyPlayAreaCanvasCards() {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("CurrentPlayAreaCards");
        foreach (GameObject gameObj in cardObjs)
        {
            Destroy(gameObj);
        }
    }

    public static void DestroyMordredCanvasCards()
    {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("MordredAreaCards");
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
        noDragInstance.name = cardInPlay.GetCardName();
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
        HideAllyCards();
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
		if (BoardManagerMediator.getInstance().getCardInPlay().IsQuest())
        {
            Quest questInPlay = (Quest)BoardManagerMediator.getInstance().getCardInPlay();
            for (int i = 0; i < questInPlay.getNumStages(); i++)
            {
                GameObject boardAreaFoe = GameObject.Find("Canvas/TabletopImage/StageAreaFoe" + i);
                Adventure stageCard = null;
                List<Adventure> weapons = new List<Adventure>();
                foreach (Transform child in boardAreaFoe.transform) {
                    Type genericType = Type.GetType(child.name.Replace(" ", ""), true);
                    Adventure card = (Adventure)Activator.CreateInstance(genericType);
                    card.cardImageName = child.name.Replace(" ", "");
					if (card.IsWeapon()) {
                        weapons.Add(card);
                    } else {
                        stageCard = card;
                    }
                }
                stages.Add(new Stage(stageCard, weapons, i));
            }
        }
        return stages;
    }

	public static List<Adventure> GetPlayArea(Player player) {
		List<Adventure> cards = new List<Adventure> ();
		GameObject PlayArea = GameObject.Find ("Canvas/TabletopImage/PlayerPlayArea");
		foreach (Transform child in PlayArea.transform) {
			Logger.getInstance ().info ("Cards in play area: " + child.name);
            foreach(Adventure card in player.GetHand()) {
				if (child.name.Trim () == card.GetCardName ().Trim ()) {
					cards.Add (card);
					break;
				}
            }
		}
		return cards;
	}

	public static void TransferCards(Player player, List<Adventure> cards) {
		foreach (Adventure card in cards) {
			bool amourExistsInPlayArea = false;
			foreach (Adventure playAreaCard in player.getPlayArea().getCards()) {
				if (playAreaCard.IsAmour()) {
					amourExistsInPlayArea = true;
					break;
				}
			}
			if (!amourExistsInPlayArea || (amourExistsInPlayArea && !card.IsAmour())) {
				Debug.Log("Moving card from hand to play area: " + card.GetCardName());
				BoardManagerMediator board = BoardManagerMediator.getInstance ();
				if (board.IsOnlineGame ()) {
					board.getPhotonView ().RPC ("TransferCard", PhotonTargets.Others, PunManager.Serialize (player), PunManager.Serialize (card));
				}
				TransferCard (player, card);
			}
		}
	}
		
	public static void TransferCard(Player player, Adventure card) {
        player.getPlayArea().AddCard(card);
		player.RemoveCard(card);
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
        foreach(Player currPlayer in players) {
            GameObject CurrentPlayerInfo = Instantiate(Resources.Load("PlayerInfo", typeof(GameObject))) as GameObject;
            CurrentPlayerInfo.name = "PlayerInfo" + currPlayer.getName();
            CurrentPlayerInfo.tag = "PlayerInfo";
            CurrentPlayerInfo.transform.position = new Vector3(position, CurrentPlayerInfo.transform.position.y, CurrentPlayerInfo.transform.position.z);

            //Handle texts
            Text[] texts = CurrentPlayerInfo.transform.GetComponentsInChildren<Text>();
            texts[0].text = "Player: " + currPlayer.getName();
            texts[1].text = currPlayer.GetHand().Count.ToString();
            Logger.getInstance().info("Inside display player info, number of cards for player : " + currPlayer.getName() + " is: " + currPlayer.GetHand().Count);
            texts[2].text = currPlayer.getNumShields().ToString();
            Logger.getInstance().info("Inside display player info, shields for player : " + currPlayer.getName() + " is: " + currPlayer.getNumShields());

            //Handle rank images
            Image[] images = CurrentPlayerInfo.transform.GetComponentsInChildren<Image>();
            images[3].sprite = Resources.Load<Sprite>("cards/ranks/" + currPlayer.getRank().GetCardName());

            CurrentPlayerInfo.transform.SetParent(PlayersInfo.transform, false);



            position += 150;
        }

    }

    public static void DisplayStageButton(List<Player> players) {
        GameObject CanvasViewButton = GameObject.Find("Canvas/TabletopImage/Reveal");
        if (CanvasViewButton == null) {
            GameObject CanvasArea = GameObject.Find("Canvas/TabletopImage");
            GameObject ViewButton = Instantiate(Resources.Load("Reveal", typeof(GameObject))) as GameObject;
            ViewButton.name = "Reveal";

            ViewButton.transform.SetParent(CanvasArea.transform, false);

            ViewButton.GetComponent<Button>().onClick.AddListener(delegate {
                DisplayStageCards(players);
            });
        }
    }


    public static void DisplayStageCards(List<Player> players) {

        Debug.Log("Trying to display new");
        HideStageCards();

        playAreaCanvas.SetActive(true);


        for (int i = 0; i < players.Count; i++) {
            GameObject playArea = GameObject.Find("playAreaCanvas/Player" + i + "Area/CardInPlayArea");
            GameObject playAreaNames = GameObject.Find("playAreaCanvas/Player" + i + "Area");
            Text[] texts = playAreaNames.transform.GetComponentsInChildren<Text>();
            texts[0].text = "Player: " + players[i].getName();


            foreach (Card card in players[i].getPlayArea().getCards())
            {
                GameObject instance = Instantiate(Resources.Load("NoDragPlayArea", typeof(GameObject))) as GameObject;
                instance.name = card.GetCardName();
                Image cardImg = instance.GetComponent<Image>();
                cardImg.sprite = Resources.Load<Sprite>("cards/" + card.cardImageName);
                instance.tag = "CurrentPlayAreaCards";
                instance.transform.SetParent(playArea.transform, false);
            }

        }

        GameObject ExitButton = GameObject.Find("playAreaCanvas/Hide");
        ExitButton.GetComponent<Button>().onClick.AddListener(new UnityAction(HideStageCards));
    }



    public static void HideStageCards() {
        DestroyPlayAreaCanvasCards();
        if (playAreaCanvas == null)
        {
            playAreaCanvas = GameObject.Find("playAreaCanvas");
        }
        playAreaCanvas.SetActive(false);
    }



    public static void DisplayMordredButton(Player player, List<Player> players)
    {
        DestroyMordredButton();
        bool displayButton = false;

        foreach (Card card in player.GetHand())
        {
            if (card.GetType() == typeof(Mordred)) {
                displayButton = true;
                Debug.Log("Mordred is in current player's hand!");
            }
        }

        if (displayButton) {

            Logger.getInstance().info("Adding button for Mordred's special ability.");
            GameObject CanvasViewButton = GameObject.Find("Canvas/TabletopImage/MordredButton");
            if (CanvasViewButton == null)
            {
                GameObject CanvasArea = GameObject.Find("Canvas/TabletopImage");
                GameObject ViewButton = Instantiate(Resources.Load("MordredButton", typeof(GameObject))) as GameObject;
                ViewButton.name = "MordredButton";

                ViewButton.transform.SetParent(CanvasArea.transform, false);

                ViewButton.GetComponent<Button>().onClick.AddListener(delegate {
                    DisplayAllyCards(player, players);
                });
            }
        }
    }

    public static void DisplayAllyCards(Player player, List<Player> players)
    {
        Debug.Log("Displaying ally card canvas area.");
        Logger.getInstance().info("Displaying ally card canvas area.");
        HideAllyCards();

        mordredCanvas.SetActive(true);

        for (int i = 0; i < players.Count; i++)
        {
            GameObject mordredArea = GameObject.Find("mordredCanvas/Player" + i + "Area/CardInPlayArea");
            GameObject mordredAreaNames = GameObject.Find("mordredCanvas/Player" + i + "Area");
            Text[] texts = mordredAreaNames.transform.GetComponentsInChildren<Text>();
            texts[0].text = "Player: " + players[i].getName();


            foreach (Card card in players[i].getPlayArea().getCards())
            {
                if (card.IsAlly())
                {
                    GameObject instance = Instantiate(Resources.Load("DraggablePlayArea", typeof(GameObject))) as GameObject;
                    instance.name = card.GetCardName();
                    Image cardImg = instance.GetComponent<Image>();
                    cardImg.sprite = Resources.Load<Sprite>("cards/" + card.cardImageName);
                    instance.tag = "MordredAreaCards";
                    instance.transform.SetParent(mordredArea.transform, false);
                }
            }

        }

        SetupMordredDiscardPanel();

        GameObject SubmitButton = GameObject.Find("mordredCanvas/Submit");
        SubmitButton.GetComponent<Button>().onClick.AddListener(new UnityAction(ValidateSelectedAlly));

        GameObject ExitButton = GameObject.Find("mordredCanvas/Cancel");
        ExitButton.GetComponent<Button>().onClick.AddListener(new UnityAction(HideAllyCards));
    }



    public static void SetupMordredDiscardPanel()
    {
        DestroyMordredDiscardArea();
        GameObject mordredDiscardArea = Instantiate(Resources.Load("MordredDiscardArea", typeof(GameObject))) as GameObject;
        mordredDiscardArea.name = "MordredDiscardArea";
        mordredDiscardArea.transform.SetParent(mordredCanvas.transform, false);
    }


    public static void HideAllyCards()
    {
        Debug.Log("Destroying ally card canvas area.");
        Logger.getInstance().info("Destroying ally card canvas area.");

        DestroyMordredCanvasCards();
        if (mordredCanvas == null)
        {
            mordredCanvas = GameObject.Find("mordredCanvas");
        }
        mordredCanvas.SetActive(false);
    }
}
