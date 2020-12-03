using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day04 : DayBase, ITwoPartQuestion
    {
        public List<string> Lines = new List<string>();

        public Day04()
        {
            Lines = (from line in File.ReadAllLines("Input\\Day04.txt")
                     select line).ToList();

            Console.WriteLine(Part1());
            Console.WriteLine(Part2());
        }

        public string Part1()
        {
            return string.Empty;
        }

        public string Part2()
        {
            return string.Empty;
        }
    }
}