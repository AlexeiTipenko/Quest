using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SwordTest {

	[Test]
	public void SwordTestSimplePasses() {
		Assert.IsTrue (Sword.frequency == 16);
		Weapon sword = new Sword ();
		Assert.AreEqual ("Sword", sword.GetCardName ());
		Assert.IsTrue (sword.getBattlePoints() == 10);
	}
}
