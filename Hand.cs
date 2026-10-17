using Blackjack.enums;
namespace Blackjack.models;

public class Hand
{
    List<Card> Cards { get; } = new List<Card>(); // Ger lista av kort till spelarens hand


    public void AddCard(Card card) //  Lägger till kort till spelarens hand
    {
        Cards.Add(card);

    }
    public void ClearHand() // rensar spelarens hand på slutet av rundan
    {
        Cards.Clear();
    }

    public int CalculateScore() // räknar ut spelarens totala poäng
    {
        int score = 0; 
        int aceCount = 0;
        foreach (Card card in Cards) 
        {
            score += card.GetValue();
        }
        while (score > 21 && aceCount > 0) // om din score blir mer än 21 ändrar ACE till 1. 
        {
            score -= 10;
            aceCount--;
        }
        return score; // skriver ut spelarens totala poäng
    }

    // IsBust metoden blir true om handen är över 21, då förlorar man 
    public bool IsBust()
    {
        return CalculateScore()> 21;
    }

    // blackjack metoden används om spelaren har en naturlig 21 på två kort
    public bool Blackjack()
    {
        return Cards.Count == 2 && CalculateScore() == 21; 

    }
}