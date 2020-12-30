using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    public class Day13 : DayBase, ITwoPartQuestion
    {
        public List<string> Lines;
        public List<int> Buses = new List<int>();
        public int EarliestTime = 0;

        public Day13()
        {
            Lines = InputFileAsStringList;
            EarliestTime = int.Parse(Lines[0]);
            Buses = (from l in Lines[1].Split(',').Where(i => !i.Equals("x"))
                     select int.Parse(l)).ToList();

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            double earliest = 999999;
            int busNumber = 0;

            foreach (var bus in Buses)
            {
                float check = (float)EarliestTime / (float)bus;
                var nextBus = Math.Floor(check) * bus + bus;
                var minutesToWait = nextBus - EarliestTime;
                if (minutesToWait < earliest)
                {
                    earliest = minutesToWait;
                    busNumber = bus;
                }
            }

            return $"Earliest bus is #{busNumber}, wait {earliest} minutes = {busNumber * earliest}";
        }

        public string Part2()
        {
            return "";
        }
    }
}