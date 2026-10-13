using System.Resources;
using Blackjack.enums;
namespace Blackjack.models;

public class Deck
{
    // Listan som ska innehålla alla våra kort i kortleken.
    private List<Card> _cards= new();

    // Slumpgeneratorn som hjälper oss att blanda korten.
    private static readonly Random _random=new();

    public Deck()
    {
        // Skapar och blandar ordning på alla kort direkt från start.
        Reset();
    }

    // Skapar en ny kortlek och blandar den.
    public void Reset()
    {
        _cards.Clear();

        // Skapa alla 52 kort.
        foreach(Suit suit in Enum.GetValues<Suit>())
        {
            foreach(Ranks rank in Enum.GetValues<Ranks>())
            {
                _cards.Add(new Card(suit,rank));
            }
        }
        Shuffle();
    }

    // Blanda kortleken.
    public void Shuffle()
    {
        for (int i=0; i<_cards.Count;i++)
        {
            //Väljer ett slumpmässigt index
            int randomIndex=_random.Next(_cards.Count);

            //Byter plats på kortet vi är på och det slumpade kortet
            Card temp= _cards[i];
            _cards[i]=_cards[randomIndex];
            _cards[randomIndex]=temp;

        }
    }


    // Drar det översta kortet.
    public Card DrawCard()
    {
        // Om korten tar slut, återställer leken automatisk.
        if(_cards.Count==0)
        {
            Reset();

        }

        Card card= _cards[0];
        _cards.RemoveAt(0);
        return card;
    }
}