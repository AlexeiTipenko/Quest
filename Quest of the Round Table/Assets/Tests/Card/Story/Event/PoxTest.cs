using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PoxTest {

	[Test]
	public void PoxTestSimplePasses() {
		Assert.IsTrue (Pox.frequency == 1);
		Event pox = new Pox ();
		Assert.AreEqual ("Pox", pox.getCardName ());

		//need to implement some sort of test case to test out processEvent function
		//Assert.IsTrue (false);
	}
}
