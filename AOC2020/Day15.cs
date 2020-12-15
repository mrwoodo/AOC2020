using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day15 : DayBase, ITwoPartQuestion
    {
        //Dict Key = Number spoken
        //Dict Value = Tuple (1st = Most recent turn spoken, 2nd = 2nd most recent turn spoken)
        //We keep popping turns off the tuple, as we don't care about turns older than 1st/2nd
        private Dictionary<int, (int?, int?)> Nums = new Dictionary<int, (int?, int?)>();
        private readonly IEnumerable<int> Input;

        public Day15()
        {
            Input = (from s in File.ReadAllText("Input\\Day15.txt").Split(",")
                         select int.Parse(s));

            Run(() => Part1(), () => Part2());
        }

        private void LoadData()
        {
            Nums = new Dictionary<int, (int?, int?)>();

            for (int i = 0; i < Input.Count(); i++)
                Nums.Add(Input.ElementAt(i), (i + 1, null));
        }

        public string Part1()
        {
            var turns = 2020;
            var nthNumberSpoken = PlayGame(turns);

            return $"Number #{turns} = {nthNumberSpoken}";
        }

        public string Part2()
        {
            var turns = 30000000;
            var nthNumberSpoken = PlayGame(turns);

            return $"Number #{turns} = {nthNumberSpoken}";
        }

        private int PlayGame(int Turns)
        {
            LoadData();
            var turn = Nums.Count() + 1;
            var prevSpoken = 0;

            while (turn <= Turns)
            {
                var timesSpoken = Nums[prevSpoken];

                if (timesSpoken.Item2.HasValue)
                {
                    prevSpoken = timesSpoken.Item1.Value - timesSpoken.Item2.Value;

                    if (Nums.ContainsKey(prevSpoken))
                        Nums[prevSpoken] = (turn, Nums[prevSpoken].Item1);
                    else
                        Nums[prevSpoken] = (turn, null);
                }
                else
                {
                    prevSpoken = 0;
                    Nums[0] = (turn, Nums[0].Item1);
                }

                turn++;
            }

            return prevSpoken;
        }
    }
}