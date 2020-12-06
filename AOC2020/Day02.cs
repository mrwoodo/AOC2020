using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day02 : DayBase, ITwoPartQuestion
    {
        public List<string> Lines = new List<string>();

        public Day02()
        {
            Lines = (from line in File.ReadAllLines("Input\\Day02.txt")
                    select line).ToList();

            Console.WriteLine(Part1());
            Console.WriteLine(Part2());
        }

        public string Part1()
        {
            var passwords = new List<PasswordVer1>();

            foreach (var line in Lines)
                passwords.Add(new PasswordVer1(line));

            return $"Valid passwords (Part 1) : {passwords.Where(i => i.IsValid).Count()}";
        }

        public string Part2()
        {
            var passwords = new List<PasswordVer2>();

            foreach (var line in Lines)
               passwords.Add(new PasswordVer2(line));

            return $"Valid passwords (Part 2) : {passwords.Where(i => i.IsValid).Count()}";
        }
    }

    public abstract class Password
    {
        public char Letter { get; set; }
        public string UserPassword { get; set; }
        public abstract bool IsValid { get; }

        public Password(string line)
        {
            var split = line.Split(" ");
            Letter = split[1][0];
            UserPassword = split[2];
        }
    }

    public class PasswordVer1 : Password
    {
        public int Min { get; }
        public int Max { get; }

        public PasswordVer1(string line) : base(line)
        {
            var split = line.Split(" ");
            var minmax = split[0].Split("-");
            Min = int.Parse(minmax[0]);
            Max = int.Parse(minmax[1]);
        }

        public override bool IsValid
        {
            get
            {
                var count = UserPassword.Count(i => i.Equals(Letter));

                return (count >= Min) && (count <= Max);
            }
        }
    }

    public class PasswordVer2 : Password
    {
        public int Char1 { get; }
        public int Char2 { get; }

        public PasswordVer2(string line) : base(line)
        {
            var split = line.Split(" ");
            var minmax = split[0].Split("-");
            Char1 = int.Parse(minmax[0]);
            Char2 = int.Parse(minmax[1]);
        }

        public override bool IsValid
        {
            get
            {
                var char1Valid = UserPassword[Char1 - 1].Equals(Letter);
                var char2Valid = UserPassword[Char2 - 1].Equals(Letter);

                return char1Valid ^ char2Valid; //one or the other, not both
            }
        }
    }
}