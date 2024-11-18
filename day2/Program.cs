using System.Text.RegularExpressions;

namespace day2;

enum State {
    Start,
    Game
}

class Program
{
    
    static Regex regex = new Regex("^Game [0-9]+:| [0-9]+ blue|[0-9]+ red|[0-9]+ green|;|$");
    static Regex number = new Regex("[0-9]+");

    static int maxRed = 12;
    static int maxGreen = 13;
    static int maxBlue = 14;

    static void Main(string[] args)
    {
        
        try
        {
            using(var reader = new StreamReader("C:\\Dev\\Practice\\AoC2023\\aocday2.txt"))
            {
                string line;
                int total = 0;
                int totalPower = 0;
                while((line = reader.ReadLine()!) != null)
                {
                    int game = -1;
                    bool invalid = false;   
                    int red = 0, blue = 0, green = 0;
                    int highRed = 0, highBlue = 0, highGreen = 0;

                    var matches = regex.Matches(line).OrderBy(x => x.Index);
                    if(matches.Count() == 0)
                        return;
                    
                    foreach(var match in matches)
                    {
                        if(match.Value.Contains("Game"))
                        {
                            game = GetNum(match);

                            Console.WriteLine($"Game: {game}");
                        }
                        else if(match.Value.Contains("red"))
                        {
                            red = GetNum(match);
                        }
                        else if(match.Value.Contains("blue"))
                        {
                            blue = GetNum(match);
                        }
                        else if(match.Value.Contains("green"))
                        {
                            green = GetNum(match);
                        }
                        
                        if(match.Value.Equals(";") || match.Value.Equals(""))
                        {
                            if(red > maxRed || blue > maxBlue || green > maxGreen)
                            {
                                // invalid
                                Console.WriteLine($"Game {game} is invalid ({red}, {blue}, {green})");
                                invalid = true;
                            }

                            if(highRed < red)
                                highRed = red;
                            if(highBlue < blue)
                                highBlue = blue;
                            if(highGreen < green)
                                highGreen = green;
                            

                            // reset
                            red = 0; blue = 0; green = 0;
                        }

                        // only run on end of line
                        if(match.Value.Equals(""))
                        {
                            int power = highRed * highBlue * highGreen;
                            totalPower += power;
                        }
                    }

                    if(!invalid)
                    {
                        total += game;
                        Console.WriteLine($"Game {game} is valid, running total: {total}");
                    }
                }

                Console.WriteLine($"Total Power: {totalPower}");

            }

        }
        catch(Exception e)
        {
            Console.WriteLine($"Error reading input file: {e.ToString()}");
        }
    }

    private static int GetNum(Match? match)
    {
        int game;
        var countstr = number.Match(match!.Value);
        game = Int32.Parse(countstr.Value);
        return game;
    }
}