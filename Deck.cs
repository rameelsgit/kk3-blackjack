using System.Resources;
using Blackjack.enums;
namespace Blackjack.models;

public class Deck
{
    // Listan som ska innehålla alla våra kort i kortleken.
    private List<Card> _cards= new();

    // Slumpgeneratorn som hjälper oss att blanda korten.
    private static readonly Random _random=new();

    // Default värde för int deckCount.
    private int _deckCount=1;

    // Antal deck
    public Deck(int deckCount)
    {
        // körs bara om deck count är 1 eller större annars det kommer vara default.
        if (deckCount>0)
        {
            _deckCount=deckCount;
        }
        // Skapar och blandar ordning på alla kort direkt från start.
        Reset();
    }

    // Skapar en ny kortlek och blandar den.
    public void Reset()
    {
        _cards.Clear();

        // Körs till antal deckcount (om 2 deck= 104 kort)
        for(int i=0; i<_deckCount; i++)
        {
        // Skapa alla 52 kort i en deck.
        foreach(Suit suit in Enum.GetValues<Suit>())
        {
            foreach(Ranks rank in Enum.GetValues<Ranks>())
            {
                _cards.Add(new Card(suit,rank));
            }
        }
        }
        Shuffle();
    }

    // Blanda kortleken.
    public void Shuffle()
    {
         //blandar nu med fisher yates, itirerar baklänges
        for (int i=0; i<_cards.Count-1;i++)
        {
            //Väljer ett slumpmässigt index från index till slutet
            int randomIndex=_random.Next(i,_cards.Count);

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

    public int CardsRemaining()
    {
        return _cards. Count;
    }
}

