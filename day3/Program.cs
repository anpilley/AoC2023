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

            long total = 0;
            long subtotal = 0;
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

    private static long Process(MatchCollection? prev, MatchCollection curr, MatchCollection? next)
    {
        long subtotal = 0;

        // safety check, and force  the collection to enumerate.
        if(curr== null || curr.Count() == 0)
            return 0;

        Stack<int> gears = new Stack<int>();

        // loop every match in the current line
        for(int i = 0; i < curr.Count(); i++)
        {
            Match match = curr[i];

            if(match.Groups["symbol"].Success == true && match.Value == "*")
            {
                // find the length of the string, and index
                int len = match.Value.Length;
                
                // find the range to look for
                // we don't care if these go past the bounds, they're just a range for comparison.
                int startidx = match.Index - 1;
                int endidx = match.Index + 1 ;

                // check for overlaps. An overlap is when !(endidx < p.Index || p.Index + len < startidx)
                // see https://stackoverflow.com/questions/17148839/overlapping-line-segments-in-2d-space
                
                // iterate over previous if not null
                if(prev != null)
                {
                    foreach(Match p in prev)
                    {
                        if(p.Groups["digits"].Success == true)
                        {
                            
                            if(!(endidx < p.Index || (p.Index + p.Value.Length - 1) < startidx)) 
                            {
                                // match
                                gears.Push(Int32.Parse(p.Value));
                            }
                        }
                    }
                }
                
                // check current line before/after
                if(i > 0)
                {
                    if(curr[i-1].Groups["digits"].Success == true && (curr[i-1].Index + curr[i-1].Value.Length -1) == startidx) 
                    {
                        gears.Push(Int32.Parse(curr[i-1].Value));
                    }
                }
                if(i < curr.Count() - 1)
                {
                    if(curr[i+1].Groups["digits"].Success == true && curr[i+1].Index == endidx) 
                    {
                        gears.Push(Int32.Parse(curr[i+1].Value));
                    }
                }

                // iterate over next, if not null.
                if(next != null)
                {
                    foreach(Match n in next)
                    {
                        if(n.Groups["digits"].Success == true)
                        {
                            if(!(endidx < n.Index || (n.Index + n.Value.Length - 1) < startidx)) 
                            {
                                // match
                                gears.Push(Int32.Parse(n.Value));
                            }
                        }
                    }
                }

                if(gears.Count == 2)
                {
                    // pop them both off, multiply, add to subtotal.
                    int gearval = gears.Pop();
                    gearval *= gears.Pop();
                    subtotal += gearval;
                }
                else if (gears.Count > 2)
                {
                    Console.WriteLine(gears.ToString());
                    throw new Exception("More gears than expected?!");
                }

                gears.Clear();
            }
        }

        return subtotal;
    }
}