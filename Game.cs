using Blackjack.enums;
namespace Blackjack.models;
class Game
{
    // Privata fält som håller reda på det pågående spelets tillstånd
    private Deck _deck;
    private List<Player> _players;
    private Dealer _dealer;
    
    // Rules property så att Game.cs får tillgång till rules.
    public RuleSet Rules{get;}
    // Konstruktor skapar kortleken, blandar den och sätter upp spelaren och dealern
    public Game(List<Player> players, RuleSet rules)
    {
        //Sparat rules i games objekt.
        Rules=rules;
        _deck = new Deck(rules.DeckCount);
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
        // Ändrat 15 till värdet som mattas in i textfilen. 
        if (_deck.CardsRemaining() < Rules.Reshuffle)
        {
            // Skapar ny x antal deck.
            _deck = new Deck(Rules.DeckCount);
            _deck.Shuffle();
        }

        // Skapar en ny runda med de aktiva objekten
        Round currentRound = new Round(_deck, _players, _dealer, Rules);

        // Dela ut de två första korten till varje deltagare
        currentRound.DealCards();

        return currentRound;
    }
}