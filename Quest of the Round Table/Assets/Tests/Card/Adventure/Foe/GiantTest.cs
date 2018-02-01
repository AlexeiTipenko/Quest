using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class GiantTest {

	[Test]
	public void GiantTestSimplePasses() {
		Assert.IsTrue (Giant.frequency == 2);
		Foe giant = new Giant();
		Assert.AreEqual ("Giant", giant.getCardName());
	
		Assert.IsTrue (giant.getBattlePoints() == 40);
	}
}
