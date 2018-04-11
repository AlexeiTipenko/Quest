using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AmourTest {

	[Test]
	public void AmourTestSimplePasses() {
		Assert.IsTrue (Amour.frequency == 8);
		Amour amour = new Amour();
		Assert.AreEqual ("Amour", amour.GetCardName());
		Assert.IsTrue (amour.getBattlePoints() == 10);
		Assert.IsTrue (amour.getBidPoints() == 1);
	}
}
