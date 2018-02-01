using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlagueTest {

	[Test]
	public void PlagueTestSimplePasses() {
		Assert.IsTrue (Plague.frequency == 1);
		Event plague = new Plague ();
		Assert.AreEqual ("Plague", plague.getCardName ());

		//need to implement some sort of test case to test out processEvent function
		Assert.IsTrue (false);
	}
}
