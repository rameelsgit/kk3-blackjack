# Blackjack - Konsolspel

Detta är ett Blackjack spel som startas med dotnet run i terminalen. Spelet stödjer upp till 5 spelare (människor och datorstyrda bottar) och körs direkt i konsolen. Spelet är uppbyggt av flera klasser: Participant är basklass för Player, Dealer och ComputerPlayer. Hand beräknar poäng och hanterar Ess. Deck skapar och blandar kortleken. Round styr omgångar och insatser, medan Game håller ihop spelsessionen. RuleSet läser inställningar från rules.txt och GameStorage sparar statistik i players.txt. Graphics och ConsoleUi hanterar kortgrafik och användargränssnitt.

Alla fyra tillval har byggts i projektet:

Kortgrafik: Graphics.cs ritar kort bredvid varandra med ramar, Unicode-symboler och dolda hålkort. ConsoleUi hanterar röda och vita färger. (Ramez Khurshid)

Bettingsystem: Marker sparas i players.txt. (Rameel Siddiqui)

Datorstyrda spelare: Bottar styrs via IPlayerStrategy med 5 olika strategier. (Rojvan Qasim)

Konfigurationsfil: RuleSet.cs läser regler från rules.txt med fail-safe om filen saknas. (Talha Malik)

Reglerna följer klassisk Blackjack. Kända brister är om en enskild rad saknas i rules.txt används defaultvärdet utan att användaren varnas om vilken specifik rad som fattades.

# Klass diagram i mermaid

The diagram includes the main properties, methods, and relationships in the project.

```mermaid
classDiagram
    direction TB

    namespace Blackjack.models {
    class Game {
        -Deck _deck
        -List~Player~ _players
        -Dealer _dealer
        +RuleSet Rules
        +CurrentPlayers List~Player~
        +CurrentDealer Dealer
        +Game(players, rules)
        +ResetHands() void
        +StartNewRound() Round
    }
    class Round {
        -Deck RoundDeck
        -List~Player~ RoundPlayers
        -Dealer RoundDealer
        -Outcomes RoundOutcome
        -Dictionary~Player, int~ PlayerBets
        -RuleSet Rules
        +Round(deck, players, dealer, rules)
        +PlaceBets(player, betAmounts) void
        +GetBet(player) int
        +DealCards() void
        +PlayerTurn(move, player) void
        +DealerTurn() void
        +EvaluateOutcome(player) Outcomes
    }
    class RuleSet {
        +bool DealerStandsOnSoft17
        +int Reshuffle
        +int DeckCount
        +bool NaturalBlackjack
        +LoadFromFile(filePath) RuleSet
    }
    class Deck {
        -List~Card~ _cards
        -int _deckCount
        +Deck(deckCount)
        +Reset() void
        +Shuffle() void
        +DrawCard() Card
        +CardsRemaining() int
    }
    class Card {
        +Suit Suit
        +Ranks Rank
        +Card(suit, rank)
        +GetValue() int
        +ToString() string
    }
    class Hand {
        +List~Card~ Cards
        +AddCard(card) void
        +ClearHand() void
        +CalculateScore() int
        +IsBust() bool
        +Blackjack() bool
    }
    class Participant {
        +string Name
        +Hand Hand
        +Participant(name)
        +ReceiveCard(card) void
    }
    class Player {
        +int Win
        +int Loss
        +int Push
        +int Chips
        +Player(name)
        +AddWin() void
        +AddLoss() void
        +AddPush() void
        +setChips(amount) void
        +AddChips(amount) void
        +RemoveChips(amount) void
        +SetStats(win, loss, push) void
    }
    class Dealer {
        +Dealer()
        +NotHit(rules) bool
    }
    class ComputerPlayer {
        +IPlayerStrategy Strategy
        +ComputerPlayer(name, strategy)
        +ChooseMove(dealerUpCard) Moves
    }
    class IPlayerStrategy {
        <<interface>>
        +string Name
        +Decide(hand, dealerUpCard) Moves
    }
    class BasicStrategy {
        +string Name
        +Decide(hand, dealerUpCard) Moves
    }
    class CautiousStrategy {
        +string Name
        +Decide(hand, dealerUpCard) Moves
    }
    class DealerAwareStrategy {
        +string Name
        +Decide(hand, dealerUpCard) Moves
    }
    class HitUnder17Strategy {
        +string Name
        +Decide(hand, dealerUpCard) Moves
    }
    class RiskyStrategy {
        +string Name
        +Decide(hand, dealerUpCard) Moves
    }
    }

    class GameStorage {
        +SavePlayers(players) void
        +LoadPlayers() List~Player~
    }
    namespace Blackjack.Graphics {
    class Graphics {
        +GetCardLines(card, isHidden) string[]
        -GetSuitSymbol(suit) string
        -GetRankSymbol(rank) string
    }
    }
    class Program {
        <<top-level>>
        +Main() void
        +PrintHand(ownerName, hand, hideFirstCard) void
    }
    namespace Blackjack.enums {
    class Suit {
        <<enumeration>>
        Hearts
        Diamonds
        Clubs
        Spades
    }
    class Ranks {
        <<enumeration>>
        Two
        Three
        Four
        Five
        Six
        Seven
        Eight
        Nine
        Ten
        Jack
        Queen
        King
        Ace
    }
    class Moves {
        <<enumeration>>
        Hit
        Stand
    }
    class Outcomes {
        <<enumeration>>
        PlayerWins
        DealerWins
        Push
        PlayerBlackjack
        PlayerBust
        DealerBust
    }
    }

    Participant <|-- Player
    Participant <|-- Dealer
    Player <|-- ComputerPlayer

    IPlayerStrategy <|.. BasicStrategy
    IPlayerStrategy <|.. CautiousStrategy
    IPlayerStrategy <|.. DealerAwareStrategy
    IPlayerStrategy <|.. HitUnder17Strategy
    IPlayerStrategy <|.. RiskyStrategy
    ComputerPlayer --> IPlayerStrategy : uses

    Participant "1" *-- "1" Hand : owns
    Hand "1" *-- "0..*" Card : holds
    Deck "1" *-- "0..*" Card : contains
    Card ..> Suit : uses
    Card ..> Ranks : uses

    Game "1" *-- "1" Deck : manages
    Game "1" o-- "1..*" Player : manages
    Game "1" o-- "1" Dealer : manages
    Game --> RuleSet : reads
    Game ..> Round : creates
    Round --> Deck : draws from
    Round "1" o-- "1..*" Player : plays
    Round --> Dealer : plays
    Round --> RuleSet : applies
    Round ..> Moves : handles
    Round ..> Outcomes : returns
    IPlayerStrategy ..> Hand : evaluates
    IPlayerStrategy ..> Card : checks dealer card
    IPlayerStrategy ..> Moves : returns
    Dealer ..> RuleSet : checks

    Graphics ..> Card : renders
    Graphics ..> Suit : renders
    Graphics ..> Ranks : renders
    GameStorage ..> Player : loads and saves
    Program ..> GameStorage : loads and saves players
    Program ..> Game : runs
    Program ..> Round : controls
    Program ..> IPlayerStrategy : configures computer players
    Program ..> Graphics : renders hands
```

`Program` represents the top-level statements in `ConsoleUi.cs`.
