using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class KingsRecognitionTest {

	[Test]
	public void KingsRecognitionTestSimplePasses() {
		Assert.IsTrue (KingsCallToArms.frequency == 1);
		Event kingsCallToArms = new KingsCallToArms ();
		Assert.AreEqual ("King's Call To Arms", kingsCallToArms.getCardName ());

		//need to implement some sort of test case to test out processEvent function
		Assert.IsTrue (false);
	}
}
