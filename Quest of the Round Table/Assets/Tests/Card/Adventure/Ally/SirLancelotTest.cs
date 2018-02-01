using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SirLancelotTest {

	[Test]
	public void SirLancelotTestSimplePasses() {
		Assert.IsTrue (SirLancelot.frequency == 1);
		Ally sirLancelot = new SirLancelot();
		Assert.AreEqual ("Sir Lancelot", sirLancelot.getCardName());

		//unempowered battle points
		Assert.IsTrue (sirLancelot.getBattlePoints() == 15);

		//empowered battle points
		Assert.IsTrue (sirLancelot.getBattlePoints() == 25);

		Assert.IsTrue (sirLancelot.getBidPoints() == 0);
	}
}
