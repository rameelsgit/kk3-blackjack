using Blackjack.enums;
namespace Blackjack.models;
class Game
{
    // Privata fält som håller reda på det pågående spelets tillstånd
    private Deck _deck;
    private List<Player> _players;
    private Dealer _dealer;

    // Konstruktor skapar kortleken, blandar den och sätter upp spelaren och dealern
    public Game(List<Player> players)
    {
        _deck = new Deck();
        _deck.Shuffle();
        _players = players;
        _dealer = new Dealer();
    }

    public List<Player> CurrentPlayers => _players;
    public Dealer CurrentDealer => _dealer;

    // Tömmer både spelarens och dealerns hand innan en ny runda startar
    public void ResetHands()
    {
        foreach (Player player in _players)
        {
            player.Hand.ClearHand();
        }
        _dealer.Hand.ClearHand();
    }

    // Skapar och förbereder en ny runda
    public Round StartNewRound()
    {
        // Rensar korten från föregående runda
        ResetHands();

        // Byter ut kortleken om det börjar bli få kort kvar 
        if (_deck.CardsRemaining() < 15)
        {
            _deck = new Deck();
            _deck.Shuffle();
        }

        // Skapar en ny runda med de aktiva objekten
        Round currentRound = new Round(_deck, _players, _dealer);

        // Dela ut de två första korten till varje deltagare
        currentRound.DealCards();

        return currentRound;
    }
}