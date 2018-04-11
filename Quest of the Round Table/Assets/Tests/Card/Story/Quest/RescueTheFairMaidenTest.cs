using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class RescueTheFairMaidenTest {

	[Test]
	public void RescueTheFairMaidenTestSimplePasses() {
		Assert.IsTrue (RescueTheFairMaiden.frequency == 1);

		Quest rescueTheFairMaiden = new RescueTheFairMaiden ();
		Assert.AreEqual ("Rescue the Fair Maiden", rescueTheFairMaiden.GetCardName());
		Assert.IsTrue (rescueTheFairMaiden.getShieldsWon() == 3);

		//need to figure out the dominantFoe business before passing the test case as complete
		//Assert.IsTrue(false);
	}
}
