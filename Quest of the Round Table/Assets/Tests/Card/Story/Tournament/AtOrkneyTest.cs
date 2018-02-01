using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AtOrkneyTest {

	[Test]
	public void AtOrkneyTestSimplePasses() {
		Assert.IsTrue (AtOrkney.frequency == 1);

		Tournament orkney = new AtOrkney ();

		Assert.AreEqual("At Orkney", orkney.getCardName());
		Assert.IsTrue(orkney.getBonusShields() == 2);
	}
}
