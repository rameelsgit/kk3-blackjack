
//Testar om allt som har gjort hittils funkar.

using Blackjack.enums;
using Blackjack.models;

Deck deck= new Deck();


Player player = new Player("Talha");
player.ReceiveCard(deck.DrawCard());
player.ReceiveCard(deck.DrawCard());
Dealer dealer= new();
dealer.ReceiveCard(deck.DrawCard());
dealer.ReceiveCard(deck.DrawCard());


Console.WriteLine(player.Hand.CalculateScore());
Console.WriteLine(dealer.Hand.CalculateScore());


if (player.Hand.IsBust())
{
    Console.WriteLine("Bust!!!");
}
if (player.Hand.Blackjack())
{
    Console.WriteLine("BlackJack!!!!");
}
if (dealer.NotHit())
{
    Console.WriteLine("works");
}
    

