using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SirGalahadTest {

	[Test]
	public void SirGalahadTestSimplePasses() {
		Ally sirGalahad = new SirGalahad();
		Assert.AreEqual ("Sir Galahad", sirGalahad.getCardName());
		Assert.IsTrue (sirGalahad.getBattlePoints() == 10);
		Assert.IsTrue (sirGalahad.getBidPoints() == 0);
	}
}
