using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AtYorkTest {

	[Test]
	public void AtYorkTestSimplePasses() {
		Assert.IsTrue (AtYork.frequency == 1);
		Tournament york = new AtYork ();
		Assert.AreEqual("At York", york.GetCardName());
	}
}
