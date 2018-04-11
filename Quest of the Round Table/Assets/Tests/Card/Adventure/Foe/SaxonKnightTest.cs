using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SaxonKnightTest {

	[Test]
	public void SaxonKnightTestSimplePasses() {
		Assert.IsTrue (SaxonKnight.frequency == 8);
		Foe saxonKnight = new SaxonKnight();
		Assert.AreEqual ("Saxon Knight", saxonKnight.GetCardName());

		//Not empowered
		Assert.IsTrue (saxonKnight.getBattlePoints() == 15);

		//Card is empowered (need to add to test cases so that below statement is true
		//Assert.IsTrue (saxonKnight.getBattlePoints() == 25);
	}
}
