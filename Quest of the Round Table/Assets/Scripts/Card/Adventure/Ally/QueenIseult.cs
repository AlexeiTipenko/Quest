using System;

[Serializable]
public class QueenIseult : Ally {

	public static int frequency = 1;
	private int empoweredBidPoints;

	public QueenIseult() : base ("Queen Iseult", 0, 2) {
		empoweredBidPoints = 4;
	}

	public new int getBidPoints() {
        foreach (Player player in BoardManagerMediator.getInstance().getPlayers()) {
            if (player.getPlayArea().containsCard("Sir Tristan")) {
                return empoweredBidPoints;
            }
        }
		return bidPoints;
	}
}
