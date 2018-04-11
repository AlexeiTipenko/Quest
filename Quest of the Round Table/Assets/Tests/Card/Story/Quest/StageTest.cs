using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class StageTest {

    [Test]
    public void testGetTotalCards() {
        Story questCard = new SlayTheDragon();
        Adventure stageCard = new Thieves();
        BoardManagerMediator.getInstance().setCardInPlay(questCard);
        Stage stage = new Stage(stageCard, null, 0);
        Assert.AreEqual(stage.getTotalCards(), 1);
    }

	[Test]
	public void testGetTotalBattlePointsNoWeapons() {
        Story questCard = new SlayTheDragon();
        Adventure stageCard = new Thieves();
        BoardManagerMediator.getInstance().setCardInPlay(questCard);
        Stage stage = new Stage(stageCard, null, 0);
        Assert.AreEqual(stage.getTotalBattlePoints(), 5);
	}

    [Test]
    public void testGetTotalBattlePointsWeapons() {
        Story questCard = new SlayTheDragon();
        Adventure stageCard = new Thieves();
        List<Adventure> weapons = new List<Adventure>();
        weapons.Add(new Excalibur());
        weapons.Add(new Dagger());
        BoardManagerMediator.getInstance().setCardInPlay(questCard);
        Stage stage = new Stage(stageCard, weapons, 0);
        Assert.AreEqual(stage.getTotalBattlePoints(), 40);
    }

    [Test]
    public void testGetTotalBattlePointsEmpowered() {
        Story questCard = new SlayTheDragon();
        Adventure stageCard = new Dragon();
        BoardManagerMediator.getInstance().setCardInPlay(questCard);
        Stage stage = new Stage(stageCard, null, 0);
        Assert.AreEqual(stage.getTotalBattlePoints(), 70);
    }
}
