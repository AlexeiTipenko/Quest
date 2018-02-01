using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SirPercivalTest {

	[Test]
	public void SirPercivalTestSimplePasses() {
		Ally sirPercival = new SirPercival();
		Assert.AreEqual ("Sir Percival", sirPercival.getCardName());
		Assert.IsTrue (sirPercival.getBattlePoints() == 5);
		Assert.IsTrue (sirPercival.getBidPoints() == 0);
	}
}
