using Blackjack.enums; // Använder enums från Rank.cs
namespace Blackjack.models;

public class Card 
{
    public Suit Suit { get; } // Kortens symbol, dras från Suit enum
    public Ranks Rank { get; } // Kortens ID, dras från Ranks enum

    public Card (Suit suit, Ranks rank) // Konstruktor som använder Suit och Ranks som parametrar
    {
        Suit = suit;
        Rank = rank;
    }

    // Ger oss Kortets Blackjack-poäng 
        public int GetValue()
        {
        if (Rank == Ranks.Jack ||Rank == Ranks.Queen || Rank == Ranks.King) return 10;
        if (Rank == Ranks.Ace) return 11;
    
    return (int)Rank;   
        }
    // Skapar textrepresentation av kortet, t.ex. "Ace of Spades"
    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
 } 
