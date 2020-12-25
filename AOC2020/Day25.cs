using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day25 : DayBase, ITwoPartQuestion
    {
        private readonly string Lines;

        public Day25()
        {
            Lines = File.ReadAllText("Input\\Day25.txt");

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