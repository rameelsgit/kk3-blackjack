namespace Blackjack.models;

//ärver från klassen participant 
class Dealer : Participant
{
    public Dealer(): base("Dealer")
    {
        
    }

    // regel för dealer om dealer får mer än 17, 
    public bool NotHit()
    {
        return Hand.CalculateScore()>= 17;

    }

}