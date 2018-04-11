using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class KnightTest {

	[Test]
	public void KnightTestSimplePasses() {
		Assert.IsTrue (Knight.frequency == 4);
		Rank knight = new Knight ();
		Assert.AreEqual ("Knight", knight.GetCardName());
		Assert.IsTrue (knight.getBattlePoints() == 10);
	}
}
