using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class QueensFavorTest {

	[Test]
	public void QueensFavorTestSimplePasses() {
		Assert.IsTrue (QueensFavor.frequency == 2);
		Event queensFavor = new QueensFavor ();
		Assert.AreEqual ("Queen's Favor", queensFavor.getCardName ());

		//need to implement some sort of test case to test out processEvent function
		//Assert.IsTrue (false);
	}
}
