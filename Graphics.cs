using Blackjack.enums;
using Blackjack.models;

namespace Blackjack.Graphics;

public class Graphics //Tar ett kort i taget och ritar ut den
{
    public string[] GetCardLines(Card card, bool isHidden = false) //Bygger upp kortet
    {
        if (isHidden)
        {
            return new string[]
            {
                "┌───────┐",
                "│▒▒▒▒▒▒▒│",
                "│▒▒▒▒▒▒▒│",
                "│▒▒▒▒▒▒▒│",
                "│▒▒▒▒▒▒▒│",
                "└───────┘"
            };
        }

        //hämtar rätt symboler för kortets färg och valör
        string suitSymbol = GetSuitSymbol(card.Suit);
        string rankSymbol = GetRankSymbol(card.Rank);
        
        //Anpassar avståndet så att siffran 10 inte förstör kortets ram
        string topRank = rankSymbol.PadRight(2);
        string bottomRank = rankSymbol.PadLeft(2);

        // returnerar kortets sex rader i en array
        return new string[]
        {
            "┌───────┐",
            $"│{topRank}     │",
            $"│   {suitSymbol}   │",
            $"│       │",
            $"│     {bottomRank}│",
            "└───────┘"
        };
    }

    private string GetSuitSymbol(Suit suit) //översätter suit-enum till motsvarande symbol
    {
        return suit switch
        {
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Clubs => "♣",
            Suit.Spades => "♠",
            _ => "?"
        };
    }
    private string GetRankSymbol(Ranks rank) //översätter ranks-enum till valörtext
    {

        return rank switch
        {
            Ranks.Ace => "A",
            Ranks.King => "♚",
            Ranks.Queen => "♛",
            Ranks.Jack => "♞",
            _ => ((int)rank).ToString()
        };
    }
}