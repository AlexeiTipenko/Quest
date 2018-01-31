using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MerlinTest {

	[Test]
	public void MerlinTestSimplePasses() {
		Ally merlin = new Merlin();
		Assert.AreEqual ("Merlin", merlin.getCardName());
	}
}
