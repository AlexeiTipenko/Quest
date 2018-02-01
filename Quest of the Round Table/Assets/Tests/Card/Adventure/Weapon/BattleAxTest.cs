using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BattleAxeTest {

	[Test]
	public void BattleAxeTestSimplePasses() {
		Assert.IsTrue (BattleAx.frequency == 8);
		Weapon battleAx = new BattleAx ();
		Assert.AreEqual ("Battle Ax", battleAx.getCardName ());
		Assert.IsTrue (battleAx.getBattlePoints == 15);
	}
}
