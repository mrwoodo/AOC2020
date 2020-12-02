using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day01 : DayBase, ITwoPartQuestion
    {
        private readonly List<int> expenses;
        private readonly int targetAmount = 2020;

        public Day01()
        {
            expenses = (from line in File.ReadAllLines("Day01.txt")
                        select int.Parse(line)).ToList();

            Console.WriteLine(Part1());
            Console.WriteLine(Part2());
        }

        public string Part1()
        {
            for (int i = 0; i < expenses.Count; i++)
            {
                for (int j = 0; j < expenses.Count; j++)
                {
                    if (i != j)
                    {
                        var x = expenses[i];
                        var y = expenses[j];

                        if (x + y == targetAmount)
                            return $"{x} x {y} = {x * y}";
                    }
                }
            }
            return string.Empty;
        }

        public string Part2()
        {
            for (int i = 0; i < expenses.Count; i++)
            {
                for (int j = 0; j < expenses.Count; j++)
                {
                    for (int k = 0; k < expenses.Count; k++)
                    {
                        if ((i != j) && (i != k) && (k != j))
                        {
                            var x = expenses[i];
                            var y = expenses[j];
                            var z = expenses[k];

                            if (x + y + z == targetAmount)
                                return $"{x} x {y} x {z} = {x * y * z}";
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}