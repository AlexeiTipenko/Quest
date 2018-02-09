using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class VanquishKingArthursEnemiesTest {

	[Test]
	public void VanquishKingArthursEnemiesTestSimplePasses() {
		Assert.IsTrue (VanquishKingArthursEnemies.frequency == 2);

		Quest vanquishKingArthursEnemies = new VanquishKingArthursEnemies ();
		Assert.AreEqual ("Vanquish King Arthur's Enemies", vanquishKingArthursEnemies.getCardName());
		Assert.IsTrue (vanquishKingArthursEnemies.getShieldsWon() == 3);

		//need to figure out the dominantFoe business before passing the test case as complete
		//Assert.IsTrue(false);
	}
}
