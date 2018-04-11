using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BoarTest {

	[Test]
	public void BoarTestSimplePasses() {
		Assert.IsTrue (Boar.frequency == 4);
		Foe boar = new Boar();
		Assert.AreEqual ("Boar", boar.GetCardName());

		//Not empowered
		Assert.IsTrue (boar.getBattlePoints() == 5);

		//Card is empowered (need to add to test cases so that below statement is true
		//Assert.IsTrue (boar.getBattlePoints() == 15);
	}
}
