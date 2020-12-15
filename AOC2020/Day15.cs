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
            int Turns = 2020;
            PlayGame(Turns);
            return $"Number #{Turns} = {PreviousNums[Turns - 1]}";
        }

        public string Part2()
        {
            int Turns = 30000000;
            PlayGame(Turns);
            return $"Number #{Turns} = {PreviousNums[Turns - 1]}";
        }

        private void PlayGame(int Turns)
        {
            LoadData();
            var Turn = Nums.Count() + 1;

            while (Turn <= Turns)
            {
                var previousNumber = PreviousNums[Turn - 2];
                var timesSpoken = Nums[previousNumber].Split(',');

                if (timesSpoken.Length == 1)
                {
                    PreviousNums.Add(0);
                    Nums[0] = Nums[0] + $",{Turn}";
                }
                else if (timesSpoken.Length > 1)
                {
                    var i1 = int.Parse(timesSpoken[^1]);
                    var i2 = int.Parse(timesSpoken[^2]);

                    //Keep pruning the dictionary values to keep the split fast
                    Nums[previousNumber] = $"{i2},{i1}";

                    PreviousNums.Add(i1 - i2);
                    if (Nums.ContainsKey(i1 - i2))
                        Nums[i1 - i2] = Nums[i1 - i2] + $",{Turn}";
                    else
                        Nums[i1 - i2] = $"{Turn}";
                }

                Turn++;
            }
        }
    }
}