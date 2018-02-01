using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class KingArthurTest {

	[Test]
	public void KingArthurTestSimplePasses() {
		Assert.IsTrue (KingArthur.frequency == 1);
		Ally kingArthur = new KingArthur();
		Assert.AreEqual ("King Arthur", kingArthur.getCardName());
		Assert.IsTrue (kingArthur.getBattlePoints() == 10);
		Assert.IsTrue (kingArthur.getBidPoints() == 4);
	}
}
