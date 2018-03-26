using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Strategy1 : AbstractAI
{
    public Strategy1() : base (50, 20) {
        
    }

    public override void DiscardAfterWinningTest()
    {
        throw new System.NotImplementedException();
    }

    public override bool DoIParticipateInQuest()
    {
        throw new System.NotImplementedException();
    }

    public override bool DoIParticipateInTournament()
    {
		if (SomeoneElseCanWinOrEvolveWithTournament(board.getPlayers())) 
		{
			return true;
		}
        return false;
    }

    public override bool DoISponsorAQuest()
    {
        if (SomeoneElseCanWinOrEvolveWithQuest())
        {
            return false;
        }
        else if (SufficientCardsToSponsorQuest())
        {
            return true;
        }
        return false;
    }

    public override void NextBid()
    {
        throw new System.NotImplementedException();
    }

    public override void PlayQuestStage(Stage stage)
    {
        throw new System.NotImplementedException();
    }

    public override void SponsorQuest()
    {
        throw new System.NotImplementedException();
    }

    protected override bool CanPlayCardForStage(Card card, List<Card> participationList)
    {
        throw new System.NotImplementedException();
    }

    protected override void PlayFoeStage(Stage stage)
    {
        throw new System.NotImplementedException();
    }

    protected override void PlayTestStage(Stage stage)
    {
        throw new System.NotImplementedException();
    }

    public override List<Card> ParticipateTournament() {
		Tournament tournament = (Tournament)board.getCardInPlay();
		List<Card> hand = strategyOwner.getHand();
		List<Card> sortedList = SortBattlePointsCards (hand);
		List<Card> participationList = new List<Card> ();
		if (SomeoneElseCanWinOrEvolveWithTournament (tournament.participatingPlayers)) {
			//play strongest possible hand (includes allies, amours and weapons)
			foreach (Card card in sortedList) {
				if (((card.GetType () == typeof(Amour) || card.GetType().IsSubclassOf(typeof(Weapon))) && !participationList.Contains (card)) 
					|| card.GetType().IsSubclassOf(typeof(Ally)))
				{
					participationList.Add (card);
				} 
			}
		} else {
			//Else: I play only weapons I have two or more instances of
			List<Card> weaponList = new List<Card>();
			foreach (Card card in sortedList) {
				if (card.GetType ().IsSubclassOf (typeof(Weapon))) {
					weaponList.Add (card);
				}
			}
			Dictionary<Card, int> weaponCountMap = weaponList.GroupBy( i => i ).ToDictionary(t => t.Key, t=> t.Count());

			foreach (Card card in weaponCountMap.Keys) {
				if (weaponCountMap [card] > 1) {
					participationList.Add (card);
				}
			}
		}
		return participationList;
    }
}
