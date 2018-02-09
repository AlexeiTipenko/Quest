using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestOfTemptationTest {

	[Test]
	public void TestOfTemptationTestSimplePasses() {
		Assert.IsTrue (TestOfTemptation.frequency == 2);

		Test testOfTemptation = new TestOfTemptation();
		Assert.AreEqual ("Test of Temptation", testOfTemptation.getCardName ());
		Assert.IsTrue (testOfTemptation.getMinBidValue () == 3);
	}
}
