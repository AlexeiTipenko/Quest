using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class JourneyThroughTheEnchantedForestTest {

	[Test]
	public void JourneyThroughTheEnchantedForestTestSimplePasses() {
		Assert.IsTrue (JourneyThroughTheEnchantedForest.frequency == 1);

		Quest journeyThroughTheEnchantedForest = new JourneyThroughTheEnchantedForest ();
		Assert.AreEqual ("Journey through the Enchanted Forest", journeyThroughTheEnchantedForest.GetCardName());
		Assert.IsTrue (journeyThroughTheEnchantedForest.getShieldsWon() == 3);

		//need to figure out the dominantFoe business before passing the test case as complete
		//Assert.IsTrue(false);
	}
}
