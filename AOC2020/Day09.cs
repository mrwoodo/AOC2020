using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day09 : DayBase, ITwoPartQuestion
    {
        public List<long> Lines = new List<long>();
        public long incorrectNum = 0;

        public Day09()
        {
            Lines = (from line in InputFile.Split("\r\n")
                    select long.Parse(line)).ToList();
            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var preamble = 25;
            var window = new Queue<long>(preamble);

            foreach (var num in Lines)
            {
                if (window.Count == preamble)
                {
                    incorrectNum = IsValid(num, window);
                    if (incorrectNum > 0)
                        break;
                }

                window.Enqueue(num);
                if (window.Count > preamble)
                    window.Dequeue();
            }

            return $"First incorrect number : {incorrectNum}";
        }

        private long IsValid(long num, Queue<long> window)
        {
            for (int i = 0; i < window.Count; i++)
            {
                for (int j = 0; j < window.Count; j++)
                {
                    if (i != j)
                    {
                        if (window.ElementAt(i) + window.ElementAt(j) == num)
                            return 0;
                    }
                }
            }

            return num;
        }

        public string Part2()
        {
            for (int i = 0; i < Lines.Count - 1; i++)
            {
                var check = Lines[i];

                for (int j = i + 1; j < Lines.Count; j++)
                {
                    check += Lines[j];
                    if (check == incorrectNum)
                    {
                        var sequence = new List<long>();

                        for (int k = i; k <= j; k++)
                            sequence.Add(Lines[k]);

                        var smallest = sequence.Min();
                        var largest = sequence.Max();

                        return $"Smallest and largest in range : {smallest} + {largest} = {smallest + largest}";
                    }
                    else if (check > incorrectNum)
                        break;
                }
            }

            return "Couldn't find range :(";
        }
    }
}