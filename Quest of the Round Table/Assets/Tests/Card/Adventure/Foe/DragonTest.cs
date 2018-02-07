using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class DragonTest {

	[Test]
	public void DragonTestSimplePasses() {
		Assert.IsTrue (Dragon.frequency == 1);
		Foe dragon = new Dragon();
		Assert.AreEqual ("Dragon", dragon.getCardName());

		//Not empowered
		Assert.IsTrue (dragon.getBattlePoints() == 50);

		//Card is empowered (need to add to test cases so that below statement is true
		//Assert.IsTrue (dragon.getBattlePoints() == 15);
	}
}
