using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ChampionKnightTest {

	[Test]
	public void ChampionKnightTestSimplePasses() {
		Assert.IsTrue (ChampionKnight.frequency == 4);
		Rank championKnight = new ChampionKnight ();
		Assert.AreEqual ("Champion Knight", championKnight.GetCardName());
		Assert.IsTrue (championKnight.getBattlePoints() == 20);
	}
}
