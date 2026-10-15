namespace Blackjack.models;


class Game
{
    
    // Game äger alla delar som behövs under spelets gång. 
    public Player Player {get; }
    public Dealer Dealer {get; }
    public Deck Deck {get; }

    public Game(string playerName)
    {
        Player = new Player(playerName);
        Dealer = new Dealer();
        Deck = new Deck();
    }


}


