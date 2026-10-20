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
// Skapa ny spelare om den inte hittades.
if (activePlayer == null)
{
    activePlayer = new Player(playerName);
    players.Add(activePlayer);
}

activePlayers.Add(activePlayer);

Console.WriteLine($"Welcome {activePlayer.Name}! Wins: {activePlayer.Win} | Losses: {activePlayer.Loss} | Pushes: {activePlayer.Push} | Chips: {activePlayer.Chips}");
}

// Starta spelmotorn.
Game game = new Game(activePlayers);
bool keepPlaying = true;

while (keepPlaying)
{
    Console.WriteLine("\n");
    Console.WriteLine("Starting a new round...");

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
    Console.WriteLine($"Dealer Score: {game.CurrentDealer.Hand.CalculateScore()}");
    foreach (Player p in game.CurrentPlayers)
    {
    Console.WriteLine($"{p.Name} Score: {p.Hand.CalculateScore()}");
    }

    // Loop för spelarens tur.
foreach (Player activePlaye in game.CurrentPlayers)
{
    Console.WriteLine($"\n--- {activePlaye.Name}'s Turn ---");
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
            Console.WriteLine($"New Score: {activePlaye.Hand.CalculateScore()}");

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
    Console.WriteLine($"Dealer Final Score: {game.CurrentDealer.Hand.CalculateScore()}");
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