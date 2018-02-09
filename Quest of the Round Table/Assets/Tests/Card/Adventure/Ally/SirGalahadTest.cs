using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SirGalahadTest {

	[Test]
	public void SirGalahadTestSimplePasses() {
		Assert.IsTrue (SirGalahad.frequency == 1);
		Ally sirGalahad = new SirGalahad();
		Assert.AreEqual ("Sir Galahad", sirGalahad.getCardName());
		Assert.IsTrue (sirGalahad.getBattlePoints() == 15);
		Assert.IsTrue (sirGalahad.getBidPoints() == 0);
	}
}
