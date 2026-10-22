using System.Data;
using Blackjack.enums;
namespace Blackjack.models;
class Round
{
    private Deck RoundDeck;  

    private List<Player> RoundPlayers;

    private Dealer RoundDealer;

    private Outcomes RoundOutcome;

    //dictionary håller koll på varje player objekts satsning separat , 
    private Dictionary<Player, int> PlayerBets= new Dictionary<Player, int>();

    private RuleSet Rules;



public Round(Deck deck, List<Player> players, Dealer dealer, RuleSet rules)
//skapade en Konstruktör som hämtar information som sparas inför rundans början.
    {
        RoundDeck = deck;
        RoundPlayers = players;
        RoundDealer = dealer;
        Rules=rules;
    }

    //metoden placerar ut satsningarna fören korten delas ut
    public void PlaceBets(Player player, int betAmounts)
    {
        //tar bort marker från spelarens betAmounts
        player.RemoveChips(betAmounts);
        // lägger spelarens betAmounts i dictionary, för att komma ihåg allas amounts
        PlayerBets[player]= betAmounts;
    }

    //metoden hämtar spelarens satsning för rundan 
    public int GetBet(Player player)
    {
        //kollar om spelaren har lagt en satsning i dictionary 
        if(PlayerBets.ContainsKey(player))
        {
            return PlayerBets[player];
        }
        //returnerar 0 om inget hittas
        else
        {
            return 0;
        }
    }

// när rundan startar får varje spelare två kort
    public void DealCards()
    {
        // foreach loopen ser till att varje spelare får sina två kort
        foreach( Player player in RoundPlayers)
        {
            player.ReceiveCard(RoundDeck.DrawCard());
            player.ReceiveCard(RoundDeck.DrawCard());
        }

        //efter att spelarna fått sina kort får dealer 
        RoundDealer.ReceiveCard(RoundDeck.DrawCard());
        RoundDealer.ReceiveCard(RoundDeck.DrawCard());

    }

    // spelaren val om att göra hit eller stand.
    public void PlayerTurn(Moves move, Player player)
    {
       
        if(move==Moves.Hit)
        {
            
            player.ReceiveCard(RoundDeck.DrawCard());

            if (player.Hand.CalculateScore()>21)
            {
               RoundOutcome= Outcomes.PlayerBust;
            }
        }
        else if(move==Moves.Stand)
        {
            DealerTurn();
        }
        
    }

    // Dealers val att göra hit om totalscore är under 17.
    public void DealerTurn()
    {
        while(!RoundDealer.NotHit(Rules))
        {
            RoundDealer.ReceiveCard(RoundDeck.DrawCard());
        }
    }

    public Outcomes EvaluateOutcome(Player player) // ger resultatet genom att jämföra poäng
{
    int playerScore = player.Hand.CalculateScore();
    int dealerScore = RoundDealer.Hand.CalculateScore();
    //använder getbet för att hantera satsningarna i evaluateoutcome
    int bet = GetBet(player);

    // if-sats för naturlig 21
    if (player.Hand.Blackjack() && !RoundDealer.Hand.Blackjack())
     {
        // splearens payout blir satsningaen tillbaka och * 1.5
        player.AddWin();
        int payout = bet + (int)(bet * 1.5);
        player.AddChips(payout);
        return Outcomes.PlayerBlackjack;
    }


    if (playerScore > 21)
    {
        player.AddLoss();
        return Outcomes.PlayerBust;
    }
 
    if (dealerScore > 21)
    {
        // lagt till 1:1 vinst
        int payout = bet * 2;
        player.AddChips(payout);
        player.AddWin();
        return Outcomes.DealerBust;
    }

    // kollar om det är en Natural Blackjack alltså Blackjack med (2 kort).
    if (Rules.NaturalBlackjack&& player.Hand.Blackjack()&& dealerScore==21 && !RoundDealer.Hand.Blackjack())
        {
            player.AddWin();
            return Outcomes.PlayerBlackjack;
        }
    
    if (playerScore > dealerScore)
    {
        // lagt till 1:1 vinst
        int payout = bet * 2;
        player.AddChips(payout);
        player.AddWin();
        return Outcomes.PlayerWins;
    }
    if (dealerScore > playerScore)
    {
        player.AddLoss();
        return Outcomes.DealerWins;
    }

    // splearen får tillbaka sin insats
    player.AddChips(bet);
    player.AddPush();
    return Outcomes.Push;
}

}