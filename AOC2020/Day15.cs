using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day15 : DayBase, ITwoPartQuestion
    {
        public List<int> Nums = new List<int>();

        public Day15()
        {
            Nums = (from s in File.ReadAllText("Input\\Day15.txt").Split(",")
                    select int.Parse(s)).ToList();

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            int searchNum = 2020;
            int Turn = Nums.Count() + 1;

            while (Turn <= searchNum)
            {
                var previousNumber = Nums[Turn - 2];
                var timesSpoken = Nums.Where(i => i == previousNumber).Count();

                if (timesSpoken == 1)
                {
                    Nums.Add(0);
                }
                else if (timesSpoken > 1)
                {
                    var lastIndex1 = Nums.FindLastIndex(i => i == previousNumber);
                    var lastIndex2 = Nums.GetRange(0, lastIndex1).FindLastIndex(i => i == previousNumber);

                    Nums.Add(lastIndex1 - lastIndex2);
                }

                Turn++;
            }

            return $"Number #{searchNum} = {Nums[searchNum - 1]}";
        }

        public string Part2()
        {
            return "";
        }
    }
}