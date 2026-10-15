namespace Blackjack.models;

class Player : Participant // Ärver från klassen Participant
{
    public int Win { get; private set; } //antal vinster för spelaren
    public int Loss { get; private set; } // spelarens förluster
    public int Push { get; private set; } // spelarens oavgjorda omgångar.
    
    // Konstruktor med spelarens namn. (anropar bas klassen (Participant) med spelarens namn.)
    public Player(string name) : base(name) 
    {

    }

    public void AddWin() // Ökar vinst med 1
    {
        Win++;
    }

    public void AddLoss() // Ökar förlust med 1
    {
         Loss++;
    }

    public void AddPush() // Ökar push med 1
    {
         Push++;
    }
    
   

}