using System.Text.RegularExpressions;

namespace day5;

class Program
{
    static void Main(string[] args)
    {
        var regex = new Regex("(?<data>[0-9]+)|(?<header>[\\w+\\-]+ map:)|(?<seed>seeds:)");
    }
}
