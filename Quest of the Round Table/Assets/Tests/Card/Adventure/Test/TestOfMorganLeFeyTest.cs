using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestOfMorganLeFeyTest {

	[Test]
	public void TestOfMorganLeFeyTestSimplePasses() {
		Assert.IsTrue (TestOfMorganLeFey.frequency == 2);

		Test testOfMorganLeFey = new TestOfMorganLeFey();
		Assert.AreEqual ("Test of Morgan LeFey", testOfMorganLeFey.getCardName ());
		Assert.IsTrue (testOfMorganLeFey.getMinBidValue () == 3);
	}
}
