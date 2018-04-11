using System;

[Serializable]
public class KingPellinore : Ally {

	public static int frequency = 1;
	private int empoweredBidPoints;

	public KingPellinore() : base ("King Pellinore", 10, 0) {
		empoweredBidPoints = 4;
	}

	public override int getBidPoints() {
        if (BoardManagerMediator.getInstance().getCardInPlay().GetCardName() == "Search for the Questing Beast") {
            return empoweredBidPoints;
        }
		return bidPoints;
	}
}