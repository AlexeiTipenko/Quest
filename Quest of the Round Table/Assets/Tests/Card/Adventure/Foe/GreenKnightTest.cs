using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class GreenKnightTest {

	[Test]
	public void GreenKnightTestSimplePasses() {
		Assert.IsTrue (GreenKnight.frequency == 2);
		Foe greenKnight = new GreenKnight();
		Assert.AreEqual ("Green Knight", greenKnight.getCardName());

		//Not empowered
		Assert.IsTrue (greenKnight.getBattlePoints() == 25);

		//Card is empowered (need to add to test cases so that below statement is true
		Assert.IsTrue (greenKnight.getBattlePoints() == 40);
	}
}
