public class SirLancelot : Ally {

	public static int frequency = 1;
	private int empoweredBattlePoints;

	public SirLancelot() : base ("Sir Lancelot", 15, 0) {
		empoweredBattlePoints = 25;
	}

	public new int getBattlePoints() {
        if (BoardManagerMediator.getInstance().getCardInPlay().getCardName() == "Defend the Queen's Honor") {
            return empoweredBattlePoints;
        }
		return battlePoints;
	}
}

