using Blackjack.enums;
namespace Blackjack.models;

// Gemensamt gränssnitt för alla strategier. Samma anrop, olika beteenden.
public interface IPlayerStrategy
{
    // Strategins namn, kan visas i UI:t.
    string Name { get; }

    // Tar emot spelarens hand och dealerns öppna kort och returnerar Hit eller Stand.
    Moves Decide(Hand hand, Card dealerUpCard);
}