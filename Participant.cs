namespace Blackjack.models;

class Participant
{
    public string Name {get;}
    public Hand Hand {get;}

    // Konstruktor som använder name som parameter och ger nytt värdet till Hand.
    public Participant (string name) 
    {
       Name= name;
       Hand= new Hand();
    }

    // Method som lägger till kort till spelarens hand.
    public void ReceiveCard(Card card)
    {
        Hand.AddCard(card);
    }
   
    
}