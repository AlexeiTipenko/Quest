using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AtTintagelTest {

	[Test]
	public void AtTintagelTestSimplePasses() {
		Assert.IsTrue (AtTintagel.frequency == 1);
		Tournament tintagel = new AtTintagel ();
		Assert.AreEqual("At Tintagel", tintagel.GetCardName());
	}
}
