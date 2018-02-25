using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

public class AtCamelotTest
{

    [Test]
    public void AtCamelotTestSimplePasses()
    {
        Assert.IsTrue(AtCamelot.frequency == 1);
        Tournament camelot = new AtCamelot();
        Assert.AreEqual("At Camelot", camelot.getCardName());
    }


    [Test]
    public void TestGetBonusShields()
    {
        Tournament camelot = new AtCamelot();
        Assert.IsTrue(camelot.GetBonusShields() == 3);
    }


    [Test]
    public void TestAddPlayerBattlePoints()
    {
        Tournament camelot = new AtCamelot();
        camelot.playerToPrompt = new Player("Ghandi", false);
        int points;

        List<Card> cardList1 = new List<Card>();
        cardList1.Add(new BattleAx());
        cardList1.Add(new Amour());
        cardList1.Add(new KingArthur());

        camelot.AddPlayerBattlePoints(cardList1);

        camelot.pointsDict.TryGetValue(camelot.playerToPrompt, out points);
        Assert.AreEqual(35, points);

        camelot.pointsDict.Clear();

        /*
        List<Card> cardList2 = new List<Card>();
        cardList2.Add(new Sword());
        cardList2.Add(new Boar());
        cardList2.Add(new TestOfValor());

        camelot.AddPlayerBattlePoints(cardList2);

        Assert.AreEqual(10, camelot.pointsDict);
        camelot.pointsDict.Clear();
        */
    }


    [Test]
    public void TestValidateChosenCards()
    {
        Tournament camelot = new AtCamelot();
        camelot.playerToPrompt = new Player("Ghandi", false);

        List<Card> cardList1 = new List<Card>();
        cardList1.Add(new BattleAx());
        cardList1.Add(new Amour());
        cardList1.Add(new KingArthur());

        bool validCards = camelot.ValidateChosenCards(cardList1);
        Assert.IsTrue(validCards == true);



        List<Card> cardList2 = new List<Card>();
        cardList2.Add(new Sword());
        cardList2.Add(new Boar());
        cardList2.Add(new TestOfValor());

        bool validCards2 = camelot.ValidateChosenCards(cardList2);
        Assert.IsTrue(validCards2 == false);



        List<Card> cardList3 = new List<Card>();
        cardList2.Add(new Sword());
        cardList2.Add(new Sword());
        cardList2.Add(new BattleAx());

        bool validCards3 = camelot.ValidateChosenCards(cardList2);
        Assert.IsTrue(validCards3 == false);
    }


    [Test]
    public void TestGetNextPlayer(){
        
        Tournament camelot = new AtCamelot();
        camelot.playerToPrompt = new Player("Ghandi", false);
        Player nextPlayer = new Player("Rasputin", false);

        List<Player> playersList = new List<Player>();
        playersList.Add(camelot.playerToPrompt);
        playersList.Add(nextPlayer);
        playersList.Add(new Player("Kobe", false));
        playersList.Add(new Player("LUCINE", false));

        camelot.participatingPlayers = playersList;

        Player newPlayer = camelot.GetNextPlayer(camelot.playerToPrompt);
        Assert.AreEqual(nextPlayer, newPlayer);

    }
}
