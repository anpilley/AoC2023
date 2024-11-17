namespace aoc2023;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            using (StreamReader reader = new StreamReader("c:\\dev\\Practice\\aoc2023\\aoc1data.txt"))
            {
                int total = 0;
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    int digits = 0;
                    for(int i = 0; i < line.Length; i++)
                    {
                        if((line[i]) >= '0' && line[i] <= '9')
                        {
                            digits = Int32.Parse(line[i].ToString());
                            break;
                        }
                    }

                    digits *= 10;

                    for(int i = line.Length - 1; i >= 0; i--)
                    {
                        if((line[i]) >= '0' && line[i] <= '9')
                        {
                            digits += Int32.Parse(line[i].ToString());
                            break;
                        }
                    }

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
}
