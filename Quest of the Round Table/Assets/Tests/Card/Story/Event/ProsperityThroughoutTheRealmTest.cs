using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ProsperityThroughoutTheRealmTest {

	[Test]
	public void ProsperityThroughoutTheRealmTestSimplePasses() {
		Assert.IsTrue (ProsperityThroughoutTheRealm.frequency == 1);
		Event prosperityThroughoutTheRealm = new ProsperityThroughoutTheRealm ();
		Assert.AreEqual ("Prosperity Throughout the Realm", prosperityThroughoutTheRealm.getCardName ());

		//need to implement some sort of test case to test out processEvent function
		Assert.IsTrue (false);
	}
}
