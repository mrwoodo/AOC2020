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
        public List<string> Lines;
        public StringBuilder GlobalRule = new StringBuilder();
        public Dictionary<string, StringBuilder> Rules = new Dictionary<string, StringBuilder>();
        public int[] MyTicket;
        public List<int[]> OtherTickets = new List<int[]>();

        public Day16()
        {
            Lines = InputFileAsStringList;
            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var readingRules = true;
            var i = 0;

            //Read Rules
            while (readingRules)
            {
                var line = Lines[i].Split(": ");

                if (line.Count() > 1)
                {
                    var splitNum = line[1].Split(" or ");
                    var rule = GetRule(splitNum);

                    Rules.Add(line[0], rule);

                    if (GlobalRule.Length == 0)
                        GlobalRule = new StringBuilder(rule.ToString());
                    else
                    {
                        for (int j = 0; j < MAX_WIDTH; j++)
                            if (rule[j] == '1' && GlobalRule[j] == '0')
                                GlobalRule[j] = '1';
                    }
                }
                else
                    readingRules = false;
                i++;
            }

            //Read My Ticket
            MyTicket = Lines[i + 1].Split(',').ToInts();

            //Read Other Tickets
            for (int j = i + 4; j < Lines.Count; j++)
                OtherTickets.Add(Lines[j].Split(',').ToInts());

            //Work out with other Tickets are invalid
            var failedScan = 0;
            for (int j = OtherTickets.Count - 1; j >= 0; j--)
            {
                var invalidTicket = false;
                foreach (var field in OtherTickets[j])
                {
                    if (GlobalRule[field] == '0')
                    {
                        failedScan += field;
                        invalidTicket = true;
                    }
                }

                if (invalidTicket)
                    OtherTickets.RemoveAt(j);
            }

            return $"Ticket scanning error rate : {failedScan}";
        }

        //Lets say a rule is "2-5 or 8-10", we make a StringBuilder which has:
        //00111100111 - The rule is treated like a mask against integer values
        //e.g. 4 is valid because char[4] = 1 whereas 7 is invalid because char[7] = 0
        private StringBuilder GetRule(string[] numRange)
        {
            var sb = new StringBuilder("".PadLeft(MAX_WIDTH, '0'));

            foreach (var range in numRange)
            {
                var r1 = GetRange(range);

                for (int i = r1.Item1; i <= r1.Item2; i++)
                    sb[i] = '1';
            }

            return sb;
        }

        private (int, int) GetRange(string n)
        {
            var s = n.Split('-');
            return (int.Parse(s[0]), int.Parse(s[1]));
        }

        public string Part2()
        {
            var potentialRules = new Dictionary<int, List<string>>();
            var usedrule = new Dictionary<string, int>();

            //Slice the tickets vertically to get column values,
            //then check which rules would fit these values
            for (int i = 0; i < OtherTickets[0].Length; i++)
                potentialRules.Add(i, GetRulesThatFitColumnValues((from col in OtherTickets select col[i]).ToList()));

            foreach (var rule in potentialRules.OrderBy(i => i.Value.Count))
            {
                //If only 1 rule fits for a column, it must be used
                if (rule.Value.Count == 1)
                    usedrule[(rule.Value[0])] = rule.Key;
                else
                {
                    //Otherwise find rules in order
                    foreach (var possibleField in rule.Value)
                    {
                        if (!usedrule.ContainsKey(possibleField))
                        {
                            usedrule[possibleField] = rule.Key;
                            break;
                        }
                    }
                }
            }

            long answer = 1;
            foreach (var key in usedrule.Keys.Where(i => i.StartsWith("departure")))
                answer *= MyTicket[usedrule[key]];

            return $"Departure fields multiplied : {answer}";
        }

        private List<string> GetRulesThatFitColumnValues(List<int> columnValues)
        {
            var result = new List<string>();

            foreach (var k in Rules.Keys)
            {
                var addRule = true;
                var rule = Rules[k];

                for (int i = 0; i < columnValues.Count(); i++)
                {
                    if (rule[columnValues[i]] == '0')
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
        public static int[] ToInts(this string[] stringValue)
        {
            return (from val in stringValue select int.Parse(val)).ToArray();
        }
    }
}