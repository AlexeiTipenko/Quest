using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ThievesTest {

	[Test]
	public void ThievesTestSimplePasses() {
		Assert.IsTrue (Thieves.frequency == 8);
		Foe thieves = new Thieves();
		Assert.AreEqual ("Thieves", thieves.getCardName());
		Assert.IsTrue (thieves.getBattlePoints() == 5);
	}
}
