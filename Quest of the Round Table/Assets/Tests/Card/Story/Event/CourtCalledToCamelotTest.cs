using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CourtCalledToCamelotTest {

	[Test]
	public void CourtCalledToCamelotTestSimplePasses() {
		Assert.IsTrue (CourtCalledToCamelot.frequency == 2);
		Event courtCalledToCamelot = new CourtCalledToCamelot ();
		Assert.AreEqual ("Court Called to Camelot", courtCalledToCamelot.getCardName ());

		//need to implement some sort of test case to test out processEvent function
		//Assert.IsTrue (false);
	}
}
