public class SirGawain : Ally {

	public static int frequency = 1;
	private int empoweredBattlePoints;

	public SirGawain() : base ("Sir Gawain", 10, 0) {
		empoweredBattlePoints = 20;
	}

	public override int getBattlePoints() {
        if (BoardManagerMediator.getInstance().getCardInPlay().getCardName() == "Test of the Green Knight") {
            return empoweredBattlePoints;
        }
		return battlePoints;
	}
}