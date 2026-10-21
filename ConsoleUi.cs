using Blackjack.enums;
using Blackjack.models;

//Ladda alla spelare från filen.
GameStorage storage = new GameStorage();
List<Player> players = storage.LoadPlayers();

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

Console.WriteLine($"Welcome {activePlayer.Name}! Wins: {activePlayer.Win} | Losses: {activePlayer.Loss} | Pushes: {activePlayer.Push}");
}

// Alla sex strategier som kan väljas.
IPlayerStrategy[] strategies =
{
    //new HitUnder17Strategy(),
    //new CautiousStrategy(),
    //new RiskyStrategy(),
    //new RandomStrategy(),
    //new DealerAwareStrategy(),
    //new BasicStrategy()
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
Game game = new Game(activePlayers);
bool keepPlaying = true;

while (keepPlaying)
{
    Console.WriteLine("\n");
    Console.WriteLine("Starting a new round...");

    Round currentRound = game.StartNewRound();

    // Visa startpoäng.
    Console.WriteLine($"\nDealer Cards: {string.Join(", ", game.CurrentDealer.Hand.Cards)} (Score: {game.CurrentDealer.Hand.CalculateScore()})");
    foreach (Player p in game.CurrentPlayers)
    {
    Console.WriteLine($"{p.Name} Cards: {string.Join(", ", p.Hand.Cards)} (Score: {p.Hand.CalculateScore()})");
    }

    // Loop för spelarens tur.
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
    Console.WriteLine($"{activePlaye.Name}: {result} | Updated Stats -> Wins: {activePlaye.Win} | Losses: {activePlaye.Loss} | Pushes: {activePlaye.Push}");
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