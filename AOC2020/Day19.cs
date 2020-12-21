using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day19 : DayBase, ITwoPartQuestion
    {
        public List<string> Lines = new List<string>();

        public Day19()
        {
            Lines = (from line in File.ReadAllLines("Input\\Day19.txt")
                     select line).ToList();

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            return $"";
        }

        public string Part2()
        {
            return $"";
        }
    }
}