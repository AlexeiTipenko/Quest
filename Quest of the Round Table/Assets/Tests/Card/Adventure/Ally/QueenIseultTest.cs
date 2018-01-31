using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class QueenIseultTest {

	[Test]
	public void QueenIseultTestSimplePasses() {
		Ally queenIseult = new QueenIseult();
		Assert.AreEqual ("Queen Iseult", queenIseult.getCardName());
		Assert.IsTrue (queenIseult.getBattlePoints() == 0);
		Assert.IsTrue (queenIseult.getBidPoints() == 2);
	}
}
