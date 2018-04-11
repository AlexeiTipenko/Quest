using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class DaggerTest {

	[Test]
	public void DaggerTestSimplePasses() {
		Assert.IsTrue (Dagger.frequency == 6);
		Weapon dagger = new Dagger ();
		Assert.AreEqual ("Dagger", dagger.GetCardName ());
		Assert.IsTrue (dagger.getBattlePoints() == 5);
	}
}
