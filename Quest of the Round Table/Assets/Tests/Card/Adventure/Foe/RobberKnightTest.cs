using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class RobberKnightTest {

	[Test]
	public void RobberKnightTestSimplePasses() {
		Assert.IsTrue (RobberKnight.frequency == 7);
		Foe robberKnight = new RobberKnight();
		Assert.AreEqual ("Robber Knight", robberKnight.getCardName());

		//Not empowered
		Assert.IsTrue (robberKnight.getBattlePoints() == 15);
	}
}
