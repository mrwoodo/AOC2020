using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day23 : DayBase, ITwoPartQuestion
    {
        private readonly string Lines;

        public Day23()
        {
            Lines = File.ReadAllText("Input\\Day23.txt");

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            return "";
        }

        public string Part2()
        {
            return "";
        }
    }
}