using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class KingsRecognitionTest {

	//Event description: The next player(s) to complete a Quest will receive 2 extra shields.
	[Test]
	public void KingsRecognitionTestSimplePasses() {
		Assert.IsTrue (KingsRecognition.frequency == 2);
		Event kingsRecognition = new KingsRecognition ();
		Assert.AreEqual ("King's Recognition", kingsRecognition.getCardName ());

		kingsRecognition.startBehaviour ();

		//see if active when card drawn, potentially implement other test cases depending on future game logic
	
		Assert.IsTrue (Quest.KingsRecognitionActive == false);
	}
}
