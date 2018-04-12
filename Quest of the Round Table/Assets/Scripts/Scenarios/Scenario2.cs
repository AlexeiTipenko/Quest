public class Scenario2 {

	public static Scenario2 instance;
	private AdventureDeck adventureDeck;
	private StoryDeck storyDeck;

	public static Scenario2 getInstance() {
		if (instance == null) {
			instance = new Scenario2 ();
		}
		return instance;
	}

    public Deck AdventureDeck()
    {
        adventureDeck = new AdventureDeck();

        //Player 1
        adventureDeck.moveCardToIndex("BattleAx", 0);
        adventureDeck.moveCardToIndex("Mordred", 1);
        //Player 2
        adventureDeck.moveCardToIndex("Saxons", 12);
        adventureDeck.moveCardToIndex("Dragon", 13);
        adventureDeck.moveCardToIndex("SirGalahad", 14);

        return adventureDeck;
    }

    public Deck StoryDeck()
    {
        storyDeck = new StoryDeck();

        storyDeck.moveCardToIndex("BoarHunt", 0);
        storyDeck.moveCardToIndex("RepelTheSaxonRaiders", 1);

        return storyDeck;
    }

}
