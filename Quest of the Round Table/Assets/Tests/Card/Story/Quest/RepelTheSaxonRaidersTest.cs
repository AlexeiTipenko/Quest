using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class RepelTheSaxonRaidersTest {

	[Test]
	public void RepelTheSaxonRaidersTestSimplePasses() {
		Assert.IsTrue (RepelTheSaxonRaiders.frequency == 2);

		Quest repelTheSaxonRaiders = new RepelTheSaxonRaiders ();
		Assert.AreEqual ("Repel the Saxon Raiders", repelTheSaxonRaiders.GetCardName());
		Assert.IsTrue (repelTheSaxonRaiders.getShieldsWon() == 2);

		//need to figure out the dominantFoe business before passing the test case as complete
		//Assert.IsTrue(false);
	}
}
