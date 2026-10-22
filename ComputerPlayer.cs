using Blackjack.enums;
namespace Blackjack.models;

public class ComputerPlayer : Player
{
    public IPlayerStrategy Strategy { get; set; }

    public ComputerPlayer(string name, IPlayerStrategy strategy) : base(name)
    {
        Strategy = strategy;
    }

    public Moves ChooseMove(Card dealerUpCard)
    {
        return Strategy.Decide(Hand, dealerUpCard);
    }
}