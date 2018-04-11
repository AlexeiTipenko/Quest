using System;

[Serializable]
public class SirTristan : Ally {

	public static int frequency = 1;
	private int empoweredBattlePoints;

	public SirTristan() : base ("Sir Tristan", 10, 0) {
		empoweredBattlePoints = 20;
	}

	public override int getBattlePoints() {
        foreach (Player player in BoardManagerMediator.getInstance().getPlayers()) {
            if (player.getPlayArea().Contains("Queen Iseult")) {
                return empoweredBattlePoints;
            }
        }
		return battlePoints;
	}
}
