using Blackjack.enums;
namespace Blackjack.models;
class Round
{
    private Deck RoundDeck;  

    private List<Player> RoundPlayers;

    private Dealer RoundDealer;

    private Outcomes RoundOutcome;

    //dictionary håller koll på varje player objekts satsning separat 
    private Dictionary<Player, int> PlayerBets= new Dictionary<Player, int>(); 
public Round(Deck deck, List<Player> players, Dealer dealer)
//skapade en Konstruktör som hämtar information som sparas inför rundans början.
    {
        RoundDeck = deck;
        RoundPlayers = players;
        RoundDealer = dealer;
    }

    //metoden placerar ut satsningarna fören korten delas ut
    public void PlaceBets(Player player, int betAmounts)
    {
        //tar bort marker från spelarens betAmounts
        player.RemoveChips(betAmounts);
        // lägger spelarens betAmounts i dictionary, för att komma ihåg allas amounts
        PlayerBets[player]= betAmounts;
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
        while(!RoundDealer.NotHit())
        {
            RoundDealer.ReceiveCard(RoundDeck.DrawCard());
        }
    }

    public Outcomes EvaluateOutcome(Player player) // ger resultatet genom att jämföra poäng
{
    int playerScore = player.Hand.CalculateScore();
    int dealerScore = RoundDealer.Hand.CalculateScore();
 
  
    if (playerScore > 21)
    {
        player.AddLoss();
        return Outcomes.PlayerBust;
    }
 
    
    if (dealerScore > 21)
    {
        player.AddWin();
        return Outcomes.DealerBust;
    }
 
    
    if (playerScore > dealerScore)
    {
        player.AddWin();
        return Outcomes.PlayerWins;
    }
    if (dealerScore > playerScore)
    {
        player.AddLoss();
        return Outcomes.DealerWins;
    }
 
    
    player.AddPush();
    return Outcomes.Push;
}

}