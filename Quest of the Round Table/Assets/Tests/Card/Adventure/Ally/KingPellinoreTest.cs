using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class KingPellinoreTest {

	[Test]
	public void KingPellinoreTestSimplePasses() {
		Assert.IsTrue (KingPellinore.frequency == 1);
		Ally kingPellinore = new KingPellinore();
		Assert.AreEqual ("King Pellinore", kingPellinore.GetCardName());
		Assert.IsTrue (kingPellinore.getBattlePoints() == 10);
		Assert.IsTrue (kingPellinore.getBidPoints() == 0);
	}
}
