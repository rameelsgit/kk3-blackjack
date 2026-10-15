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

// när rundan startar får varje spelare två kort
    public void DealCards()
    {
        // foreach loopen ser till att varje spelare får sina två kort
        foreach( Player player in RoundPlayers)
        {
            player.ReceiveCard(RoundDeck.DrawCard());
            player.ReceiveCard(RoundDeck.DrawCard());
        }

        //efter att spelarna fått sina kort får dealer 
        RoundDealer.ReceiveCard(RoundDeck.DrawCard());
        RoundDealer.ReceiveCard(RoundDeck.DrawCard());

    }


}