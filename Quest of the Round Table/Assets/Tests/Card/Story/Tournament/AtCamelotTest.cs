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
        Assert.AreEqual("At Camelot", camelot.GetCardName());
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
        camelot.playerToPrompt = new HumanPlayer("Ghandi");
        int points;

        List<Adventure> cardList1 = new List<Adventure>();
        cardList1.Add(new BattleAx());
        cardList1.Add(new Amour());
        cardList1.Add(new KingArthur());

        camelot.AddPlayerBattlePoints(cardList1);
    }


    [Test]
    public void TestValidateChosenCards()
    {
        Tournament camelot = new AtCamelot();
        camelot.playerToPrompt = new HumanPlayer("Ghandi");

        List<Adventure> cardList1 = new List<Adventure>();
        cardList1.Add(new BattleAx());
        cardList1.Add(new Amour());
        cardList1.Add(new KingArthur());

        bool validCards = camelot.ValidateChosenCards(cardList1);
        Assert.IsTrue(validCards == true);



        List<Adventure> cardList2 = new List<Adventure>();
        cardList2.Add(new Sword());
        cardList2.Add(new Boar());
        cardList2.Add(new TestOfValor());

        bool validCards2 = camelot.ValidateChosenCards(cardList2);
        Assert.IsTrue(validCards2 == false);



        cardList2.Add(new Sword());
        cardList2.Add(new Sword());
        cardList2.Add(new BattleAx());

        bool validCards3 = camelot.ValidateChosenCards(cardList2);
        Assert.IsTrue(validCards3 == false);
    }


    [Test]
    public void TestGetNextPlayer(){
        
        Tournament camelot = new AtCamelot();
        camelot.playerToPrompt = new HumanPlayer("Ghandi");
        Player nextPlayer = new HumanPlayer("Rasputin");

        List<Player> playersList = new List<Player>();
        playersList.Add(camelot.playerToPrompt);
        playersList.Add(nextPlayer);
        playersList.Add(new HumanPlayer("Kobe"));
        playersList.Add(new HumanPlayer("LUCINE"));

        camelot.participatingPlayers = playersList;

        Player newPlayer = camelot.GetNextPlayer(camelot.playerToPrompt);
        Assert.AreEqual(nextPlayer, newPlayer);

    }
}
