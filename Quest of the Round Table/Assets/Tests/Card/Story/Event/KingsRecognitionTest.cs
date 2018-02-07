using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class KingsRecognitionTest {

	[Test]
	public void KingsRecognitionTestSimplePasses() {
		Assert.IsTrue (KingsRecognition.frequency == 2);
		Event kingsRecognition = new KingsRecognition ();
		Assert.AreEqual ("King's Recognition", kingsRecognition.getCardName ());

		//need to implement some sort of test case to test out processEvent function
		//Assert.IsTrue (false);
	}
}
