using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ChivalrousDeedTest {

	[Test]
	public void ChivalrousDeedTestSimplePasses() {
		Assert.IsTrue (ChivalrousDeed.frequency == 1);
		Event chivalrousDeed = new ChivalrousDeed ();
		Assert.AreEqual ("Chivalrous Deed", chivalrousDeed.getCardName ());

		//need to implement some sort of test case to test out processEvent function
		Assert.IsTrue (false);
	}
}
