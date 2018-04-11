using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestOfTheGreenKnightTest {

	[Test]
	public void TestOfTheGreenKnightTestSimplePasses() {
		Assert.IsTrue (TestOfTheGreenKnight.frequency == 1);

		Quest testOfTheGreenKnight = new TestOfTheGreenKnight ();
		Assert.AreEqual ("Test of the Green Knight", testOfTheGreenKnight.GetCardName());
		Assert.IsTrue (testOfTheGreenKnight.getShieldsWon() == 4);

		//need to figure out the dominantFoe business before passing the test case as complete
		//Assert.IsTrue(false);
	}
}
