using Blackjack.enums;
namespace Blackjack.models;
class Round
{
    private Deck RoundDeck;  

    private List<Player> RoundPlayers;

    private Dealer RoundDealer;
public Round(Deck deck, List<Player> players, Dealer dealer)
//skapade en Konstruktör som hämtar information som sparas inför rundans början.
    {
        RoundDeck = deck;
        RoundPlayers = players;
        RoundDealer = dealer;
    }

}