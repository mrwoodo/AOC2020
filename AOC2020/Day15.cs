using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day15 : DayBase, ITwoPartQuestion
    {
        private Dictionary<int, string> Nums = new Dictionary<int, string>();
        private List<int> PreviousNums = new List<int>();
        private IEnumerable<int> Input;

        public Day15()
        {
            Input = (from s in File.ReadAllText("Input\\Day15.txt").Split(",")
                         select int.Parse(s));
            Run(() => Part1(), () => Part2());
        }

        private void LoadData()
        {
            Nums = new Dictionary<int, string>();

            for (int i = 0; i < Input.Count(); i++)
                Nums.Add(Input.ElementAt(i), (i + 1).ToString());

            PreviousNums = Input.ToList();
        }

        public string Part1()
        {
            var turns = 2020;
            var result = PlayGame(turns);

            return $"Number #{turns} = {result}";
        }

        public string Part2()
        {
            var turns = 30000000;
            var result = PlayGame(turns);

            return $"Number #{turns} = {result}";
        }

        private int PlayGame(int Turns)
        {
            LoadData();
            var turn = Nums.Count() + 1;
            var prev = PreviousNums[turn - 2];

            while (turn <= Turns)
            {
                var timesSpoken = Nums[prev].Split(',');

                if (timesSpoken.Length == 1)
                {
                    prev = 0;
                    Nums[0] = Nums[0] + $",{turn}";
                }
                else if (timesSpoken.Length > 1)
                {
                    var i1 = int.Parse(timesSpoken[^1]);
                    var i2 = int.Parse(timesSpoken[^2]);

                    //Keep pruning the dictionary values to keep the split fast
                    Nums[prev] = $"{i2},{i1}";

                    prev = i1 - i2;

                    if (Nums.ContainsKey(i1 - i2))
                        Nums[i1 - i2] = Nums[i1 - i2] + $",{turn}";
                    else
                        Nums[i1 - i2] = $"{turn}";
                }

                turn++;
            }

            return prev;
        }
    }
}