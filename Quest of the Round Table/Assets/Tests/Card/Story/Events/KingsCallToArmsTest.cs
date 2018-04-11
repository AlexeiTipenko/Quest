using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class KingsCallToArmsTest {

	//Event description: The highest ranked player(s) must place 1 weapon in the discard pile. If unable to do so, 2 Foe Cards must be discarded.
	[Test]
	public void KingsCallToArmsTestSimplePasses() {
		Assert.IsTrue (KingsCallToArms.frequency == 1);
		KingsCallToArms kingsCallToArms = new KingsCallToArms ();
		Assert.AreEqual ("King's Call to Arms", kingsCallToArms.GetCardName ());
	}

	//No behaviour test due to the event being entirely UI based
}
