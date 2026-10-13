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
}