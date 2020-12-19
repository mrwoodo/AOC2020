using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day18 : DayBase, ITwoPartQuestion
    {
        public List<string> Lines = new List<string>();

        public Day18()
        {
            Lines = (from line in File.ReadAllLines("Input\\Day18.txt")
                     select line).ToList();

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var c = new Calculator();
            long sum = 0;

            foreach (var i in Lines)
                sum += c.Calculate(i);

            return $"Sum = {sum}";
        }

        public string Part2()
        {
            return "";
        }
    }

    public class Calculator
    {
        List<int> Levels;
        StringBuilder Line;

        public long Calculate(string l)
        {
            Line = new StringBuilder(l);
            Levels = new List<int>();
            var count = 0;

            //based on the brackets, set a level 'depth' for each character in the expression
            for (int i = 0; i < Line.Length; i++)
            {
                if (Line[i] == '(')
                    count++;
                else if ((i > 0) && (Line[i - 1] == ')'))
                    count--;

                Levels.Add(count);
            }

            //loop through these levels in reverse order, calculating as we go
            long result = 0;
            var levelToCheck = Levels.Max();
            while (levelToCheck >= 0)
            {
                InspectBracketLevel(levelToCheck, out result);

                if (!Levels.Any(i => i == levelToCheck))
                    levelToCheck--;
            }

            return result;
        }

        private void InspectBracketLevel(int level, out long result)
        {
            int start = -1;
            int end = -1;

            //work out the start/end chars of the first bracket found (bracket must be correct 'level')
            for (int i = 0; i < Levels.Count; i++)
            {
                if (Levels[i] == level)
                {
                    if (start == -1)
                        start = i;
                    end = i;
                }
                else if ((Levels[i] < level) && (end != -1))
                {
                    break;
                }
            }

            //strip out the bracket and calculate its components
            var sub = Line.ToString().Substring(start, end - start + 1);
            result = CalculateBracket(sub);

            //remove the bracket from the original
            Line.Remove(start, end - start + 1);
            Levels.RemoveRange(start, end - start + 1);

            //and substitute with the calc we just performed
            var replace = result.ToString();
            for (int x = 0; x < replace.Length; x++)
            {
                Line.Insert(start + x, replace[x]);
                Levels.Insert(start + x, level - 1);
            }
        }

        private long CalculateBracket(string s)
        {
            var split = s.Replace("(", "").Replace(")", "").Split(" ");
            var result = long.Parse(split[0]);

            for (int i = 1; i < split.Length; i++)
            {
                if (split[i] == "+")
                    result += long.Parse(split[i + 1]);
                else if (split[i] == "*")
                    result *= long.Parse(split[i + 1]);
            }

            return result;
        }
    }
}