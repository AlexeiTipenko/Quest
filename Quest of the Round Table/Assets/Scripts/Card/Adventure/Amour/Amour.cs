public class Amour : Adventure {

	public static int frequency = 8;
	private int battlePoints, bidPoints;

	public Amour() : base ("Amour") {
		battlePoints = 10;
		bidPoints = 1;
	}

	public override int getBattlePoints() {
		return battlePoints;
	}

	public override int getBidPoints() {
		return bidPoints;
	}

}
