using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day06 : DayBase, ITwoPartQuestion
    {
        public string[] Groups = File.ReadAllText("Input\\Day06.txt").Split("\r\n\r\n");

        public Day06()
        {
            Console.WriteLine(Part1());
            Console.WriteLine(Part2());
        }

        public string Part1()
        {
            var score = 0;
            var answers = new List<string>();

            foreach (var g in Groups)
                answers.Add(g.Replace("\r\n", ""));

            foreach (var ans in answers)
                score += (from a in ans select a).Distinct().Count();

            return $"Sum of Yes answers : {score}";
        }

        public string Part2()
        {
            var score = 0;

            foreach (var group in Groups)
            {
                var tally = new Dictionary<char, int>();
                var people = group.Split("\r\n");

                foreach (var person in people)
                {
                    foreach (var answer in person)
                    {
                        if (!tally.ContainsKey(answer))
                            tally.Add(answer, 0);

                        tally[answer] += 1;
                    }
                }

                score += tally.Count(i => i.Value == people.Length);
            }

            return $"Count of Yes answers : {score}";
        }
    }
}