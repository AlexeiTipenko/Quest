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
        adventureDeck.moveCardToIndex("TestOfTheQuestingBeast", 4);
        //Player 2
        adventureDeck.moveCardToIndex("Horse", 12);
        adventureDeck.moveCardToIndex("Dagger", 13);
		adventureDeck.moveCardToIndex("TestOfMorganLeFey", 14);
        //Player 3
        adventureDeck.moveCardToIndex("QueenGuinevere", 24);
        adventureDeck.moveCardToIndex("Mordred", 25);
        adventureDeck.moveCardToIndex("Amour", 26);
        adventureDeck.moveCardToIndex("TestOfTemptation", 27);
        //Player 4
        adventureDeck.moveCardToIndex("KingArthur", 36);
        adventureDeck.moveCardToIndex("SirLancelot", 37);
        adventureDeck.moveCardToIndex("Thieves", 38);

        return adventureDeck;
    }

    public Deck StoryDeck() {
        storyDeck = new StoryDeck();

        storyDeck.moveCardToIndex("SlayTheDragon", 0);
        storyDeck.moveCardToIndex("ChivalrousDeed", 1);
        storyDeck.moveCardToIndex("ProsperityThroughoutTheRealm", 2);
        storyDeck.moveCardToIndex("AtYork", 3);

        return storyDeck;
    }
}
