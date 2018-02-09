using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SirPercivalTest {

	[Test]
	public void SirPercivalTestSimplePasses() {
		Assert.IsTrue (SirPercival.frequency == 1);
		Ally sirPercival = new SirPercival();
		Assert.AreEqual ("Sir Percival", sirPercival.getCardName());

		//unempowered battle points
		Assert.IsTrue (sirPercival.getBattlePoints() == 5);

		//empowered battle points
		//Assert.IsTrue (sirPercival.getBattlePoints() == 25);

		Assert.IsTrue (sirPercival.getBidPoints() == 0);
	}
}
