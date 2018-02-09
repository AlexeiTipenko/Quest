using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MordredTest {

	[Test]
	public void MordredTestSimplePasses() {
		Assert.IsTrue (Mordred.frequency == 4);
		Foe mordred = new Mordred();
		Assert.AreEqual ("Mordred", mordred.getCardName());

		//Not empowered
		Assert.IsTrue (mordred.getBattlePoints() == 30);

		//TODO: Test Mordred's special ability
		//Assert.IsTrue(false);
	}
}
