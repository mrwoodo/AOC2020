using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    public class Day23 : DayBase, ITwoPartQuestion
    {
        private readonly LinkedList<int> Cups = new LinkedList<int>();
        private readonly LinkedList<int> PickedUp = new LinkedList<int>();
        //Find() in a linked-list is very slow - using a Cache to get around this
        private readonly Dictionary<int, LinkedListNode<int>> Cache = new Dictionary<int, LinkedListNode<int>>();
        private LinkedListNode<int> CurrentCup;
        private int LowestCupVal = 0;
        private int HighestCupVal = 0;
        private const int CUPS_MOVE = 3;

        public Day23()
        {
            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            foreach (var c in (from c in InputFile select int.Parse(c.ToString())))
                Cache[c] = Cups.AddLast(c);

            CurrentCup = Cups.First;
            LowestCupVal = Cups.Min();
            HighestCupVal = Cups.Max();

            Play(100);

            var result = "";
            foreach (var c in Cups)
                result += c.ToString();

            var start = result.IndexOf("1");
            return $"Cup Sequence = {result[(start + 1)..]}{result.Substring(0, start)}";
        }

        public string Part2()
        {
            Cache.Clear();
            Cups.Clear();
            PickedUp.Clear();

            foreach (var c in (from c in InputFile select int.Parse(c.ToString())))
                Cache[c] = Cups.AddLast(c);

            LowestCupVal = Cups.Min();
            HighestCupVal = 1000000;

            var i = Cups.Max() + 1;
            for (int l = i; l <= 1000000; l++)
                Cache[l] = Cups.AddLast(l);

            CurrentCup = Cups.First;

            Play(10000000);

            long ans1 = Convert.ToInt64(Cache[1].Next.Value);
            long ans2 = Convert.ToInt64(Cache[1].Next.Next.Value);

            return $"Cups after Cup1, {ans1} X {ans2} = {ans1 * ans2}";
        }

        private void Play(int moves)
        {
            for (int m = 0; m < moves; m++)
            {
                for (int i = 0; i < CUPS_MOVE; i++)
                {
                    var cup = Clockwise(CurrentCup);
                    PickedUp.AddLast(cup.Value);
                    Cups.Remove(cup);
                }

                var dest = DestinationCup(CurrentCup.Value - 1);
                for (int i = 0; i < CUPS_MOVE; i++)
                {
                    dest = Cups.AddAfter(dest, PickedUp.First());
                    Cache[dest.Value] = dest;
                    PickedUp.RemoveFirst();
                }

                CurrentCup = Clockwise(CurrentCup);
            }
        }

        private LinkedListNode<int> Clockwise(LinkedListNode<int> cup)
        {
            //Not bothering with circular list
            if (cup.Next == null)
                return cup.List.First;

            return cup.Next;
        }

        private LinkedListNode<int> DestinationCup(int dest)
        {
            while (true)
            {
                if (PickedUp.Contains(dest))
                    dest--;

                if (dest < LowestCupVal)
                    dest = HighestCupVal;

                if (!PickedUp.Contains(dest))
                {
                    if (Cache.ContainsKey(dest))
                        return Cache[dest];

                    dest--;
                }
            }
        }
    }
}