public class Scenario1 {

	public static Scenario1 instance;
	private AdventureDeck adventureDeck;
	private StoryDeck storyDeck;

	public static Scenario1 getInstance() {
		if (instance == null) {
			instance = new Scenario1 ();
		}
		return instance;
	}

    public Deck AdventureDeck() {
        adventureDeck = new AdventureDeck();

        //Player 1
        adventureDeck.moveCardToIndex("Saxons", 0);
        adventureDeck.moveCardToIndex("Boar", 1);
        adventureDeck.moveCardToIndex("Sword", 2);
        adventureDeck.moveCardToIndex("Dagger", 3);
        //Player 2
        adventureDeck.moveCardToIndex("Horse", 12);
        adventureDeck.moveCardToIndex("Dagger", 13);
        //Player 3
        adventureDeck.moveCardToIndex("Horse", 24);
        adventureDeck.moveCardToIndex("Excalibur", 25);
        adventureDeck.moveCardToIndex("Amour", 26);
        //Player 4
        adventureDeck.moveCardToIndex("BattleAx", 36);
        adventureDeck.moveCardToIndex("Lance", 37);
        adventureDeck.moveCardToIndex("Thieves", 38);

        return adventureDeck;
    }

    public Deck StoryDeck() {
        storyDeck = new StoryDeck();

        storyDeck.moveCardToIndex("BoarHunt", 0);
        storyDeck.moveCardToIndex("ProsperityThroughoutTheRealm", 1);
        storyDeck.moveCardToIndex("ChivalrousDeed", 2);

        return storyDeck;
    }
}
