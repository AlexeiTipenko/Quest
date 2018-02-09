using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BlackKnightTest {

	[Test]
	public void BlackKnightTestSimplePasses() {
		Assert.IsTrue (BlackKnight.frequency == 3);
		Foe blackKnight = new BlackKnight();
		Assert.AreEqual ("Black Knight", blackKnight.getCardName());
	
		//Not empowered
		Assert.IsTrue (blackKnight.getBattlePoints() == 25);

		//Card is empowered (need to add to test cases so that below statement is true
		//Assert.IsTrue (blackKnight.getBattlePoints() == 35);
	}
}
