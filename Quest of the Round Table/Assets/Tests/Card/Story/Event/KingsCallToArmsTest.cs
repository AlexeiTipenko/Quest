using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class KingsCallToArmsTest {

	//Event description: The highest ranked player(s) must place 1 weapon in the discard pile. If unable to do so, 2 Foe Cards must be discarded.
	[Test]
	public void KingsCallToArmsTestSimplePasses() {
		Assert.IsTrue (KingsCallToArms.frequency == 1);
		Event kingsCallToArms = new KingsCallToArms ();
		Assert.AreEqual ("King's Call to Arms", kingsCallToArms.getCardName ());



		//need to implement some sort of test case to test out processEvent function
		//Assert.IsTrue (false);
	}
}
