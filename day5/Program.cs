using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day5;

enum MapType{
    Seeds,
    SeedSoil,
    SoilFert,
    FertWater,
    WaterLight,
    LightTemp,
    TempHumid,
    HumidLocation
}

public class MapEntry : IComparable<MapEntry>
{
    public long dest; 
    public long source; 
    public long range;
    
    public MapEntry(long dest, long source, long range)  
    {
        this.dest = dest;
        this.source = source;
        this.range = range;
    }
    

    public int CompareTo(MapEntry? other)
    {
        if(other == null)
            return -1;
        
        MapEntry comp = other;
        if(source != comp.source)
        {
            if(source > comp.source)
                return 1;
            if(source < comp.source)
                return -1;
        }
        if(range != comp.range)
        {
            if(range > comp.range)
                return 1;
            if(range < comp.range)
                return -1;
        }

        if(dest != comp.dest)
        {
            if(dest > comp.dest)
                return 1;
            if(dest < comp.dest)
                return -1;
        }

        return 0;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var regex = new Regex("(?<seed>seeds:)|(?<data>[0-9]+)|(?<header>[\\w+\\-]+ map:)");

        SortedSet<MapEntry> SeedSoilMap = new();
        SortedSet<MapEntry> SoilFertMap = new();
        SortedSet<MapEntry> FertWaterMap = new();
        SortedSet<MapEntry> WaterLightMap = new();
        SortedSet<MapEntry> LightTempMap = new();
        SortedSet<MapEntry> TempHumidMap = new();
        SortedSet<MapEntry> HumidLocationMap = new();
        SortedSet<long> SeedSet = new();

        using(var reader = new StreamReader("C:\\Dev\\Practice\\AoC2023\\aocday5.txt"))
        {
            MapType state = MapType.Seeds;
            string line;
            SortedSet<MapEntry> current = null;
            while((line = reader.ReadLine())!= null)
            {
                var matches = regex.Matches(line);

                foreach(Match match in matches)
                {
                    // check for various headers
                    if(match.Groups["seed"].Success)
                    {
                        state = MapType.Seeds;
                    }
                    else if(match.Groups["header"].Success)
                    {
                        switch(match.Value)
                        {
                            case "seed-to-soil map:":
                                state = MapType.SeedSoil;
                                current = SeedSoilMap;
                                break;
                            case "soil-to-fertilizer map:":
                                state = MapType.SoilFert;
                                current = SoilFertMap;
                                break;
                            case "fertilizer-to-water map:":
                                state = MapType.FertWater;
                                current = FertWaterMap;
                                break;
                            case "water-to-light map:":
                                state = MapType.WaterLight;
                                current = WaterLightMap;
                                break;
                            case "light-to-temperature map:":
                                state = MapType.LightTemp;
                                current = LightTempMap;
                                break;
                            case "temperature-to-humidity map:":
                                state = MapType.TempHumid;
                                current = TempHumidMap;
                                break;
                            case "humidity-to-location map:":
                                state = MapType.HumidLocation;
                                current = HumidLocationMap;
                                break;
                            default:
                                throw new Exception($"Unexpected header {match.Value}?");
                        }
                    }
                    else if(match.Groups["data"].Success)
                    {
                        // check if it's a digit, insert into current map
                        if(state == MapType.Seeds)
                        {
                            SeedSet.Add(Int64.Parse(match.Value));
                        }
                        else
                        {
                            if(matches.Count()!= 3)
                                throw new Exception("Unexpected data count");
                            long dest = Int64.Parse(matches[0].Value);
                            long source = Int64.Parse(matches[1].Value);
                            long range = Int64.Parse(matches[2].Value);
                            var entry = new MapEntry(dest, source, range);
                            current.Add(entry);
                            continue; // we looked ahead, skip the rest of the line.
                        }
                    }

                }
                
            }

            Console.WriteLine($"Seed count: {SeedSet.Count()}");
            Console.WriteLine($"SeedSoil count: {SeedSoilMap.Count()}");
            Console.WriteLine($"SoilFert count: {SoilFertMap.Count()}");
            Console.WriteLine($"FertWater count: {FertWaterMap.Count()}");
            Console.WriteLine($"WaterLight count: {WaterLightMap.Count()}");
            Console.WriteLine($"LightTemp count: {LightTempMap.Count()}");
            Console.WriteLine($"TempHumid count: {TempHumidMap.Count()}");
            Console.WriteLine($"HumidLoc count: {HumidLocationMap.Count()}");

            // search.
            long lowestloc = long.MaxValue;
            foreach(long seed in SeedSet)
            {
                long loc = Traverse(seed, SeedSoilMap, nameof(SeedSoilMap));
                loc = Traverse(loc, SoilFertMap, nameof(SoilFertMap));
                loc = Traverse(loc, FertWaterMap, nameof(FertWaterMap));
                loc = Traverse(loc, WaterLightMap, nameof(WaterLightMap));
                loc = Traverse(loc, LightTempMap, nameof(LightTempMap));
                loc = Traverse(loc, TempHumidMap, nameof(TempHumidMap));
                loc = Traverse(loc, HumidLocationMap, nameof(HumidLocationMap));
                if(loc < lowestloc)
                    lowestloc = loc;
            }

            Console.WriteLine($"Nearest location is {lowestloc}");


        }
    }

    static long Traverse(long n, IReadOnlySet<MapEntry> map, string setname)
    {
        long orig = n;
        foreach(var entry in map)
        {
            if(n >= entry.source && n <= (entry.source + entry.range) )
            {
                n = n - entry.source + entry.dest;
                Console.WriteLine($"Mapped {orig} to {n} with {setname}");
                return n;      
            }
        }

        return n;
    }
}
