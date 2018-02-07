using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class QueenIseultTest {

	[Test]
	public void QueenIseultTestSimplePasses() {
		Assert.IsTrue (QueenIseult.frequency == 1);
		Ally queenIseult = new QueenIseult();
		Assert.AreEqual ("Queen Iseult", queenIseult.getCardName());
		Assert.IsTrue (queenIseult.getBattlePoints() == 0);

		//unempowered bid points
		Assert.IsTrue (queenIseult.getBidPoints() == 2);

		//empowered bid points 
		//Assert.IsTrue (queenIseult.getBidPoints() == 4);
	}
}
