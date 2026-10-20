namespace Blackjack.models;

class RuleSet
{
    // Properties med default värde (används om filen inte hittas)
    public bool DealerStandsOnSoft17 {get; set;}=true;
    public int Reshuffle{get; set;}= 15;
    public int DeckCount{get; set;}= 1;
    public bool NaturalBlackjack{get; set;}= true;


    public static RuleSet LoadFromFile(string filePath= "rules.txt")
    {
        // Ny RuleSet object som innehåller default värdet
        RuleSet rules= new RuleSet();

        // Kontrollerar om filen finns om inte då körs med default värdet.
        if(!File.Exists(filePath))
        {
            return rules;
        }

        // Läser igenom filen och delar upp det i array (lines).
        string[] lines= File.ReadAllLines(filePath);

        foreach (string line in lines)
        {
            // En till array som delar upp raden vid '='
            // T.ex "Reshuffle=0" parts[0]= "Reshuffle", parts[1]= "15":
            string[] parts= line.Split('=');

            if (parts.Length==2)
            {
                string key= parts[0].Trim();
                string value=parts[1].Trim();

                if (key=="DealerStandsOnSoft17")
                {
                    if (bool.TryParse(value,out bool result))
                    {
                    rules.DealerStandsOnSoft17= result;
                    }
                }
                else if (key=="DeckCount")
                {
                    if (int.TryParse(value,out int result))
                    {
                    rules.DeckCount= result;
                    }
                }
                else if (key=="Reshuffle")
                {
                    if (int.TryParse(value,out int result))
                    {
                    rules.Reshuffle= result;
                    }
                }
                else if (key=="NaturalBlackjack")
                {
                    if (bool.TryParse(value,out bool result))
                    {
                    rules.NaturalBlackjack= result;
                    }
                }
                
            }
        }
        return rules;

    }

}