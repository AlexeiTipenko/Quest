using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ChivalrousDeedTest {

	//Event description: Player with both lowest rank and least amount of shields, receives 3 shields.
	[Test]
	public void ChivalrousDeedTestSimplePasses() {
		Assert.IsTrue (ChivalrousDeed.frequency == 1);
		ChivalrousDeed chivalrousDeed = new ChivalrousDeed ();
		Assert.AreEqual ("Chivalrous Deed", chivalrousDeed.getCardName ());

		BoardManagerData.getPlayers ();

		chivalrousDeed.startBehaviour ();

		//need to implement some sort of test case to test out processEvent function
		//Assert.IsTrue (false);
	}
}
