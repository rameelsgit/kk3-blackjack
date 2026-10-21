using System.Runtime;

namespace Blackjack.models;

public class Player : Participant // Ärver från klassen Participant
{
    public int Win { get; private set; } //antal vinster för spelaren
    public int Loss { get; private set; } // spelarens förluster
    public int Push { get; private set; } // spelarens oavgjorda omgångar.
    // sätter spelarens start marker
    public int Chips { get; private set;} = 100; 
    
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
    // sätter och läser in start marker
    public void setChips(int amount)
    {
        Chips = amount;
    }
    // lägger till vunna marker till spelarens amount 
    public void AddChips(int amount)
    {
        Chips += amount;
    }
    // subtraherar förlorade marker från spelarens amount
    public void RemoveChips(int amount)
    {
        Chips -= amount; 
    }

    // metod som hämtar stats från storage
    public void SetStats(int win, int loss, int push)
    {
        Win = win;
        Loss = loss;
        Push = push;
    }

}