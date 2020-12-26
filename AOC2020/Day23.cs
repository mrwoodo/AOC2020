using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day23 : DayBase, ITwoPartQuestion
    {
        private LinkedList<int> Cups = new LinkedList<int>();
        private LinkedList<int> PickedUp = new LinkedList<int>();
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
                Cups.AddLast(c);

            CurrentCup = Cups.First;
            LowestCupVal = Cups.Min();
            HighestCupVal = Cups.Max();

            PlayMove(100);

            var result = "";
            foreach (var c in Cups)
                result += c.ToString();

            var start = result.IndexOf("1");
            return $"Cup Sequence = {result[(start + 1)..]}{result.Substring(0, start)}";
        }

        public string Part2()
        {
            foreach (var c in (from c in InputFile select int.Parse(c.ToString())))
                Cups.AddLast(c);

            var i = Cups.Max() + 1;
            for (int l = i; l <= 1000000; l++)
                Cups.AddLast(l);

            CurrentCup = Cups.First;
            LowestCupVal = Cups.Min();
            HighestCupVal = Cups.Max();

            PlayMove(10000000);

            return "";
        }

        private void PlayMove(int moves)
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
                    var search = Cups.Find(dest);
                    if (search != null)

                        return search;

                    dest--;
                }
            }
        }
    }
}