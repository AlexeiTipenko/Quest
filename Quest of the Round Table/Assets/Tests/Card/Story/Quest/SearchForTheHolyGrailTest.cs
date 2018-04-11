using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SearchForTheHolyGrailTest {

	[Test]
	public void SearchForTheHolyGrailTestSimplePasses() {
		Assert.IsTrue (SearchForTheHolyGrail.frequency == 1);

		Quest searchForTheHolyGrail = new SearchForTheHolyGrail ();
		Assert.AreEqual ("Search for the Holy Grail", searchForTheHolyGrail.GetCardName());
		Assert.IsTrue (searchForTheHolyGrail.getShieldsWon() == 5);

		//need to figure out the dominantFoe business before passing the test case as complete
		//Assert.IsTrue(false);
	}
}
