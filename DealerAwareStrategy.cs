using Blackjack.enums;
namespace Blackjack.models;

// Tänk på dealerns kort:
// - Visar dealern ett svagt kort (2-6) är risken stor att dealern går bust, så vi stannar vid 12 eller mer.
// - Visar dealern ett starkt kort (7 eller högre, ess räknas som 11) måste vi försöka slå det och tar kort tills 17.
public class DealerAwareStrategy : IPlayerStrategy
{
    public string Name => "Tänk på dealerns kort";

    public Moves Decide(Hand hand, Card dealerUpCard)
    {
        int score = hand.CalculateScore();
        int dealerValue = dealerUpCard.GetValue();

        bool dealerIsWeak = dealerValue >= 2 && dealerValue <= 6;

        int standAt = dealerIsWeak ? 12 : 17;
        return score < standAt ? Moves.Hit : Moves.Stand;
    }
}