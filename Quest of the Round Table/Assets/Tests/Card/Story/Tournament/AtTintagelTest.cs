using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AtTintagelTest {

	[Test]
	public void AtTintagelTestSimplePasses() {
		Assert.IsTrue (AtTintagel.frequency == 1);

		Tournament tintagel = new AtTintagel ();

		Assert.AreEqual("At Tintagel", tintagel.getCardName());
		//Assert.IsTrue(tintagel.getBonusShields() == 1);
	}
}
