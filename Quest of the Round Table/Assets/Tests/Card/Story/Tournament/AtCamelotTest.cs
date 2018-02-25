using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

public class AtCamelotTest  {

	[Test]
	public void AtCamelotTestSimplePasses() {
		Assert.IsTrue (AtCamelot.frequency == 1);

		Tournament camelot = new AtCamelot ();

		Assert.AreEqual("At Camelot", camelot.getCardName());
		//Assert.IsTrue(camelot.getBonusShields() == 3);
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
        cardList2.Add(new BattleAx());
        cardList2.Add(new Amour());
        cardList2.Add(new KingArthur());

        camelot.AddPlayerBattlePoints(cardList2);

        Assert.AreEqual(3, camelot.pointsDict);
        camelot.pointsDict.Clear();
*/


        /*
        foreach (KeyValuePair<Player, int> entry in camelot.pointsDict)
        {
            Assert.IsTrue(entry.Value == );
        }
        */
    }

    /*
    [Test]
    public void TeststartBehaviour()
    {
        Player player = new Player("Ghandi", false);
        Tournament camelot = new AtCamelot();
        camelot.setOwner(player);
        camelot.startBehaviour();

        Assert.IsTrue(camelot.sponsor != null);
        Assert.IsTrue(camelot.playerToPrompt == camelot.sponsor);
    }
    */




}
