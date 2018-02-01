using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SaxonsTest {

	[Test]
	public void SaxonsTestSimplePasses() {
		Assert.IsTrue (SaxonKnight.frequency == 5);
		Foe saxons = new Saxons();
		Assert.AreEqual ("Saxons", saxons.getCardName());

		//Not empowered
		Assert.IsTrue (saxons.getBattlePoints() == 10);

		//Card is empowered (need to add to test cases so that below statement is true
		Assert.IsTrue (saxons.getBattlePoints() == 20);
	}
}
