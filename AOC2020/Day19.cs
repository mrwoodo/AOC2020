using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    public class Day19 : DayBase, ITwoPartQuestion
    {
        public Day19()
        {
            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var Rules = new Dictionary<int, Rule>();
            var Messages = new List<string>();
            LoadRules(Rules, Messages);
            ParseRules(Rules);

            var validMessages = (from Message in Messages
                                 let result = Rules[0].ParseRule(Message, (0, true))
                                 where (result.Item2) && (result.Item1 == Message.Length)
                                 select Message).Count();

            return $"Messages passing validation : {validMessages}";
        }

        private void LoadRules(Dictionary<int, Rule> Rules, List<string> Messages)
        {
            foreach (var (l, search) in from l in InputFileAsStringList
                                        where l.Length > 0
                                        let search = l.IndexOf(":")
                                        select (l, search))
            {
                if (search > 0)
                {
                    var key = l.Substring(0, search);
                    var value = l[(search + 2)..];
                    Rules.Add(int.Parse(key), new Rule(value));
                }
                else
                    Messages.Add(l);
            }
        }

        private void ParseRules(Dictionary<int, Rule> Rules)
        {
            foreach (var k in Rules.Keys)
            {
                var r = Rules[k];

                if (r.Char == null)
                {
                    if (r.RuleString.Contains("|"))
                    {
                        var parts = r.RuleString.Split("|");
                        r.SubRules.AddRange(from s in parts[0].Split(" ", System.StringSplitOptions.RemoveEmptyEntries)
                                            select Rules[int.Parse(s)]);
                        r.SubRulesAlt.AddRange(from s in parts[1].Split(" ", System.StringSplitOptions.RemoveEmptyEntries)
                                               select Rules[int.Parse(s)]);
                    }
                    else
                    {
                        r.SubRules.AddRange(from s in r.RuleString.Split(" ", System.StringSplitOptions.RemoveEmptyEntries)
                                            select Rules[int.Parse(s)]);
                    }
                }
            }
        }

        public string Part2()
        {
            return $"";
        }
    }

    public class Rule
    {
        public string Char { get; set; }
        public string RuleString { get; set; }
        public List<Rule> SubRules { get; set; }
        public List<Rule> SubRulesAlt { get; set; }

        public Rule(string Input)
        {
            this.RuleString = Input;
            if (Input.Contains("\""))
                this.Char = Input.Replace("\"", "");

            SubRules = new List<Rule>();
            SubRulesAlt = new List<Rule>();
        }

        public (int, bool) ParseRule(string Input, (int, bool) from)
        {
            if (Char != null)
            {
                if (Input.Substring(from.Item1, 1) == Char)
                    return (from.Item1 + 1, true);
                return (0, false);
            }

            var to = from;
            var passed = true;
            foreach (var subrule in SubRules)
            {
                to = subrule.ParseRule(Input, to);
                if (!to.Item2)
                {
                    passed = false;
                    break;
                }
            }

            if (passed)
                return (to.Item1, true);

            if (SubRulesAlt.Count > 0)
            {
                passed = true;
                to = from;
                foreach (var subrule in SubRulesAlt)
                {
                    to = subrule.ParseRule(Input, to);
                    if (!to.Item2)
                    {
                        passed = false;
                        break;
                    }
                }
                if (passed)
                    return (to.Item1, true);
            }
            return (from.Item1, false);
        }
    }
}