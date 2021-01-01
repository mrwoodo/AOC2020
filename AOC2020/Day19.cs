using System;
using System.Collections.Generic;
using System.Text;

namespace AOC2020
{
    public class Day19 : DayBase, ITwoPartQuestion
    {
        Dictionary<int, Rule> Rules;
        List<string> Messages;

        public Day19()
        {
            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            Rules = new Dictionary<int, Rule>();
            Messages = new List<string>();

            foreach (var l in InputFileAsStringList)
            {
                if (l.Length > 0)
                {
                    var srch = l.IndexOf(":");
                    if (srch > 0)
                    {
                        var key = l.Substring(0, srch);
                        var val = l.Substring(srch + 2);
                        Rules.Add(int.Parse(key), new Rule(int.Parse(key), val));
                    }
                    else
                    {
                        Messages.Add(l);
                    }
                }
            }

            foreach (var k in Rules.Keys)
            {
                var r = Rules[k];

                if (r.CharValue == null)
                {
                    if (r.RuleString.Contains("|"))
                    {
                        var or = r.RuleString.Split("|");
                        var sub = or[0].Split(" ", System.StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < sub.Length; i++)
                        {
                            var lookup = Rules[int.Parse(sub[i])];
                            r.SubRules.Add(lookup);
                        }
                        sub = or[1].Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < sub.Length; i++)
                        {
                            var lookup = Rules[int.Parse(sub[i])];
                            r.SubRulesAlt.Add(lookup);
                        }
                    }
                    else
                    {
                        var sub = r.RuleString.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < sub.Length; i++)
                        {
                            var lookup = Rules[int.Parse(sub[i])];
                            r.SubRules.Add(lookup);
                        }
                    }
                }
            }

            foreach (var m in Messages)
            {
                Console.Write($"{m}\t");
                if (Rules[0].ParseRule(m, 0))
                    Console.WriteLine("VALID");
                else
                    Console.WriteLine();
            }

            return $"";
        }

        public string Part2()
        {
            return $"";
        }
    }

    public class Rule
    {
        public int ID { get; set; }
        public string CharValue { get; set; }
        public string RuleString { get; set; }
        public List<Rule> SubRules { get; set; }
        public List<Rule> SubRulesAlt { get; set; }

        public Rule(int id, string Input)
        {
            this.ID = id;
            this.RuleString = Input;
            if (Input.Contains("\""))
                this.CharValue = Input.Replace("\"", "");

            SubRules = new List<Rule>();
            SubRulesAlt = new List<Rule>();
        }

        public bool ParseRule(string Input, int fromPosition)
        {
            Console.WriteLine($"\r\nRule {this.ID} : {this.RuleString}, checking {Input}");

            if (CharValue != null)
            {
                Console.WriteLine($"{((Input.Substring(fromPosition, 1) == CharValue) ? "passed!" : "failed!")}");
                return (Input.Substring(fromPosition, 1) == CharValue);
            }

            var result = true;
            foreach (var subrule in SubRules)
            {
                result &= subrule.ParseRule(Input, fromPosition);
                if (result)
                    fromPosition++;
                else
                    break;
            }

            if ((SubRulesAlt.Count == 0) || result)
                return result;

            result = true;
            foreach (var subrule in SubRulesAlt)
            {
                result &= subrule.ParseRule(Input, fromPosition);
                if (result)
                    fromPosition++;
                else
                    break;
            }

            return result;
        }
    }
}