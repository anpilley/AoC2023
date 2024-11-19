using System.Text.RegularExpressions;

namespace day4;

class Program
{
    static void Main(string[] args)
    {
        var regex = new Regex("(?<header>Card\\s+[0-9]+:)|(?<nums>[0-9]+(?!:))|(?<pipe>\\|)");

        using(var reader = new StreamReader("C:\\Dev\\Practice\\AoC2023\\aocday4.txt"))
        {
            string line;
            int total = 0;
            while((line = reader.ReadLine()!) != null)
            {
                var matches = regex.Matches(line);
                string card = "";
                int subtotal = 0;
                SortedSet<int> winners = new();
                SortedSet<int> values = new();
                bool winNumbers = true;
                foreach(Match match in matches)
                {
                    if(match.Groups["header"].Success)
                    {
                        card = match.Value;
                    }
                    else if(match.Groups["pipe"].Success)
                    {
                        winNumbers = false;
                    }
                    else if(match.Groups["nums"].Success)
                    {
                        if(winNumbers)
                        {
                            winners.Add(Int32.Parse(match.Value));    
                        }
                        else
                        {
                            values.Add(Int32.Parse(match.Value));
                        }
                    }
                }
                // total up winning matches.
                int wins = winners.Intersect(values).Count();
                subtotal = wins == 1 ? 1 : (int)Math.Pow(2, wins-1);
                total += subtotal;
                Console.WriteLine($"card: {card}, {winners.Count()}, {values.Count()}: {wins}, subtotal: {subtotal}, running total {total}");
            }
        }
    }
}
