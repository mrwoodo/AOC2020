using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    public class Day19 : DayBase, ITwoPartQuestion
    {
        public Dictionary<int, Rule> Rules = new Dictionary<int, Rule>();
        public List<string> Messages = new List<string>();

        public Day19()
        {
            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            LoadRules(InputFileAsStringList);
            ParseRules();

            return ValidateMessges();
        }

        public string Part2()
        {
            var source = InputFile;
            source = source.Replace("8: 42", "8: 42 | 42 8");
            source = source.Replace("11: 42 31", "11: 42 31 | 42 11 31");

            LoadRules(source.Split("\r\n").ToList());
            ParseRules();

            return ValidateMessges();
        }

        private void LoadRules(List<string> source)
        {
            Rules.Clear();
            Messages.Clear();

            foreach (var (l, search) in from l in source
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

        private void ParseRules()
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

        private string ValidateMessges()
        {
            var validMessages = (from Message in Messages
                                 let result = Rules[0].ParseRule(Message, (0, true))
                                 where (result.success) && (result.charIdx == Message.Length)
                                 select Message).Count();

            return $"Messages passing validation : {validMessages}";
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

        public (int charIdx, bool success) ParseRule(string Input, (int charIdx, bool success) from)
        {
            if (Char != null)
            {
                if (from.charIdx < Input.Length)
                    if (Input.Substring(from.charIdx, 1) == Char)
                        return (from.charIdx + 1, true);

                return (0, false);
            }

            var to = from;
            var passed = true;
            foreach (var subrule in SubRules)
            {
                to = subrule.ParseRule(Input, to);
                if (!to.success)
                {
                    passed = false;
                    break;
                }
            }

            if ((SubRulesAlt.Count > 0) && !passed)
            {
                passed = true;
                to = from;
                foreach (var subrule in SubRulesAlt)
                {
                    to = subrule.ParseRule(Input, to);
                    if (!to.success)
                    {
                        passed = false;
                        break;
                    }
                }
            }

            return (passed ? to.charIdx : from.charIdx, passed);
        }
    }
}