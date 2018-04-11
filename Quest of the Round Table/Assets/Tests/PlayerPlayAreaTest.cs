using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlayerPlayAreaTest {

    private PlayerPlayArea PreparePlayArea() {
        PlayerPlayArea playArea = new PlayerPlayArea();
        playArea.addCard(new SirTristan());
        playArea.addCard(new QueenIseult());
        playArea.addCard(new Amour());
        playArea.addCard(new Amour());
        playArea.addCard(new Excalibur());
        playArea.addCard(new Horse());
        return playArea;
    }

	[Test]
	public void testDiscardWeapons() {
        PlayerPlayArea playArea = PreparePlayArea();
        playArea.discardWeapons();
        foreach (Card card in playArea.getCards()) {
            Assert.AreNotEqual(card.GetType(), typeof(Excalibur));
            Assert.AreNotEqual(card.GetType(), typeof(Horse));
        }
	}

    [Test]
    public void testDiscardAmours() {
        PlayerPlayArea playArea = PreparePlayArea();
        playArea.discardAmours();
        foreach (Card card in playArea.getCards())
        {
            Assert.AreNotEqual(card.GetType(), typeof(Amour));
        }
    }

    [Test]
    public void testDiscardAllies() {
        PlayerPlayArea playArea = PreparePlayArea();
        playArea.discardAllies();
        foreach (Card card in playArea.getCards())
        {
            Assert.AreNotEqual(card.GetType(), typeof(SirTristan));
            Assert.AreNotEqual(card.GetType(), typeof(QueenIseult));
        }
    }

    [Test]
    public void testDiscardSpecificAlly() {
        PlayerPlayArea playArea = PreparePlayArea();
        playArea.discardAlly(typeof(SirTristan));
        foreach (Card card in playArea.getCards())
        {
            Assert.AreNotEqual(card.GetType(), typeof(SirTristan));
        }
    }
}
