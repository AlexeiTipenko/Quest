using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SearchForTheQuestingBeastTest {

	[Test]
	public void SearchForTheQuestingBeastTestSimplePasses() {
		Assert.IsTrue (SearchForTheQuestingBeast.frequency == 1);

		Quest searchForTheQuestingBeast = new SearchForTheQuestingBeast ();
		Assert.AreEqual ("Search for the Questing Beast", searchForTheQuestingBeast.getCardName());
		Assert.IsTrue (searchForTheQuestingBeast.getShieldsWon() == 4);

		//need to figure out the dominantFoe business before passing the test case as complete
		Assert.IsTrue(false);
	}
}
