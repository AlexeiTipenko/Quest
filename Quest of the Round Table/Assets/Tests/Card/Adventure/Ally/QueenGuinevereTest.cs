using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class QueenGuinevereTest {

	[Test]
	public void QueenGuinevereTestSimplePasses() {
		Assert.IsTrue (QueenGuinevere.frequency == 1);
		Ally queenguinevere = new QueenGuinevere();
		Assert.AreEqual ("Queen Guinevere", queenguinevere.getCardName());
		Assert.IsTrue (queenguinevere.getBattlePoints() == 0);
		Assert.IsTrue (queenguinevere.getBidPoints() == 3);
	}
}
