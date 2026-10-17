using Blackjack.enums;
using Blackjack.models;

//Ladda alla spelare från filen.
GameStorage storage = new GameStorage();
List<Player> players = storage.LoadPlayers();

Console.WriteLine("= WELCOME TO BLACKJACK =");
Console.Write("Enter your name: ");
string playerName = Console.ReadLine()!;

if (string.IsNullOrEmpty(playerName))
{
    playerName = "Player";
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

Console.WriteLine($"Welcome {activePlayer.Name}!");
Console.WriteLine($"Wins: {activePlayer.Win} | Losses: {activePlayer.Loss} | Pushes: {activePlayer.Push}");

// Starta spelmotorn.
Game game = new Game(activePlayer.Name);
bool keepPlaying = true;

while (keepPlaying)
{
    Console.WriteLine("\n");
    Console.WriteLine("Starting a new round...");

    Round currentRound = game.StartNewRound();

    // Visa startpoäng.
    Console.WriteLine($"Dealer Score: {game.CurrentDealer.Hand.CalculateScore()}");
    Console.WriteLine($"{activePlayer.Name} Score: {activePlayer.Hand.CalculateScore()}");

    // Loop för spelarens tur.
    bool isTurnActive = true;

    while (isTurnActive)
    {
        // Om spelaren redan blir Bust.
        if (activePlayer.Hand.IsBust())
        {
            Console.WriteLine("Bust! Score went over 21.");
            isTurnActive = false;
            break;
        }

        Console.Write("\nChoose: (1) Hit  (2) Stand: ");
        string input = Console.ReadLine()!;

        if (input == "1" || input == "hit")
        {
            currentRound.PlayerTurn(Moves.Hit, activePlayer);
            Console.WriteLine($"New Score: {activePlayer.Hand.CalculateScore()}");

            if (activePlayer.Hand.IsBust())
            {
                Console.WriteLine("Bust! Score went over 21.");
                isTurnActive = false;
            }
        }
        else if (input == "2" || input == "stand")
        {
            currentRound.PlayerTurn(Moves.Stand, activePlayer);
            // Spelaren stannar, turen avslutas.
            isTurnActive = false; 
        }
        else
        {
            Console.WriteLine("Invalid input! Please enter 1 or 2.");
        }
    }

    // Dealerns tur körs endast om spelaren inte blev Bust.
    if (!activePlayer.Hand.IsBust())
    {
        Console.WriteLine("\nDealer turn...");
        currentRound.DealerTurn();
        Console.WriteLine($"Dealer Final Score: {game.CurrentDealer.Hand.CalculateScore()}");
    }

    // Skriver ut resultatet av rundan.
    Outcomes result = currentRound.EvaluateOutcome(activePlayer);
    Console.WriteLine($"\nRound Result: {result}");
    Console.WriteLine($"Updated Stats -> Wins: {activePlayer.Win} | Losses: {activePlayer.Loss} | Pushes: {activePlayer.Push}");

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
Console.WriteLine("\nGame finished! Your stats have been saved.");