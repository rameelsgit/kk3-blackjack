namespace Blackjack.models;

//ärver från klassen participant 
class Dealer : Participant
{
    public Dealer(): base("Dealer")
    {
        
    }

    // regel för dealer om dealer får mer än 17.
    public bool NotHit(RuleSet rules)
    {
        int score=Hand.CalculateScore();

        if (score>17)
        {
            return true;
        }
        if (score<17)
        {
            return false;
        }

        // Om score är exakt 17 då används den här regel.
        bool hasAce = Hand.ToString()!.Contains("Ace");

        if(hasAce)
        {
            return rules.DealerStandsOnSoft17;

        }
        return true;

    }

}