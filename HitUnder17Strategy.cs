using Blackjack.enums;
namespace Blackjack.models;

// Grundstrategi: ta kort så länge handen är under 17, annars stanna.
// Dealerns kort ignoreras, strategin tittar bara på den egna handen.
public class HitUnder17Strategy : IPlayerStrategy
{
    public string Name => "Hit under 17";

    public Moves Decide(Hand hand, Card dealerUpCard)
    {
        return hand.CalculateScore() < 17 ? Moves.Hit : Moves.Stand;
    }
}