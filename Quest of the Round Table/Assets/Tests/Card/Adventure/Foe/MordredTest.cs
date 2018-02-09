using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MordredTest {

	[Test]
	public void MordredTestSimplePasses() {
<<<<<<< HEAD
		Assert.IsTrue (Mordred.frequency == 4);
=======
		Assert.IsTrue (Mordred.frequency == 2);
>>>>>>> e14cab5b8bd1508c6c794574e09ce8b9b07fb0cb
		Foe mordred = new Mordred();
		Assert.AreEqual ("Mordred", mordred.getCardName());

		//Not empowered
		Assert.IsTrue (mordred.getBattlePoints() == 30);

		//TODO: Test Mordred's special ability
		//Assert.IsTrue(false);
	}
}
