using Blackjack.enums;
namespace Blackjack.models;

class Hand
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
        while (score > 21 && aceCount > 0) //om mer än 21 poäng, skriver ut "Bust" och tar bort 10 poäng
        {
            score -= 10;
            aceCount--;
        }
        return score; // skriver ut spelarens totala poäng
    }
}