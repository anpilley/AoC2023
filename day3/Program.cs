using System.Text.RegularExpressions;

namespace day3;



class Program
{
    static void Main(string[] args)
    {
        var regex = new Regex("(?<digits>[0-9]+)|(?<symbol>(?!\\.|[0-9]).)");

        using(var reader = new StreamReader("C:\\Dev\\Practice\\AoC2023\\aocday3.txt"))
        {
            // need the previous and next to look for symbols.
            MatchCollection? prev = null;
            MatchCollection? curr = null;
            MatchCollection? next = null;
            string line;

            // pre-read and regex a line into "next"
            line = reader.ReadLine()!;
            if(line == null)
                return;
            next = regex.Matches(line);

            int total = 0;
            int subtotal = 0;
            int linecount = 0;
            while((line = reader.ReadLine()!) != null)
            {
                linecount++;
                var matches = regex.Matches(line);
                prev = curr;
                curr = next;
                next = matches;

                subtotal = Process(prev, curr, next);
                Console.WriteLine($"Line {linecount}: subtotal: {subtotal}, running total {total}");
                total += subtotal;
                
            }

            prev = curr;
            curr = next;
            next = null;
            linecount++;

            subtotal = Process(prev, curr, next);
            total+= subtotal;
            Console.WriteLine($"Line {linecount}: subtotal: {subtotal}, running total {total}");

            Console.WriteLine($"Total: {total}");
        }

    }

    private static int Process(MatchCollection? prev, MatchCollection curr, MatchCollection? next)
    {
        int subtotal = 0;

        // safety check, and force  the collection to enumerate.
        if(curr== null || curr.Count() == 0)
            return 0;

        // loop every match in the current line
        for(int i = 0; i < curr.Count(); i++)
        {
            Match match = curr[i];

            bool found = false;
            if(match.Groups["digits"].Success == true)
            {
                // find the length of the string, and index
                int len = match.Value.Length;
                
                // find the range to look for
                // we don't care if these go past the bounds, they're just a range for comparison.
                int startidx = match.Index - 1;
                int endidx = match.Index + len ;
                
                // iterate over previous if not null
                if(prev != null)
                {
                    foreach(Match p in prev)
                    {
                        if(p.Groups["symbol"].Success == true)
                        {
                            if(p.Index >= startidx && p.Index <= endidx)
                            {
                                // match
                                subtotal += Int32.Parse(match.Value);
                                found = true;
                                break;
                            }
                        }
                    }
                }

                // move on to the next;
                if(found)
                    continue;
                
                // check current line before/after
                if(i > 0)
                {
                    if(curr[i-1].Groups["symbol"].Success == true && curr[i-1].Index == startidx){
                        subtotal += Int32.Parse(match.Value);
                        continue;
                    }
                }
                if(i < curr.Count() - 1)
                {
                    if(curr[i+1].Groups["symbol"].Success == true && curr[i+1].Index == endidx){
                        subtotal += Int32.Parse(match.Value);
                        continue;
                    }
                }

                // iterate over next, if not null.
                if(next != null)
                {
                    foreach(Match n in next)
                    {
                        if(n.Groups["symbol"].Success == true)
                        {
                            if(n.Index >= startidx && n.Index <= endidx)
                            {
                                // match
                                subtotal += Int32.Parse(match.Value);
                                found = true;
                                break;
                            }
                        }
                    }
                    if(found)
                        continue;
                }
            }
        }

        return subtotal;
    }
}