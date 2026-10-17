using Blackjack.enums;
namespace Blackjack.models;
class Game
{
    // Privata fält som håller reda på det pågående spelets tillstånd
    private Deck _deck;
    private Player _player;
    private Dealer _dealer;

    // Konstruktor skapar kortleken, blandar den och sätter upp spelaren och dealern
    public Game(string playerName)
    {
        _deck = new Deck();
        _deck.Shuffle();

        _player = new Player(playerName);
        _dealer = new Dealer();
    }

    // Egenskaper som låter Program.cs användargränssnittet komma åt spelaren och dealern
    public Player CurrentPlayer => _player;
    public Dealer CurrentDealer => _dealer;

    // Tömmer både spelarens och dealerns hand innan en ny runda startar
    public void ResetHands()
    {
        _player.Hand.ClearHand();
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
        List<Player> players = new List<Player> { _player };
        Round currentRound = new Round(_deck, players, _dealer);

        // Dela ut de två första korten till varje deltagare
        currentRound.DealCards();

        return currentRound;
    }
}