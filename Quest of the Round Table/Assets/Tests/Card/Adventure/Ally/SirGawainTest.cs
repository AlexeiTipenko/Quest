using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SirGawainTest {

	[Test]
	public void SirGawainTestSimplePasses() {
		Assert.IsTrue (SirGawain.frequency == 1);
		Ally sirGawain = new SirGawain();
		Assert.AreEqual ("Sir Gawain", sirGawain.getCardName());

		//unempowered battle points
		Assert.IsTrue (sirGawain.getBattlePoints() == 10);

		//empowered battle points
		Assert.IsTrue (sirGawain.getBattlePoints() == 20);


		Assert.IsTrue (sirGawain.getBidPoints() == 0);
	}
}
