using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class LanceTest {

	[Test]
	public void LanceTestSimplePasses() {
		Assert.IsTrue (Lance.frequency == 6);
		Weapon lance = new Lance ();
		Assert.AreEqual ("Lance", lance.getCardName ());
		Assert.IsTrue (lance.getBattlePoints() == 20);
	}
}
