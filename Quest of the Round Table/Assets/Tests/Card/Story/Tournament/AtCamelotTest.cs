﻿using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AtCamelotTest {

	[Test]
	public void AtCamelotTestSimplePasses() {
		Assert.IsTrue (AtCamelot.frequency == 1);

		Tournament camelot = new AtCamelot ();

		Assert.AreEqual("At Camelot", camelot.getCardName());
		//Assert.IsTrue(camelot.getBonusShields() == 3);
	}

    /*
    [Test]
    public void TestGetBonusShields()
    {
        Tournament camelot = new AtCamelot();
        Assert.IsTrue(camelot.GetBonusShields() == 3);
    }

    [Test]
    public void TeststartBehaviour()
    {
        Tournament camelot = new AtCamelot();
        Assert.IsTrue(camelot.GetBonusShields() == 3);
    }
    */
}