using Blackjack.enums;
namespace Blackjack.models;

// Grundstrategi för Hit/Stand.
// Double down och split finns inte i spelet, så de dragen ersätts av närmaste alternativ hit eller stand.
//
// Hård hand (inget ess som räknas som 11):
//   8 eller lägre och 9-11 : Hit
//   12                     : Stand mot 4-6, annars Hit
//   13-16                  : Stand mot 2-6, annars Hit
//   17 eller mer           : Stand
// Mjuk hand (ess som räknas som 11):
//   17 eller lägre         : Hit
//   18                     : Stand mot 2-8, Hit mot 9, 10 och ess
//   19 eller mer           : Stand
public class BasicStrategy : IPlayerStrategy
{
    public string Name => "Grundstrategi";

    public Moves Decide(Hand hand, Card dealerUpCard)
    {
        int score = hand.CalculateScore();
        int dealer = dealerUpCard.GetValue(); // 2-11, ess = 11

        if (IsSoft(hand))
        {
            if (score <= 17) return Moves.Hit;
            if (score == 18) return dealer <= 8 ? Moves.Stand : Moves.Hit;
            return Moves.Stand;
        }

        if (score <= 11) return Moves.Hit;
        if (score == 12) return (dealer >= 4 && dealer <= 6) ? Moves.Stand : Moves.Hit;
        if (score <= 16) return (dealer >= 2 && dealer <= 6) ? Moves.Stand : Moves.Hit;
        return Moves.Stand;
    }

    // En hand är mjuk om den har ett ess som fortfarande räknas som 11 utan att gå över 21.
    private static bool IsSoft(Hand hand)
    {
        int lowTotal = 0;      // alla ess räknas som 1
        bool hasAce = false;

        foreach (Card card in hand.Cards)
        {
            if (card.Rank == Ranks.Ace)
            {
                hasAce = true;
                lowTotal += 1;
            }
            else
            {
                lowTotal += card.GetValue();
            }
        }

        return hasAce && lowTotal + 10 <= 21;
    }
}