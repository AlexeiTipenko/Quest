using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class DefendTheQueensHonorTest {

	[Test]
	public void DefendTheQueensHonorTestSimplePasses() {
		Assert.IsTrue (DefendTheQueensHonor.frequency == 1);

		Quest defendtheQueensHonor = new DefendTheQueensHonor ();
		Assert.AreEqual ("Defend the Queen's Honor", defendtheQueensHonor.GetCardName());
		Assert.IsTrue (defendtheQueensHonor.getShieldsWon() == 4);

		//need to figure out the dominantFoe business before passing the test case as complete
		//Assert.IsTrue(false);
	}
}
