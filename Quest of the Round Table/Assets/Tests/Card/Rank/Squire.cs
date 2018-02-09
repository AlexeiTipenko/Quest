using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SquireTest {

	[Test]
	public void SquireTestSimplePasses() {
		Assert.IsTrue (Squire.frequency == 4);
		Squire squire = new Squire ();
		Assert.AreEqual ("Squire", squire.getCardName());
		Assert.IsTrue (squire.getBattlePoints() == 5);
	}
}
