using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ExcaliburTest {

	[Test]
	public void ExcaliburTestSimplePasses() {
		Assert.IsTrue (Excalibur.frequency == 2);
		Weapon excalibur = new Excalibur ();
		Assert.AreEqual ("Excalibur", excalibur.GetCardName ());
		Assert.IsTrue (excalibur.getBattlePoints() == 30);
	}
}
