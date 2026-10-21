using Blackjack.enums;
using Blackjack.models;
using Blackjack.Graphics;

//Ladda alla spelare från filen.
GameStorage storage = new GameStorage();
List<Player> players = storage.LoadPlayers();
RuleSet rules= RuleSet.LoadFromFile("rules.txt");

Console.WriteLine("= WELCOME TO BLACKJACK =");

// Fråga hur många som ska spela (1 till 5 spelare).
int playerCount = 0;
while (playerCount < 1 || playerCount > 5)
{
    Console.Write("How many players? (1-5): ");
    int.TryParse(Console.ReadLine(), out playerCount);
}

List<Player> activePlayers = new List<Player>();

// Registrera namn för alla valda spelare.
for (int i = 1; i <= playerCount; i++)
{
    Console.Write($"Enter name for Player {i}: ");
    string playerName = Console.ReadLine()!;

    if (string.IsNullOrEmpty(playerName))
    {
        playerName = $"Player{i}";
    }

//Kontrollera om spelaren redan finns i listan.
Player activePlayer = null;

foreach (Player p in players)
{
    if (p.Name.ToLower() == playerName.ToLower())
    {
        activePlayer = p;
        // Spelaren hittades loopen avbryts.
        break; 
    }
}

// Skapa ny spelare om den inte hittades.
if (activePlayer == null)
{
    activePlayer = new Player(playerName);
    players.Add(activePlayer);
}

activePlayers.Add(activePlayer);

Console.WriteLine($"Welcome {activePlayer.Name}! Wins: {activePlayer.Win} | Losses: {activePlayer.Loss} | Pushes: {activePlayer.Push} | Chips: {activePlayer.Chips}");
}

// Alla sex strategier som kan väljas.
IPlayerStrategy[] strategies =
{
    new HitUnder17Strategy(),
    new CautiousStrategy(),
    new RiskyStrategy(),
    //new RandomStrategy(),
    new DealerAwareStrategy(),
    new BasicStrategy()
};

// Totalt max 5 spelare vid bordet, människor och datorer tillsammans.

int maxComputers = 5 - activePlayers.Count;
int computerCount = -1;
while (maxComputers > 0 && (computerCount < 0 || computerCount > maxComputers))
{
    Console.Write($"How many computer players? (0-{maxComputers}): ");
    int.TryParse(Console.ReadLine(), out computerCount);
}
if (computerCount < 0) computerCount = 0;

for (int i = 1; i <= computerCount; i++)
{
    Console.WriteLine($"\nChoose strategy for computer {i}:");
    for (int s = 0; s < strategies.Length; s++)
    {
        Console.WriteLine($"  ({s + 1}) {strategies[s].Name}");
    }

    int choice = 0;
    while (choice < 1 || choice > strategies.Length)
    {
        Console.Write("Strategy: ");
        int.TryParse(Console.ReadLine(), out choice);
    }
    IPlayerStrategy strategy = strategies[choice - 1];

    // Unikt namn, så att två datorer med samma strategi inte krockar.

    string botName = $"Bot {strategy.Name}";
    if (activePlayers.Any(p => p.Name.ToLower() == botName.ToLower()))
    {
        botName = $"{botName} {i}";
    }

    ComputerPlayer bot = new ComputerPlayer(botName, strategy);

    // Hämta sparad statistik om datorn har spelat förut, och ersätt den i listan.

    Player? saved = players.FirstOrDefault(p => p.Name.ToLower() == botName.ToLower());
    if (saved != null)
    {
        bot.SetStats(saved.Win, saved.Loss, saved.Push);
        players.Remove(saved);
    }
    players.Add(bot);       // så att den sparas i players.txt
    activePlayers.Add(bot); // så att den sitter vid bordet
}


// Starta spelmotorn.
// Här för objektet game tillgång till rules också. 
Game game = new Game(activePlayers, rules);
bool keepPlaying = true;

while (keepPlaying)
{
    Console.WriteLine("\n");
    Console.WriteLine("Starting a new round...");
    Console.WriteLine("\n=== ACTIVE GAME RULES ===");
    Console.WriteLine($"• Number of Decks: {rules.DeckCount}");
    Console.WriteLine($"• Reshuffle when: {rules.Reshuffle} cards remaining");
    Console.WriteLine($"• Dealer Stands on Soft 17: {rules.DealerStandsOnSoft17}");
    Console.WriteLine($"• Natural Blackjack beats 3-card 21: {rules.NaturalBlackjack}");
    Console.WriteLine("n");

    Round currentRound = game.StartNewRound();

    // placera insats fören kort delas ut 
    Console.WriteLine("=PLACE YOUR BETS=");
    // loopen för varje spelare att sätta in giltig amount av chips
    foreach( Player p in game.CurrentPlayers)
    {
        int bet = 0;
    // om bet inte är större än 0 eller under chips amount, körs loopen om 
        while (bet < 1 || bet > p.Chips)
        {
            Console.Write($"{p.Name} (Chips: {p.Chips}) - Enter your bet: ");
            // om input inte är ett nummer + samma krav som while-loop, körs loopen om 
            if(!int.TryParse(Console.ReadLine(), out bet) || bet <1 || bet> p.Chips)
            {
                Console.WriteLine("Enter available amount");
            }
            
        }
        //om bets är giltiga plceras dem och går vidare
        currentRound.PlaceBets(p, bet);
    }

    // Visa startpoäng.
    Console.WriteLine($"\nDealer Cards: {string.Join(", ", game.CurrentDealer.Hand.Cards)} (Score: {game.CurrentDealer.Hand.CalculateScore()})");
    foreach (Player p in game.CurrentPlayers)
    {
    Console.WriteLine($"{p.Name} Cards: {string.Join(", ", p.Hand.Cards)} (Score: {p.Hand.CalculateScore()})");
    }

    //Rita ut kort vid start och dölja dealerns första kort.
PrintHand("Dealer", game.CurrentDealer.Hand, true);
foreach (Player p in game.CurrentPlayers)
{
    PrintHand(p.Name, p.Hand, false);
}

    // Loop för spelarnas turer 
    foreach (Player activePlaye in game.CurrentPlayers)
    {
    Console.WriteLine($"\n--- {activePlaye.Name}'s Turn ---");

    if (activePlaye is ComputerPlayer bot)
{
    Card dealerUpCard = game.CurrentDealer.Hand.Cards[0];

    while (!bot.Hand.IsBust())
    {
        Moves move = bot.ChooseMove(dealerUpCard); // samma anrop, olika strategi
        Console.WriteLine($"{bot.Name} chooses {move}");
        currentRound.PlayerTurn(move, bot);

        if (move == Moves.Stand) break;

        Console.WriteLine($"Cards: {string.Join(", ", bot.Hand.Cards)} (Score: {bot.Hand.CalculateScore()})");
    }

    if (bot.Hand.IsBust())
    {
        Console.WriteLine($"{bot.Name} is Bust! Score went over 21.");
    }
    continue; // hoppa över den mänskliga inmatningen
}

    bool isTurnActive = true;

    while (isTurnActive)
    {
        if (activePlaye.Hand.IsBust())
        {
            Console.WriteLine($"{activePlaye.Name} is Bust! Score went over 21.");
            isTurnActive = false;
            break;
        }

        Console.Write($"{activePlaye.Name}, choose: (1) Hit  (2) Stand: ");
        string input = Console.ReadLine()!;

        if (input == "1" || input == "hit")
        {
            currentRound.PlayerTurn(Moves.Hit, activePlaye);
            Console.WriteLine($"Cards: {string.Join(", ", activePlaye.Hand.Cards)} (Score: {activePlaye.Hand.CalculateScore()})");

            //Ritar ut spelarens uppdaterade hand efter att ha tagit ett nytt kort
    PrintHand(activePlaye.Name, activePlaye.Hand, false);

            if (activePlaye.Hand.IsBust())
            {
                Console.WriteLine("Bust! Score went over 21.");
                isTurnActive = false;
            }
        }
        else if (input == "2" || input == "stand")
        {
            currentRound.PlayerTurn(Moves.Stand, activePlaye);
            isTurnActive = false; 
        }
        else
        {
            Console.WriteLine("Invalid input! Please enter 1 or 2.");
        }
    }
    }

// Dealerns tur körs endast om spelaren inte blev Bust.
  bool anyPlayerSurvives = false;

    foreach (Player p in game.CurrentPlayers)
    {
        if (!p.Hand.IsBust())
        {
        anyPlayerSurvives = true;
        break; // Found one surviving player, stop checking
        }
    }

    // Dealersturn körs bara om någon är i kvar i spelet.
    if (anyPlayerSurvives)
    {
    Console.WriteLine("\nDealer turn...");
    currentRound.DealerTurn();
    Console.WriteLine($"Dealer Final Score: {string.Join(", ", game.CurrentDealer.Hand.Cards)} (Score: {game.CurrentDealer.Hand.CalculateScore()})");
   
    PrintHand("Dealer Final", game.CurrentDealer.Hand, false); // Visar dealerns alla kort!

    }
    else
    {
    Console.WriteLine("\nAll players busted! Dealer wins automatically.");
    }

    // Skriver ut resultatet av rundan.
    Console.WriteLine("\n--- ROUND RESULTS ---");
    foreach (Player activePlaye in game.CurrentPlayers)
    {
    Outcomes result = currentRound.EvaluateOutcome(activePlaye);
    Console.WriteLine($"{activePlaye.Name}: {result} | Updated Stats -> Wins: {activePlaye.Win} | Losses: {activePlaye.Loss} | Pushes: {activePlaye.Push} | Chips: {activePlaye.Chips}");
    }
    
    //tar bort en spelare i taget när de inte har marker kvar
    for (int i = game.CurrentPlayers.Count - 1; i >=0; i--)
    {
        //kolar om spelaren har slut på marker och tar bort spelaren
        if (game.CurrentPlayers[i].Chips <= 0)
        {
            Console.WriteLine($"\n Player {game.CurrentPlayers[i].Name}, ran out of chips & is too poor to play");
            game.CurrentPlayers.RemoveAt(i);
        }
    }
    //avsular spelet om alla spelare vid bordet har slut på marker
    if(game.CurrentPlayers.Count == 0)
    {
        Console.WriteLine("\n All players are too poor to continue...");
        break; 
    }

    // Fråga om spelaren vill spela igen.
    Console.Write("\nDo you want to play again? (yes/no): ");
    string choice = Console.ReadLine()!;

    if (choice != "yes" && choice != "y")
    {
        keepPlaying = false;
    }
}

// Spara alla spelare till filen vid avslut.
storage.SavePlayers(players);
Console.WriteLine("Resultat sparat.");

// metod som hjälper att rita ut en spelares hand grafiskt med rätt färg
void PrintHand(string ownerName, Hand hand, bool hideFirstCard = false)
{
    Graphics renderer = new Graphics();

    Console.WriteLine($"\n=== {ownerName}s Hand ===");

    //Loopar genom kortens 6 rader
    for (int lineIndex = 0; lineIndex < 6; lineIndex++)
    {
        //Går genom alla kort från vänster till höger
        for (int cardIndex = 0; cardIndex < hand.Cards.Count; cardIndex++)
        {
            Card currentCard = hand.Cards[cardIndex];
            bool isHidden = hideFirstCard && cardIndex == 0;
            string[] cardLines = renderer.GetCardLines(currentCard, isHidden);

            // Röd färg för Hjärter och Ruter
            if (!isHidden && (currentCard.Suit == Suit.Hearts || currentCard.Suit == Suit.Diamonds))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.Write(cardLines[lineIndex] + " ");
            Console.ResetColor();
        }
        Console.WriteLine();
    }
    // Visar poängen under korten
    if (hideFirstCard)
    {
        Console.WriteLine("Poäng: ? (Dolt kort)");
    }
    else
    {
        Console.WriteLine($"Poäng: {hand.CalculateScore()}");
    }
}