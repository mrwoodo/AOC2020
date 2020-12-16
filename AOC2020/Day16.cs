using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day16 : DayBase, ITwoPartQuestion
    {
        public const int MAX_WIDTH = 1000;
        public List<string> Lines = new List<string>();
        public StringBuilder globalRule = new StringBuilder();
        public Dictionary<string, StringBuilder> Rules = new Dictionary<string, StringBuilder>();
        public int[] MyTicket;
        public List<int[]> NearbyTickets = new List<int[]>();

        public Day16()
        {
            Lines = (from line in File.ReadAllLines("Input\\Day16.txt")
                     select line).ToList();

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var readingRules = true;
            var i = 0;

            while (readingRules)
            {
                var line = Lines[i].Split(": ");

                if (line.Count() > 1)
                {
                    var splitNum = line[1].Split(" or ");
                    var rule = getRule(splitNum);

                    Rules.Add(line[0], rule);

                    if (globalRule.Length == 0)
                        globalRule = new StringBuilder(rule.ToString());
                    else
                    {
                        for (int j = 0; j < MAX_WIDTH; j++)
                            if (rule[j] == '1' && globalRule[j] == '0')
                                globalRule[j] = '1';
                    }
                }
                else
                    readingRules = false;
                i++;
            }

            MyTicket = Lines[i + 1].Split(',').ToInts();

            for (int j = i + 4; j < Lines.Count; j++)
                NearbyTickets.Add(Lines[j].Split(',').ToInts());

            var failedScan = 0;

            for (int j = NearbyTickets.Count - 1; j >= 0; j--)
            {
                var failed = false;
                foreach (var field in NearbyTickets[j])
                {
                    if (globalRule[field] == '0')
                    {
                        failedScan += field;
                        failed = true;
                    }
                }

                if (failed)
                    NearbyTickets.RemoveAt(j);
            }

            return $"Ticket scanning error rate : {failedScan}";
        }

        private StringBuilder getRule(string[] numRange)
        {
            var sb = new StringBuilder("".PadLeft(MAX_WIDTH, '0'));

            foreach (var range in numRange)
            {
                var r1 = getRange(range);

                for (int i = r1.Item1; i <= r1.Item2; i++)
                    sb[i] = '1';
            }

            return sb;
        }

        private (int, int) getRange(string n)
        {
            var s = n.Split('-');

            return (int.Parse(s[0]), int.Parse(s[1]));
        }

        public string Part2()
        {
            var colRules = new Dictionary<int, List<string>>();

            for (int i = 0; i < NearbyTickets[0].Length; i++)
            {
                var col = new List<int>();

                foreach (var t in NearbyTickets)
                    col.Add(t[i]);

                colRules.Add(i, validateColumnAgainstRule(col));
            }

            var ordered = from r in colRules orderby r.Value.Count select r;
            var used = new List<string>();
            var usedCol = new List<int>();

            foreach (var col in ordered)
            {
                if (col.Value.Count == 1)
                {
                    used.Add(col.Value[0]);
                    usedCol.Add(col.Key);
                }
                else
                {
                    foreach (var possibleField in col.Value)
                    {
                        if (!used.Contains(possibleField))
                        {
                            used.Add(possibleField);
                            usedCol.Add(col.Key);
                            break;
                        }
                    }
                }
            }

            long answer = 1;
            for (int i = 0; i < used.Count(); i++)
            {
                if (used[i].StartsWith("departure"))
                    answer *= MyTicket[usedCol[i]];
            }

            return $"Departure fields multiplied : {answer}";
        }

        private List<string> validateColumnAgainstRule(List<int> vals)
        {
            var result = new List<string>();

            foreach (var k in Rules.Keys)
            {
                var addRule = true;
                var rule = Rules[k];

                for (int i = 0; i < vals.Count(); i++)
                {
                    if (rule[vals[i]] == '0')
                    {
                        addRule = false;
                        break;
                    }
                }

                if (addRule)
                    result.Add(k);
            }

            return result;
        }
    }

    public static class Extensions
    {
        public static int[] ToInts(this string[] str)
        {
            var r = (from s in str select int.Parse(s));

            return r.ToArray();
        }
    }
}