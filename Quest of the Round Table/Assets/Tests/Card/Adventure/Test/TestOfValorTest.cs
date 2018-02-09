using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestOfValorTest {

	[Test]
	public void TestOfValorTestSimplePasses() {
		Assert.IsTrue (TestOfValor.frequency == 2);

		Test testOfValor = new TestOfValor();
		Assert.AreEqual ("Test of Valor", testOfValor.getCardName ());
		Assert.IsTrue (testOfValor.getMinBidValue() == 3);
	}
}
