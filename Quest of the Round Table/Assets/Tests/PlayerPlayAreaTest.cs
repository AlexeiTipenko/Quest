using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlayerPlayAreaTest {

    private PlayerPlayArea PreparePlayArea() {
        PlayerPlayArea playArea = new PlayerPlayArea();
        playArea.AddCard(new SirTristan());
        playArea.AddCard(new QueenIseult());
        playArea.AddCard(new Amour());
        playArea.AddCard(new Amour());
        playArea.AddCard(new Excalibur());
        playArea.AddCard(new Horse());
        return playArea;
    }

	[Test]
	public void testDiscardWeapons() {
        PlayerPlayArea playArea = PreparePlayArea();
		playArea.DiscardClass (typeof(Weapon));
        foreach (Card card in playArea.getCards()) {
            Assert.AreNotEqual(card.GetType(), typeof(Excalibur));
            Assert.AreNotEqual(card.GetType(), typeof(Horse));
        }
	}

    [Test]
    public void testDiscardAmours() {
        PlayerPlayArea playArea = PreparePlayArea();
		playArea.DiscardClass(typeof(Amour));
        foreach (Card card in playArea.getCards())
        {
            Assert.AreNotEqual(card.GetType(), typeof(Amour));
        }
    }

    [Test]
    public void testDiscardAllies() {
        PlayerPlayArea playArea = PreparePlayArea();
		playArea.DiscardClass (typeof(Ally));
        foreach (Card card in playArea.getCards())
        {
            Assert.AreNotEqual(card.GetType(), typeof(SirTristan));
            Assert.AreNotEqual(card.GetType(), typeof(QueenIseult));
        }
    }

    [Test]
    public void testDiscardSpecificAlly() {
        PlayerPlayArea playArea = PreparePlayArea();
        playArea.DiscardChosenAlly(typeof(SirTristan));
        foreach (Card card in playArea.getCards())
        {
            Assert.AreNotEqual(card.GetType(), typeof(SirTristan));
        }
    }
}
