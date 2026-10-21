using Blackjack.enums;
namespace Blackjack.models;

// Riskabel: jagar höga händer och tar kort tills handen är minst 19.
public class RiskyStrategy : IPlayerStrategy
{
    public string Name => "Riskabel";

    public Moves Decide(Hand hand, Card dealerUpCard)
    {
        return hand.CalculateScore() < 19 ? Moves.Hit : Moves.Stand;
    }
}