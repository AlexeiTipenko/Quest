using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SirTristanTest {

	[Test]
	public void SirTristanTestSimplePasses() {
		Assert.IsTrue (SirTristan.frequency == 1);
		Ally sirTristan = new SirTristan();
		Assert.AreEqual ("Sir Tristan", sirTristan.GetCardName());

		//unempowered battle points
		Assert.IsTrue (sirTristan.getBattlePoints() == 10);

		//empowered battle points
		//Assert.IsTrue (sirTristan.getBattlePoints() == 20);

		Assert.IsTrue (sirTristan.getBidPoints() == 0);
	}
}
