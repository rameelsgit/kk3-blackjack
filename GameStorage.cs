using Blackjack.models;
using System.IO;
using System.Security;
public class GameStorage
{
    // metoden hämtar lista av spelare och deras stats
    public void SavePlayers (List<Player> players)
    {
        // spelares data sparas i textfilen
        string FilePath = "players.txt";
        List<string> lines = new List<string>();

        //foreach loopen sparar spelarens data som namn, vinst osv
        foreach (Player splayer in players)
        {
            lines.Add($"{splayer.Name},{splayer.Win},{splayer.Loss},{splayer.Push}");
        }
        //sparar i player.txt
        File.WriteAllLines(FilePath, lines);
    
    }

    //metoden läser in och hämtar data från player.txt
    public List<Player> LoadPlayers()
    {
        string filePath = "players.txt";
        List<Player> loadedPlayers = new List<Player>();

        //if-satsen kollar om filen finns, om inte så laddas programmet med tom fil
        if (!File.Exists(filePath))
        {
            return loadedPlayers;
        }
        string[] lines = File.ReadAllLines(filePath);

        //loopen används för att bestämma hur varje linje i filen ska hanteras
        foreach (string line in lines)
        {
            //delar upp array vid "," till individuella strings
            string[] parts = line.Split(',');

            //ser till att det finns fyra fält av namn,vinst,förlust och lika
            if (parts.Length == 4)
            {
                string name = parts[0];
                //if-sats av int.tryParse för att hörhindra krasch
                if (int.TryParse(parts[1], out int win) &&
                    int.TryParse(parts[2], out int loss) &&
                    int.TryParse(parts[3], out int push))
                    {
                        //skapar ny spelare, hämtar stats och sparar om if sats är sann
                        Player player = new Player(name);
                        player.SetStats(win, loss, push);
                        loadedPlayers.Add(player);
                    }

            }
        }
        return loadedPlayers;
    }


    
}