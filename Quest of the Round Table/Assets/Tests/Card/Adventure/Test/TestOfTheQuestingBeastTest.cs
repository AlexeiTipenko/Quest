using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestOfTheQuestingBeastTest {

	[Test]
	public void TestOfTheQuestingBeastTestSimplePasses() {
		Assert.IsTrue (TestOfTheQuestingBeast.frequency == 2);

		Test testOfTheQuestingBeast = new TestOfTheQuestingBeast();
		Assert.AreEqual ("Test of the Questing Beast", testOfTheQuestingBeast.getCardName ());

		//If not condition, bid == 3
		Assert.IsTrue (testOfTheQuestingBeast.getMinBidValue () == 3);

		//If condition, then bid == 4
		//Assert.IsTrue (testOfTheQuestingBeast.getMinBidValue () == 4);
	}
}
