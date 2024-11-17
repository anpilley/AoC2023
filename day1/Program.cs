using System.Text.RegularExpressions;

namespace aoc2023;

class Program
{
    static Dictionary<string,int> numbers = new Dictionary<string,int>();
    static Regex fwdregex = new Regex("[1-9]|one|two|three|four|five|six|seven|eight|nine");
    static Regex bakregex = new Regex("[1-9]|one|two|three|four|five|six|seven|eight|nine", RegexOptions.RightToLeft);

    static void Main(string[] args)
    {
        numbers.Add("one", 1);
        numbers.Add("two", 2);
        numbers.Add("three", 3);
        numbers.Add("four", 4);
        numbers.Add("five", 5);
        numbers.Add("six", 6);
        numbers.Add("seven", 7);
        numbers.Add("eight", 8);
        numbers.Add("nine", 9);

        numbers.Add("1", 1);
        numbers.Add("2", 2);
        numbers.Add("3", 3);
        numbers.Add("4", 4);
        numbers.Add("5", 5);
        numbers.Add("6", 6);
        numbers.Add("7", 7);
        numbers.Add("8", 8);
        numbers.Add("9", 9);


        try
        {
            using (StreamReader reader = new StreamReader("c:\\dev\\Practice\\aoc2023\\aoc1data.txt"))
            {
                int total = 0;
                string line;
                while((line = reader.ReadLine()!) != null)
                {
                    int digits = FindDigits(line);

                    total += digits;
                }

                Console.WriteLine($"Total is {total}");
            }
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error, could not open file {args[1]}: {e.ToString()}");
        }
    }


    private static int FindDigits(string line)
    {
        int digits = 0;
        var match = fwdregex.Match(line);
        
        digits = ConvertNumber(match.Value);
        digits *= 10;

        match = bakregex.Match(line);

        digits += ConvertNumber(match.Value);

        return digits;
    }

    private static int ConvertNumber(string value)
    {
        if(numbers.ContainsKey(value))
            return numbers[value];

        throw new Exception("missing number");
    }
}
