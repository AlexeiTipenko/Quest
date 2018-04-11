using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BoarHuntTest {

	[Test]
	public void BoarHuntTestSimplePasses() {
		Assert.IsTrue (BoarHunt.frequency == 2);

		Quest boarHunt = new BoarHunt ();
		Assert.AreEqual ("Boar Hunt", boarHunt.GetCardName());
		Assert.IsTrue (boarHunt.getShieldsWon() == 2);

		//need to figure out the dominantFoe business before passing the test case as complete
		//Assert.IsTrue(false);
	}
}
