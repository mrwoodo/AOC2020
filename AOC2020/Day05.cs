using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day05 : DayBase, ITwoPartQuestion
    {
        public Day05()
        {
            var x = (from line in File.ReadAllLines("Input\\Day05.txt")
                        select int.Parse(line)).ToList();

            Console.WriteLine(Part1());
            Console.WriteLine(Part2());
        }

        public string Part1()
        {
            return String.Empty;
        }

        public string Part2()
        {
            return String.Empty;
        }
    }
}