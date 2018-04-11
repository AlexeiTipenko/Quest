using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class EvilKnightTest {

	[Test]
	public void EvilKnightTestSimplePasses() {
		Assert.IsTrue (EvilKnight.frequency == 6);
		Foe evilKnight = new EvilKnight();
		Assert.AreEqual ("Evil Knight", evilKnight.GetCardName());

		//Not empowered
		Assert.IsTrue (evilKnight.getBattlePoints() == 20);

		//Card is empowered (need to add to test cases so that below statement is true
		//Assert.IsTrue (evilKnight.getBattlePoints() == 30);
	}
}
