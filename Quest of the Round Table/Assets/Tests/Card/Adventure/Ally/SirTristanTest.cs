using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SirTristanTest {

	[Test]
	public void SirTristanTestSimplePasses() {
		Ally sirTristan = new SirTristan();
		Assert.AreEqual ("Sir Tristan", sirTristan.getCardName());
		Assert.IsTrue (sirTristan.getBattlePoints() == 10);
		Assert.IsTrue (sirTristan.getBidPoints() == 0);
	}
}
