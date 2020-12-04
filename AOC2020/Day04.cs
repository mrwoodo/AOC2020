﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day04 : DayBase, ITwoPartQuestion
    {
        public string[] Lines;
        public List<Document> docs = new List<Document>();

        public Day04()
        {
            Lines = File.ReadAllText("Input\\Day04.txt").Split("\r\n\r\n");

            foreach (var line in Lines)
            {
                var doc = new Document();
                var attrs = line.Replace("\r\n", " ").Split(" ");

                foreach (var attr in attrs)
                {
                    var pos = attr.IndexOf(":");
                    var key = attr.Substring(0, pos);
                    var val = attr[(pos + 1)..];

                    doc.AddAttribute(key, val);
                }

                docs.Add(doc);
            }

            Console.WriteLine(Part1());
            Console.WriteLine(Part2());
        }

        public string Part1()
        {
            return docs.Count(i => i.IsValidVer1).ToString();
        }

        public string Part2()
        {
            return docs.Count(i => i.IsValidVer2).ToString();
        }
    }

    public class Document
    {
        private string _byr;
        public string byr
        {
            get { return _byr; }
            set
            {
                _byr = value;
                byrValid = ValidateNum(value, 1920, 2002);
            }
        }
        private bool byrValid = false;

        private string _iyr;
        public string iyr
        {
            get { return _iyr; }
            set
            {
                _iyr = value;
                iyrValid = ValidateNum(value, 2010, 2020);
            }
        }
        private bool iyrValid = false;

        private string _eyr;
        public string eyr
        {
            get { return _eyr; }
            set
            {
                _eyr = value;
                eyrValid = ValidateNum(value, 2020, 2030);
            }
        }
        private bool eyrValid = false;

        private string _hgt;
        public string hgt
        {
            get { return _hgt; }
            set
            {
                _hgt = value;
                if (value.EndsWith("cm"))
                    hgtValid = ValidateNum(value.Replace("cm", ""), 150, 193);
                else if (value.EndsWith("in"))
                    hgtValid = ValidateNum(value.Replace("in", ""), 59, 76);
            }
        }
        private bool hgtValid = false;

        private string _hcl;
        public string hcl
        {
            get { return _hcl; }
            set
            {
                _hcl = value;
                hclValid = (value.Length == 7) &&
                    (value[0] == '#') &&
                    ValidateChars(value[1..], "0123456789abcdef", 6);
            }
        }
        private bool hclValid = false;

        private string _ecl;
        public string ecl
        {
            get { return _ecl; }
            set
            {
                _ecl = value;
                var validECL = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
                eclValid = validECL.Contains(value);
            }
        }
        private bool eclValid = false;

        private string _pid;
        public string pid
        {
            get { return _pid; }
            set
            {
                _pid = value;
                pidValid = ValidateChars(value, "0123456789", 9);
            }
        }
        private bool pidValid = false;

        public string cid { get; set; }

        public void AddAttribute(string attr, string val)
        {
            this.GetType().GetProperty(attr).SetValue(this, val, null);
        }

        public bool IsValidVer1
        {
            get
            {
                if (string.IsNullOrEmpty(byr) ||
                    string.IsNullOrEmpty(iyr) ||
                    string.IsNullOrEmpty(eyr) ||
                    string.IsNullOrEmpty(hgt) ||
                    string.IsNullOrEmpty(hcl) ||
                    string.IsNullOrEmpty(ecl) ||
                    string.IsNullOrEmpty(pid))
                    return false;

                return true;
            }
        }

        public bool IsValidVer2 => IsValidVer1 && byrValid && eclValid && eyrValid && hclValid &&
                                    hgtValid && iyrValid && pidValid;

        private bool ValidateNum(string s, int min, int max)
        {
            if (int.TryParse(s, out int v))
                return (v >= min) && (v <= max);

            return false;
        }

        private bool ValidateChars(string s, string allowedChars, int? expectedLength = null)
        {
            if (expectedLength.HasValue)
                if (s.Length != expectedLength.Value)
                    return false;

            foreach (var c in s)
                if (!allowedChars.Contains(c))
                    return false;

            return true;
        }
    }
}