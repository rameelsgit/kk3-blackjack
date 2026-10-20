using Blackjack.enums;
namespace Blackjack.models;

// Försiktig: stannar redan vid 13 eller mer för att minska risken att gå bust.
public class CautiousStrategy : IPlayerStrategy
{
    public string Name => "Försiktig";

    public Moves Decide(Hand hand, Card dealerUpCard)
    {
        return hand.CalculateScore() < 13 ? Moves.Hit : Moves.Stand;
    }
}