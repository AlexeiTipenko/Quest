using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SlayTheDragonTest {

	[Test]
	public void SlayTheDragonTestSimplePasses() {
		Assert.IsTrue (SlayTheDragon.frequency == 1);

		Quest slayTheDragon = new SlayTheDragon ();
		Assert.AreEqual ("Slay the Dragon", slayTheDragon.getCardName());
		Assert.IsTrue (slayTheDragon.getShieldsWon() == 3);

		//need to figure out the dominantFoe business before passing the test case as complete
		Assert.IsTrue(false);
	}
}
