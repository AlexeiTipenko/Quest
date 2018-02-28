public class Strategy1 : Strategy
{
    public Strategy1() : base (50) {
        
    }

    public override void DiscardAfterWinningTest()
    {
        throw new System.NotImplementedException();
    }

    public override void DoIParticipateInQuest()
    {
        throw new System.NotImplementedException();
    }

    public override void DoIParticipateInTournament()
    {
        throw new System.NotImplementedException();
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

    public override void SponsorQuest()
    {
        throw new System.NotImplementedException();
    }
}
