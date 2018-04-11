using System;

[Serializable]
public class SirPercival : Ally {

	public static int frequency = 1;
	private int empoweredBattlePoints;

	public SirPercival() : base ("Sir Percival", 5, 0) {
		empoweredBattlePoints = 20;
	}

	public new int getBattlePoints() {
        if (BoardManagerMediator.getInstance().getCardInPlay().GetCardName() == "Search for the Holy Grail") {
            return empoweredBattlePoints;
        }
		return battlePoints;
	}
}