using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class HorseTest {

	[Test]
	public void HorseTestSimplePasses() {
		Assert.IsTrue (Horse.frequency == 11);
		Weapon horse = new Horse ();
		Assert.AreEqual ("Horse", horse.getCardName ());
		Assert.IsTrue (horse.getBattlePoints == 10);
	}
}
