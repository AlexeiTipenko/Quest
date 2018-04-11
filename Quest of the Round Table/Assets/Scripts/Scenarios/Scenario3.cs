public class Scenario3 {

	public static Scenario3 instance;
	private AdventureDeck adventureDeck;
	private StoryDeck storyDeck;

	public static Scenario3 getInstance() {
		if (instance == null) {
			instance = new Scenario3 ();
		}
		return instance;
	}

	public Deck AdventureDeck() {
		adventureDeck = new AdventureDeck();

		//Player 2
		adventureDeck.moveCardToIndex("TestOfTemptation", 12);
		adventureDeck.moveCardToIndex("Dragon", 13);

		return adventureDeck;
	}

	public Deck StoryDeck() {
		storyDeck = new StoryDeck();

		storyDeck.moveCardToIndex("SlayTheDragon", 0);

		return storyDeck;
	}
}
